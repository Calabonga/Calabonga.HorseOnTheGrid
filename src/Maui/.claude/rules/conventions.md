## Именование

### Общие
- PascalCase — классы, `record`, методы, свойства; camelCase — локальные переменные и параметры; `_camelCase` — приватные поля.

### Типы проекта
- Сообщения: `[Название]CommandMessage` (команда) или `[Название]NotificationMessage` / `[Название]Message` (уведомление), в `Core/Messaging`.
- ViewModel: `[Название]ViewModel`.
- Диалоги: пара `[Название]DialogView` (`Views/Dialogs`) и `[Название]DialogResult` (`ViewModels/Dialogs`).
- Представления: `[Название]View` + `[Название]View.xaml.cs`.

### Префиксы и суффиксы
- `I` — интерфейсы (`IDashboardZoneView`).
- `T` — обобщённые параметры (`TResult`).
- `Base` — суффикс базовых классов (`GitLabRequestBase`).
- `Factory` — суффикс фабрик.
- `Converter` — суффикс XAML-конвертеров (`DateFormatConverter`).

### Файлы и папки
- Один тип — один файл, имя файла совпадает с именем типа.
