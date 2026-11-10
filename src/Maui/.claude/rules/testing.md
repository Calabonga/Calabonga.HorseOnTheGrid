## Договорённости по тестированию

Тесты лежат в `tests/Calabonga.HorseOnTheGrid.Core.Tests` и используют xUnit. В тестах запрещены `async void` и `Thread.Sleep`. Асинхронные тесты — `async Task`, синхронные — `void`.

### Frameworks
- xUnit (+ `xunit.runner.visualstudio`).
- Moq и AutoFixture не подключены: `Core` не имеет внешних зависимостей, мокать нечего. Добавлять их можно только вместе с тестовым проектом для `Mobile`, по согласованию.

### Что тестировать
- Unit-тесты обязательны для логики `Core`: `Game`, `Board`, `KnightMoves`, `Position`, `CommandHistory`.
- Новая игровая логика не принимается без теста.
- Не требуют тестов: XAML и code-behind (`GamePage.xaml.cs`), `MauiProgram.cs` и регистрация DI, `Platforms/`, стили и ресурсы (`Colors.xaml`, `Styles.xaml`), `App.xaml.cs`.
- Тесты `Core.Tests` не должны зависеть от MAUI и платформы. К `internal`-членам ядра они допущены через `InternalsVisibleTo`.
- Тесты должны быть детерминированными: любые случайные ходы — с фиксированным начальным значением.

### Шаблоны для именования (для новых тестов)
- `MethodName_Should_ExpectedBehavior_When_Condition`, например `TryMove_Should_ReturnFalse_When_CellAlreadyVisited`.
- `MethodName_ShouldNot_ExpectedBehavior_When_Condition`, например `Undo_ShouldNot_RemoveStartCell_When_OnlyStartVisited`.
- Существующие тесты ради имён не переименовывай.
- Структура теста: `// arrange`, `// act`, `// assert`.
