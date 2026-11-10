# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Обзор

«Конь на доске» — Android-приложение на .NET MAUI (.NET 10): шахматный конь обходит все 100 клеток поля 10×10, ходя буквой «Г» и не возвращаясь на посещённые клетки. Это эталонное решение к курсу на Stepik «Простое приложение на C# и MAUI за 12 часов». Решение состоит из ядра игры без зависимостей от MAUI, мобильного приложения и модульных тестов ядра. Весь код, комментарии и строки интерфейса — на русском; `ApplicationId` — `net.calabonga.horseonthegrid`. Подробности для пользователя — в `README.md`, публикация в RuStore — в `docs/rustore-publishing.md`.

## Команды

Все команды выполняются из папки `src/Maui` (PowerShell на Windows 11).

```bash
# Тесты ядра (xUnit, net10.0; нагрузка MAUI не нужна)
dotnet test tests/Calabonga.HorseOnTheGrid.Core.Tests

# Сборка и запуск на эмуляторе/телефоне (нужна нагрузка maui-android)
dotnet build src/Calabonga.HorseOnTheGrid.Mobile -f net10.0-android -t:Run
```

Один раз после клонирования — `pwsh -ExecutionPolicy Bypass -File ./setup.ps1`: без него сборка `Mobile` падает с `MSB3954` (нет иконки, заставки, шрифтов, `AndroidManifest.xml`). Скрипт берёт файлы из временного проекта `dotnet new maui` и не перезаписывает существующие; `NETSDK1147` во время его работы ожидаема. Выпуск подписанного APK и загрузка в RuStore описаны в `docs/rustore-publishing.md`.

## Архитектура

Решение `Calabonga.HorseOnTheGrid.slnx`, три проекта:

- `src/Calabonga.HorseOnTheGrid.Core` (`net10.0`) — вся логика игры, ни одной ссылки на MAUI:
  - `Position` — `readonly record struct (Column, Row)`, индексы с нуля; `ToString()` печатает с единицы (`x:1, y:1`).
  - `KnightMoves` — `From(pos, size)` перечисляет клетки в пределах поля; `IsKnightMove` проверяет ход по условию `|Δколонок| * |Δстрок| == 2`.
  - `Board` — массив номеров ходов (`0` = не посещена), `Current`, `VisitedCount`; `Visit`/`Unvisit`/`Clear` — `internal`, снаружи ядра поле менять нельзя. Минимальный размер — 5.
  - `Commands` — паттерн «Команда»: `IGameCommand`, `MoveCommand` (internal), `CommandHistory` (стек, отмена с конца).
  - `Game` — единственная точка входа для UI: `TryMove`, `Undo`, `Start`, `State` (`InProgress` / `Won` / `Stuck`), `MoveCount`, `CanUndo`, событие `Changed`.
- `src/Calabonga.HorseOnTheGrid.Mobile` (`net10.0-android`, `UseMaui`, `SingleProject`) — приложение:
  - MVVM на CommunityToolkit.Mvvm 8.4.0 с генераторами исходного кода (`[ObservableProperty]`, `[RelayCommand]`, `[NotifyPropertyChangedFor]`); классы моделей — `partial`.
  - `GameViewModel` держит `Game` и 100 `CellViewModel`, подписан на `Game.Changed` и в `Refresh()` пересчитывает `CellState` (`Empty` / `Available` / `Visited` / `Current`), `ProgressText`, `StatusText`, `IsGameOver`, рекорд. Правил игры не знает.
  - `GamePage.xaml` — один экран в `AppShell` (`Shell.FlyoutBehavior="Disabled"`, маршрут `game`). Поле — `Grid` 10×10 с `BindableLayout.ItemsSource`, вид клетки переключают `DataTrigger` по `CellState`; нажатие — `TapGestureRecognizer` → `SelectCommand`. Привязки скомпилированные (`x:DataType`).
  - Стили и палитра — `Resources/Styles/Colors.xaml` и `Styles.xaml` (цвета calabonga.net: `BrandOrange #FF7200`, `PlateBlue #0070C0` и др.).
  - DI в `MauiProgram.cs`: `Preferences.Default` (singleton), `Game`, `GameViewModel`, `GamePage` — transient. Логирование — `Microsoft.Extensions.Logging.Debug` только в `DEBUG`.
  - Рекорд хранится в `IPreferences` по ключу `best_result` и переживает перезапуск.
- `tests/Calabonga.HorseOnTheGrid.Core.Tests` (xUnit 2.9.3) — `GameTests`, `KnightMovesTests`; к `internal` членам ядра тесты допущены через `InternalsVisibleTo`. Тест `WarnsdorffRule_VisitsAllCells_AndGameIsWon` доказывает, что обход всех 100 клеток существует.

Поток данных: нажатие на клетку → `CellViewModel.SelectCommand` → `GameViewModel.OnCellSelected` → `Game.TryMove` → `CommandHistory.Execute(MoveCommand)` → `Board.Visit` → `Game.UpdateState` → `Changed` → `GameViewModel.Refresh`.

### Что важно знать

- Новая логика игры — только в `Core` и с тестом; `Mobile` лишь отображает состояние. Модели представления не должны дублировать правила хода.
- Стартовая клетка `(0, 0)` посещается в `Game.Start()`, поэтому `MoveCount` включает её, а `Undo` стартовую клетку не отменяет.
- Отмена хода работает и после тупика (`Stuck`), и после победы; `CanMoveTo` разрешает ход только при `InProgress`.
- `Board.Unvisit` бросает исключение, если отменяется не последний ход: отмена идёт только через `CommandHistory`.
- Квадратность поля обеспечивает код страницы `GamePage.xaml.cs` (`OnBoardHostSizeChanged`: сторона = меньшая из сторон области), а не XAML и не модель представления.
- Приложение только в портретной ориентации (`ScreenOrientation.Portrait` в `MainActivity`) и только в светлой теме (`UserAppTheme = AppTheme.Light` в `App.xaml.cs`): палитра рассчитана на светлую.
- `TargetFrameworks` намеренно содержит только `net10.0-android`: платформа без установленной нагрузки даёт `NETSDK1147`; как добавить другие — в комментарии в `.csproj`.
- Псевдонимы шрифтов `AppRegular` / `AppSemibold` объявлены в `MauiProgram.cs` и используются в `Styles.xaml`; файлы `OpenSans-*.ttf` приходят из `setup.ps1`.
- `ApplicationId` менять нельзя после первой публикации; `ApplicationVersion` (номер сборки) должен расти с каждой загрузкой в магазин, `ApplicationDisplayVersion` — версия для людей.
- Ключи подписи (`*.keystore`, `*.jks`, `*.p12`, `*.pfx`) и пакеты (`*.apk`, `*.aab`, `publish/`) в `.gitignore`; пароли и ключи не коммитятся и не пишутся в файлы — только в переменные окружения.
- Кириллица и пробелы в пути к папке ломают сборку MAUI — клонируйте в короткий латинский путь.
