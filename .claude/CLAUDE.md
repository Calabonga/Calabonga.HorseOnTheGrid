# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Обзор

Calabonga.HorseOnTheGrid — игра-головоломка «Конь на доске»: шахматный конь должен обойти все клетки поля 10×10, ходя буквой «Г» и не возвращаясь на посещённые клетки. Репозиторий содержит три независимые реализации одной игры (общего кода между ними нет): `src/Wpf` (WPF без MVVM, v1.0), `src/WpfMvvm` (WPF + MVVM, v2.0, .NET 9) и `src/Maui` (Android на .NET MAUI, .NET 10; эталонное решение к курсу на Stepik). Язык — C#, комментарии и UI-строки в `src/Maui` — на русском. CI-конфигураций (`.github/`) нет.

## Команды

Командной строкой собирается только `src/Maui`; WPF-проекты требуют Windows (`net9.0-windows`) и открываются через `.sln` в Visual Studio.

```bash
# Тесты ядра MAUI-версии (xUnit, net10.0; платформенные нагрузки не нужны)
dotnet test src/Maui/tests/Calabonga.HorseOnTheGrid.Core.Tests

# Один тест
dotnet test src/Maui/tests/Calabonga.HorseOnTheGrid.Core.Tests --filter "FullyQualifiedName~GameTests"

# Сборка и запуск на эмуляторе/телефоне (нужна рабочая нагрузка maui-android)
dotnet build src/Maui/src/Calabonga.HorseOnTheGrid.Mobile -f net10.0-android -t:Run
```

Сборка `Mobile` на чистом клоне падает с `MSB3954` (нет `Resources\Splash\splash.svg` и др.), пока не выполнен `src/Maui/setup.ps1` (PowerShell, один раз): он создаёт временный проект из шаблона `dotnet new maui` и копирует из него `Platforms`, `Properties`, иконку, заставку и шрифты, не перезаписывая существующие файлы. Ошибка `NETSDK1147` во время работы скрипта ожидаема.

Подписанный APK собирается через `dotnet publish ... -f net10.0-android -c Release` с параметрами `AndroidKeyStore`/`AndroidSigningKeyStore`/…; пароль берётся из переменной окружения `HORSE_KEY_PASS` (полная команда — в `src/Maui/README.md`). Файлы `*.keystore`, `*.jks`, `*.apk` в `.gitignore`.

## Архитектура

### src/Maui (основная, актуальная версия)

Решение `Calabonga.HorseOnTheGrid.slnx` из трёх проектов:

- `Core` (`net10.0`, без зависимостей от MAUI) — вся логика игры:
  - `Position` — `readonly record struct (Column, Row)`, индексы с нуля; `ToString()` выводит координаты с единицы (`x:1, y:1`).
  - `KnightMoves` — смещения коня; `From(pos, size)` перечисляет клетки в пределах поля, `IsKnightMove` проверяет ход через произведение модулей смещений `== 2`.
  - `Board` — массив номеров ходов (`0` = не посещена), `Current`, `VisitedCount`; `Visit`/`Unvisit`/`Clear` — `internal`.
  - `Commands` — паттерн «Команда»: `IGameCommand`, `MoveCommand` (internal), `CommandHistory` (стек).
  - `Game` — фасад для UI: `TryMove`, `Undo`, `Start`, `State` (`GameState`: `InProgress`/`Won`/`Stuck`), событие `Changed`.
- `Mobile` (`net10.0-android`, `UseMaui`) — MVVM на CommunityToolkit.Mvvm (source-generators: `[ObservableProperty]`, `[RelayCommand]`). `GameViewModel` держит `Game` и список `CellViewModel`, на событие `Game.Changed` вызывает `Refresh()`, который пересчитывает `CellState` (`Empty`/`Available`/`Visited`/`Current`), тексты и рекорд. Зависимости регистрируются в `MauiProgram.cs` (`Preferences.Default`, `Game`, `GameViewModel`, `GamePage` — `Transient`). Рекорд хранится в `IPreferences` под ключом `best_result`.
- `Core.Tests` (xUnit) — `GameTests`, `KnightMovesTests`; доступ к `internal` членам ядра выдан через `InternalsVisibleTo`.

Поток данных: нажатие на клетку → `CellViewModel.SelectCommand` → `GameViewModel.OnCellSelected` → `Game.TryMove` → `CommandHistory.Execute(MoveCommand)` → `Board.Visit` → `Game.UpdateState` → `Changed` → `GameViewModel.Refresh`.

### src/Wpf и src/WpfMvvm (учебные версии v1.0 и v2.0)

- `Wpf`: логика в `HorseGame` (массив `CellType[,] Grid`, связный список `Position`), `MainWindow.xaml.cs` создаёт поле и вызывает `MoveHorse(x, y, textBlock)`; `Direction` и `Position.CheckDirection` вычисляют доступные ходы.
- `WpfMvvm`: `MainWindowViewModel` (CommunityToolkit.Mvvm) + `Game/HorseGame` (список `GridCell`-контролов, расширения `GridCellExtensions.FindDirectionMoves`), DI через `Core/DependencyContainer.cs` (`Microsoft.Extensions.DependencyInjection`, Serilog в файл, `Logging.Debug`).

### Что важно знать

- Стартовая клетка коня — `(0, 0)` (левый верхний угол); `Game` сразу делает её посещённой, поэтому `MoveCount` включает старт, а `Undo` стартовую клетку не отменяет.
- Состояние `Stuck` и `Won` не блокируют отмену хода: `Game.Undo` работает и после тупика, и после победы; `CanMoveTo` при этом возвращает `false` при любом состоянии, кроме `InProgress`.
- `Board.Unvisit` бросает исключение, если отменяется не последний ход, — отмена возможна только через `CommandHistory` в порядке LIFO.
- `Game` в DI регистрируется как `Transient` через фабрику (`new Game()`), а не как singleton: у каждой `GameViewModel` своя партия.
- `Mobile.csproj` намеренно собирается только под `net10.0-android`: добавление платформы без установленной рабочей нагрузки даёт `NETSDK1147`; подсказка — в комментарии в самом `.csproj`. Псевдонимы шрифтов `AppRegular`/`AppSemibold` из `MauiProgram.cs` используются в `Styles.xaml`.
- В `src/Maui` в git нет `AndroidManifest.xml`, иконки, заставки и шрифтов — они появляются только после `setup.ps1`; не добавляйте их в репозиторий без необходимости.
- В `src/Wpf` пространство имён с опечаткой — `Calabonga.HorseOntTheGrid` (лишняя `t`), при этом сборка называется `Calabonga.HorseOnTheGrid`; опечатку сохраняйте при правках. Там же `Position(Row, Column)`, тогда как в `Maui` `Position(Column, Row)` — порядок аргументов в версиях разный.
- Папка с кириллицей или пробелами в пути приводит к ошибкам сборки MAUI; клонируйте в короткий латинский путь.
- `.editorconfig` лежит в корне репозитория и действует на все три реализации.
