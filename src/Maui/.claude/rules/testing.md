## Договорённости по тестированию

Тесты находятся в проекте `Tests` и используют фреймворк xUnit. В тестах запрещено использовать `async void` и `Thread.Sleep`. Для асинхронных тестов используй `async Task`. Для синхронных тестов используй `void`.

### Frameworks
- xUnit
- Moq
- AutoFixture (`AutoFixture.AutoMoq`)

### Что тестировать
- Unit-тесты обязательны для логики.
- Не требуют тестов: XAML и code-behind представлений, конфигурация и регистрация DI (`DependencyContainer`), чтение `settings.env`, темы, ресурсы.
- Внешние зависимости (файловая система, `ILogger`) изолируй через Moq.

### Шаблоны для именования
- `MethodName_Should_ExpectedBehavior_When_Condition` (например, `BuildUrl_Should_ReturnUrl_When_ParametersSet`)
- `MethodName_ShouldNot_ExpectedBehavior_When_Condition` (например, `Handle_ShouldNot_ThrowException_When_ItemsEmpty`)
- Структура теста: `// arrange`, `// act`, `// assert`.
