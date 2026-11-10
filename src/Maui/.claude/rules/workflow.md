## Правила рабочего процесса

- Перед внесением изменений всегда создавай отдельную ветку Git. Допустимые префиксы: `feature/`, `bugfix/`, `hotfix/`.
- Формат коммитов (Conventional Commits): `type: description`, где type — `feat`, `fix`, `refactor`, `test`, `docs`, `style`, `perf`, `build`, `ci`, `chore`, `revert`.
- Создавай атомарные коммиты: одно логическое изменение на коммит, с понятным сообщением.
- Запускай `dotnet test "src/Maui/tests/Calabonga.HorseOnTheGrid.Core.Tests/Calabonga.HorseOnTheGrid.Core.Tests.csproj"` после каждой реализации и перед коммитом; не ломай сборку (`dotnet build src/Maui/src/Calabonga.HorseOnTheGrid/Calabonga.HorseOnTheGrid.csproj`).
- Перед коммитом убедись, что код соответствует `rules/code-styles.md` и `rules/conventions.md`.
- Если нужно создать новый класс, сначала проверь, нет ли в решении файла с таким же названием.
- Проверяй наличие новых версий зависимостей. Если есть обновления — спроси разрешение и обновляй при необходимости.
- Если в коде встречается «Calabonga: Summary required» — замени «Summary» на описание на английском языке для этого метода (класса, свойства, переменной).
- При изменении версии приложения обновляй её во всех местах: `<Version>` в `src/Maui/src/Calabonga.HorseOnTheGrid/Calabonga.HorseOnTheGrid.csproj`, бейдж и «Release notes» в `README.md` и `README.ru.md`, `build/release-notes.md`.
- Каталоги `publish/` и `releases/` в git не добавляй; `dist/` коммитится только при выпуске релиза.
