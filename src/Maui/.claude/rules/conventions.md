## Именование

### Общие
- PascalCase — классы, `record`, методы, свойства; camelCase — локальные переменные и параметры; `_camelCase` — приватные поля.

### Структура решения
- `src/Calabonga.HorseOnTheGrid.Core` — логика игры, корневое пространство имён `Calabonga.HorseOnTheGrid.Core`.
- `src/Calabonga.HorseOnTheGrid.Mobile` — приложение; пространства имён совпадают с папками: `.ViewModels`, `.Views`.
- `tests/Calabonga.HorseOnTheGrid.Core.Tests` — тесты `Core`, файл `<Тип>Tests.cs` для `<Тип>`.

### Типы проекта
- Страницы: `[Название]Page` (`GamePage`) + `[Название]Page.xaml.cs`, папка `Views`.
- ViewModel: `[Название]ViewModel`, папка `ViewModels`.
- Перечисления состояния представления лежат рядом с моделями (`CellState`); состояние партии — `GameState` в `Core`.
- Команды игры: `[Действие]Command`, реализуют `IGameCommand` (`MoveCommand`).
- Команды MVVM — методы с `[RelayCommand]`: `Select()` даёт `SelectCommand`.

### Префиксы и суффиксы
- `I` — интерфейсы (`IGameCommand`).
- `T` — обобщённые параметры (`TResult`).
- `Base` — суффикс базовых классов.
- `Converter` — суффикс XAML-конвертеров (при появлении).

### Файлы и папки
- Один тип — один файл, имя файла совпадает с именем типа. Новые типы не добавляй в чужие файлы; существующий `Commands.cs` разносить по файлам только отдельной задачей.
- Ресурсы: цвета — `Resources/Styles/Colors.xaml`, стили — `Styles.xaml`; шрифты — псевдонимами `AppRegular`/`AppSemibold`.
