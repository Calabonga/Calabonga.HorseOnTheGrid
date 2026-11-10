# Tasks

Change описывает существующее поведение и не меняет код, поэтому задачи — проверка соответствия спецификаций коду и ввод их в основные спецификации.

## 1. Проверка спецификаций ядра

- [x] 1.1 Сверить `specs/knight-moves/spec.md` с `Core/KnightMoves.cs`, `Core/Position.cs` и `KnightMovesTests`; убедиться, что каждый сценарий (угол — 2 хода, центр — 8, нумерация с единицы) подтверждён кодом или тестом
- [x] 1.2 Сверить `specs/game-session/spec.md` с `Core/Game.cs`, `Core/Board.cs`, `Core/Commands.cs` и `GameTests`; выполнить `dotnet test tests/Calabonga.HorseOnTheGrid.Core.Tests` и убедиться, что все тесты проходят

## 2. Проверка спецификаций экрана и рекорда

- [x] 2.1 Сверить `specs/board-screen/spec.md` с `GamePage.xaml`, `GamePage.xaml.cs`, `GameViewModel.cs`, `App.xaml.cs`, `MainActivity.cs`: тексты состояния, состояния кнопки «Отменить ход», портретная ориентация, светлая тема
- [x] 2.2 Сверить `specs/best-result/spec.md` с `GameViewModel.cs`: рекорд 1 при первом запуске, неуменьшаемость, ключ `best_result` в `IPreferences`
- [x] 2.3 Для сценариев экрана и рекорда, которые нельзя проверить чтением кода (подсветка, поворот устройства, тёмная тема, перезапуск), прогнать приложение на эмуляторе по командам из `README.md` и отметить результат; расхождения со спецификацией записать в этот файл

## 3. Валидация и ввод в основные спецификации

- [x] 3.1 Выполнить `openspec validate document-maui-app-specs --strict` и убедиться, что нет ошибок и предупреждений
- [x] 3.2 Выполнить `/opsx:sync` (или архивацию) и убедиться, что в `openspec/specs/` появились четыре спецификации: `knight-moves`, `game-session`, `board-screen`, `best-result`
- [x] 3.3 Добавить в `context:` файла `openspec/config.yaml` ограничения, которые нельзя вывести из кода (новая логика только в `Core` с тестом; артефакты на русском; публикация только в RuStore), и убедиться, что `openspec instructions apply --change <имя> --json` отдаёт их в поле `context`

## Workflow follow-up

- Архивировать change после проверки спецификаций.
- Для `board-screen` и `best-result` автоматических тестов нет: при желании завести отдельный change на тесты модели представления.
