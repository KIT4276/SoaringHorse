# AGENT.md

## Кратко О Проекте

- Проект: `SoaringHorse`
- Движок: Unity `6000.0.29f1`
- Рендер-пайплайн: URP
- Основной стек: `Zenject`, Unity Input System, DOTween, интеграция Yandex Games WebGL
- Основные сцены в билде:
  - `Assets/SoaringHorse/Scenes/Bootstrap.unity`
  - `Assets/SoaringHorse/Scenes/GameScene.unity`

Это небольшой Unity-проект, построенный вокруг dependency injection и простой state machine. Большая часть игрового кода находится в `Assets/SoaringHorse/Scripts`.

## Где Что Лежит

- `Assets/SoaringHorse/Scripts/Core`
  - жизненный цикл приложения, сервисы, состояния, runtime-системы
- `Assets/SoaringHorse/Scripts/Installers`
  - Zenject-байндинги для bootstrap, gameplay, platform и config
- `Assets/SoaringHorse/Scripts/Gameplay`
  - логика героя, генерация окружения, UI
- `Assets/SoaringHorse/Scripts/Data`
  - ScriptableObject-конфиги, модели прогресса, DTO сохранений
- `Assets/SoaringHorse/Scripts/Platform/Yandex`
  - web/Yandex SDK bridge, реклама, cloud save
- `Assets/SoaringHorse/Config`
  - ScriptableObject-ассеты, используемые инсталлерами
- `Assets/SoaringHorse/Resources/ProjectContext.prefab`
  - глобальный Zenject project context
- `Assets/SoaringHorse/Prefabs`
  - runtime-prefабы героя, UI, окружения и платформенных сервисов

## Заметки По Архитектуре

### Поток Запуска

1. Unity запускает `Bootstrap.unity`.
2. `GameStateMachine` входит в `BootstrapState`.
3. Bootstrap инициализирует платформенные сервисы и стартовый UI.
4. При старте новой игры или загрузке сохранения происходит переход через `LoadSceneState`.
5. Основной игровой цикл живёт в `GameScene.unity`.

### Dependency Injection

Zenject здесь является composition root. Предпочтительно связывать зависимости через installers, а не через scene lookup или статические singleton-ы.

Ключевые installers:

- `BootstrapUIInstaller`
- `ConfigInstaller`
- `CoreServicesInstaller`
- `EnvironmentInstaller`
- `GameStateInstaller`
- `GameplaySystemsInstaller`
- `InputlayInstaller`
- `PlatformInstaller`

Если добавляется новый сервис или система, биндинг лучше размещать в наиболее локальном installer-е, который владеет этой ответственностью.

### State Machine

Основной игровой flow управляется `GameStateMachine` и состояниями из `Assets/SoaringHorse/Scripts/Core/States`.

Основные состояния:

- `BootstrapState`
- `LoadSceneState`
- `GameStartState`
- `GameplayState`
- `PauseState`

Если меняется flow запуска, тайминг переходов между сценами или поведение паузы, стоит смотреть на эти состояния в связке, а не править один файл изолированно.

### Сохранения И Прогресс

Основные сущности, связанные с сохранением:

- `SaveService`
- `RunSaveData`
- `IRunProgress` / `RunProgress`
- `ProgressSyncService`
- `ProgressAutosave`

Система сохранений сочетает локальный `PlayerPrefs` и облачные сейвы Yandex. `SaveService` выбирает наиболее новое сохранение по timestamp и ограничивает частоту облачных записей. Любые изменения в прогрессе, очках, жизнях или скорости нужно проверять и на runtime sync, и на сериализацию сейва.

### Платформенный Слой

Интеграция с Yandex инкапсулирована в:

- `YandexPlatform`
- `YandexSDKBridge`
- `YandexService`
- `YandexBootstrap`

Платформенно-специфичный код лучше держать здесь. Не стоит тянуть прямые JS bridge вызовы в gameplay-классы.

## Стиль Кода В Репозитории

