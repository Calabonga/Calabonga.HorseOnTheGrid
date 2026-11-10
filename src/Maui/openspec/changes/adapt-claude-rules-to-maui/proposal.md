# Proposal

## Why

Правила в `.claude/rules/` (`workflow.md`, `testing.md`, `code-styles.md`, `conventions.md`) перенесены из другого проекта (дашборд с GitLab, `DependencyContainer`, `ViewModelBase`, `WeakReferenceMessenger`, `settings.env`). В MAUI-проекте этих сущностей нет, пути сборки и тестов указывают на несуществующие файлы, а часть требований (комментарии на английском, Moq/AutoFixture, `README.ru.md`) противоречит `CLAUDE.md` и реальному коду. Агент получает неверные команды и ложные ориентиры, поэтому правила нужно привести в соответствие с проектом.

## What Changes

- `workflow.md`: исправить команды сборки и тестов (запуск из `src/Maui`, проект `Mobile` с `-f net10.0-android`); переписать правило версии на `ApplicationDisplayVersion`/`ApplicationVersion`; описание «Summary required» — на русском; убрать несуществующие `README.ru.md`, `build/release-notes.md`, `dist/`; добавить блок про OpenSpec и запрет коммитить ключи и пакеты.
- `testing.md`: указать реальный проект `Core.Tests`; убрать Moq и AutoFixture; заменить список исключений на MAUI-специфичный; шаблон имён `Should/ShouldNot` — только для новых тестов; тесты `Core` не зависят от MAUI.
- `code-styles.md`: убрать `ViewModelBase`, `WeakReferenceMessenger`, `DependencyContainer`, GitLab; добавить раздел «Архитектура» (граница `Core`/`Mobile`); регистрацию — в `MauiProgram.cs`; комментарии и XML-doc на русском; `sealed` допустим для `partial`-моделей; async-правила и `TimeProvider` оставить как заготовку; правила действуют для нового и изменяемого кода.
- `conventions.md`: заменить типы проекта реальными (`GamePage`, `GameViewModel`, `CellViewModel`, `CellState`, `*Command`); убрать сообщения, диалоги, `Factory`; описать структуру решения; оговорка про существующий `Commands.cs`.
- Код приложения и тестов не меняется; существующий код под правила не переписывается.

## Capabilities

### New Capabilities

Нет: изменение касается только инструкций для агента, поведение приложения не меняется.

### Modified Capabilities

Нет. В `.openspec.yaml` задано `skip_specs: true`.

## Impact

- Затронуты только файлы `src/Maui/.claude/rules/workflow.md`, `testing.md`, `code-styles.md`, `conventions.md`.
- Исходный код, `.csproj`, зависимости и спецификации в `openspec/specs/` не затрагиваются.
- Вне объёма: рефакторинг `Commands.cs` на отдельные файлы, `sealed` для `GameViewModel`, переименование существующих тестов, тестовый проект для `Mobile`.
