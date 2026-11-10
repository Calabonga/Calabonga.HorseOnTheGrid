# Tasks

## 1. workflow.md

- [x] 1.1 Переписать `.claude/rules/workflow.md`: разделы Git, Проверка перед коммитом (команды из `src/Maui`, `dotnet test tests/Calabonga.HorseOnTheGrid.Core.Tests`, `dotnet build src/Calabonga.HorseOnTheGrid.Mobile -f net10.0-android`), OpenSpec, Прочее, Версия приложения; убрать `README.ru.md`, `build/release-notes.md`, `dist/`, `releases/`. Проверка: в файле нет путей, которых нет в репозитории (`ls` каждого упомянутого пути).
- [x] 1.2 Привести правило «Summary required» к описанию на русском и добавить запрет коммитить `publish/`, `*.apk`, `*.aab`, ключи подписи. Проверка: формулировки не противоречат `CLAUDE.md` и `.gitignore`.

## 2. testing.md

- [x] 2.1 Переписать `.claude/rules/testing.md`: проект `tests/Calabonga.HorseOnTheGrid.Core.Tests`, только xUnit, MAUI-специфичный список исключений, шаблон имён для новых тестов, детерминированность. Проверка: `dotnet test tests/Calabonga.HorseOnTheGrid.Core.Tests` проходит, список пакетов в правиле совпадает с `Core.Tests.csproj`.

## 3. code-styles.md

- [x] 3.1 Переписать `.claude/rules/code-styles.md`: раздел «Архитектура» (`Core` без MAUI, `Mobile` без правил игры), MVVM на `ObservableObject` и CommunityToolkit.Mvvm, регистрация в `MauiProgram.cs`, комментарии на русском, оговорки про `sealed partial`, `var`, `CanUndo`. Проверка: нет упоминаний `ViewModelBase`, `WeakReferenceMessenger`, `DependencyContainer`, GitLab.
- [x] 3.2 Оформить async, `CancellationToken` и `TimeProvider` как условный раздел «когда появится». Проверка: раздел помечен и не требует правок существующего кода.

## 4. conventions.md

- [x] 4.1 Переписать `.claude/rules/conventions.md`: структура решения, реальные типы (`GamePage`, `GameViewModel`, `CellViewModel`, `CellState`, `*Command`), оговорка про `Commands.cs`. Проверка: каждое упомянутое имя типа находится поиском по `src/`.

## 5. Итоговая проверка

- [x] 5.1 Сверить четыре файла между собой и с `CLAUDE.md`, `openspec/config.yaml`: нет противоречий по языку комментариев, путям и командам. Проверка: `grep -rnE "GitLab|ViewModelBase|WeakReferenceMessenger|DependencyContainer|Moq|AutoFixture|README.ru|release-notes" .claude/rules` ничего не находит.
- [x] 5.2 Выполнить `openspec validate adapt-claude-rules-to-maui` и убедиться, что проверка проходит. Проверка: команда завершается без ошибок.

## Workflow follow-up

- Создать ветку `feature/adapt-claude-rules` перед правками; коммит в формате `docs: ...`.
- Заархивировать change после ревью.