- Большинство gameplay-классов находятся в global namespace.
- Для обычных C#-сервисов и контроллеров используется constructor injection.
- Для зависимостей `MonoBehaviour` используются методы с `[Inject]`.
- Для prefab/scene-ссылок предпочитаются serialized fields.
- Настройки игры приходят из ScriptableObject-конфигов, а не из захардкоженных констант.
- Код в проекте в целом опирается на короткие прагматичные классы без глубокой иерархии наследования.

Если задача явно не про рефакторинг, лучше придерживаться текущего локального стиля.

## Правила Работы Для Агентов

- Не редактировать сгенерированные Unity-папки, если задача этого явно не требует:
  - `Library`
  - `Temp`
  - `Logs`
  - `obj`
- По возможности не править вручную YAML в `.unity`, `.prefab` и `.asset`, если есть более безопасный способ.
- Предпочтительно менять исходники в `Assets/SoaringHorse/Scripts` и код биндингов/конфигов в `Assets/SoaringHorse/Scripts/Installers`.
- Если добавляется новый настраиваемый gameplay-параметр, лучше вынести его в существующий ScriptableObject-конфиг или создать новый, а не хардкодить.
- Если нужна новая runtime-зависимость, добавьте Zenject-binding и проверьте, где ему место: project-wide, только в bootstrap или только в gameplay scene.
- Платформенно-специфичное поведение по возможности нужно держать за интерфейсами или платформенными сервисами.
- Осторожно работать с `Time.timeScale` и логикой паузы. В проекте уже есть `PauseService`, `PauseState`, ad callbacks и Yandex pause/resume events.
- Проверять корректную очистку event subscriptions в `OnDisable`, `Dispose` или аналогичных lifecycle-методах.

## Зоны Повышенного Риска

- поведение save/load и cloud sync
- переходы между сценами и порядок bootstrap
- pause/resume вокруг рекламы или callback-ов Yandex SDK
- DI-байндинги, которые создают prefab-экземпляры с `NonLazy`
- UI и environment prefabs, создаваемые инсталлерами во время выполнения

Даже небольшие на вид изменения здесь могут сломать запуск или привести к тихим проблемам в WebGL-сборке.

## Чек-Лист Проверки

Явных пользовательских автотестов для основного игрового кода здесь почти нет, поэтому ручная проверка особенно важна.

После заметных изменений в gameplay стоит проверить:

1. Проект по-прежнему входит в `Bootstrap` без DI-ошибок.
2. Стартовое меню появляется, и кнопки работают.
3. Новый запуск игры корректно доходит до `GameScene`.
4. Ввод для героя по-прежнему запускает движение, а повторный ввод применяет импульс.
5. UI очков, жизней и скорости обновляется во время забега.
6. Пауза и возврат из паузы работают корректно.
7. Save/load восстанавливает прогресс забега.
8. Код, завязанный на Yandex, остаётся безопасным, когда SDK или player недоступны.

## Наблюдения По Конкретному Проекту

- `InputlayInstaller`, похоже, содержит опечатку в названии; совместимость лучше сохранять, если задача не включает cleanup.
- `CoroutineRunner` сейчас представляет собой пустой `MonoBehaviour`, используемый как внедряемый runner-объект.
- `README.md` фактически пустой, поэтому этот файл сейчас является основным ориентиром для coding agents.

## Хорошие Значения По Умолчанию Для Будущих Изменений

- Для новой gameplay-логики:
  - размещать поведение в `Gameplay` или `Core/Systems`
  - выносить настраиваемые параметры в `Config`
  - проводить зависимости через Zenject-installers
- Для нового UI-flow:
  - оставлять view-логику в UI-классах на `MonoBehaviour`
  - orchestration выносить в controller/service-классы там, где это уместно
- Для изменений в persistence:
  - одновременно обновлять runtime progress model, save DTO и путь применения сейва
- Для платформенных возможностей:
  - изолировать их в `Scripts/Platform/Yandex`

## Если Неясно, Куда Смотреть

Отслеживайте цепочку зависимостей в таком порядке:

1. Scene
2. SceneContext / ProjectContext
3. Installer
4. Service или controller
5. Gameplay component

В этом проекте большая часть багов формата "ничего не происходит" обычно связана не со сложными алгоритмами, а с отсутствующим Zenject-binding, prefab-instantiated dependency или неправильным startup order.
