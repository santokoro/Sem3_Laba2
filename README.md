# Архитектура проекта QuizSlides

Документ для разработчиков. Актуализируйте дату и ветку/коммит при существенных изменениях кода.

## 1. Назначение

**QuizSlides** — веб-проект на Django для интерактивных презентаций: живые сессии, участники, слайды, виджеты (опросы, облако слов и т.д.). Сценарии продукта описаны в корневом файле [`use_cases.md`](../use_cases.md).

## 2. Технологии и платформа

| Компонент | Описание |
|-----------|----------|
| Язык | Python |
| Фреймворк | Django (см. `requirements.txt` / `README.md`) |
| БД | PostgreSQL, настройки в `quizslides/settings.py` |
| Шаблоны | Django Templates, каталог `templates/` |
| Статика | `STATIC_URL`, стандартная схема Django (`collectstatic` для продакшена) |
| Медиа | Загрузки пользователей (например `ImageField` у слайдов): при продакшене задаются `MEDIA_URL`, `MEDIA_ROOT` и раздача файлов |

## 3. Структура репозитория

| Путь | Назначение |
|------|------------|
| `manage.py` | Точка входа CLI Django |
| `quizslides/` | Пакет настроек проекта: `settings.py`, `urls.py`, `wsgi.py`, `asgi.py` |
| `accounts/` | Регистрация, вход, выход (`urls`, `views`, модели при необходимости) |
| `core/` | Предметная область: модели, миграции, админка, тесты |
| `templates/` | Общие шаблоны, в т.ч. `base.html`, `accounts/login.html`, `accounts/signup.html` |
| `web/` | Отдельные статические страницы (например `guest.html`), при необходимости вне основного URLconf |
| `requirements.txt` | Зависимости Python |
| `README.md` | Развёртывание и запуск |

## 4. Конфигурация проекта (`quizslides/settings.py`)

Основные блоки: `SECRET_KEY`, `DEBUG`, `ALLOWED_HOSTS`, `INSTALLED_APPS`, `MIDDLEWARE`, `ROOT_URLCONF`, `TEMPLATES`, `WSGI_APPLICATION`, `DATABASES`, валидаторы паролей, интернационализация, `STATIC_URL`, `DEFAULT_AUTO_FIELD`.

Параметры входа:

- `LOGIN_URL` — страница входа (`accounts:login`)
- `LOGIN_REDIRECT_URL` / `LOGOUT_REDIRECT_URL` — куда вести после входа и выхода

Файлы `wsgi.py` и `asgi.py` — точки входа для боевого и асинхронного сервера; `asgi` пригодится при подключении WebSocket (например Django Channels), если это появится в проекте.

## 5. Middleware

Порядок обработки запроса (типовой для Django):

1. `SecurityMiddleware`
2. `SessionMiddleware`
3. `CommonMiddleware`
4. `CsrfViewMiddleware`
5. `AuthenticationMiddleware`
6. `MessageMiddleware`
7. `XFrameOptionsMiddleware`

В результате на запросах доступны сессия, защита CSRF, текущий пользователь (если аутентифицирован) и flash-сообщения.

## 6. Встроенные приложения Django (`contrib`)

| Приложение | Роль в архитектуре |
|------------|-------------------|
| `django.contrib.admin` | Панель `/admin/` |
| `django.contrib.auth` | Пользователи, группы, права; связи `ForeignKey` к `User` в `core` |
| `django.contrib.contenttypes` | Content types; основа части функционала админки |
| `django.contrib.sessions` | Сессия браузера |
| `django.contrib.messages` | Сообщения пользователю после редиректа |
| `django.contrib.staticfiles` | Статика в разработке и при сборке |

## 7. Собственные приложения

### `accounts`

- Маршруты: `signup/`, `login/`, `logout/` (`accounts/urls.py`, `app_name = 'accounts'`).
- Вход/выход: `LoginView` / `LogoutView` из `django.contrib.auth.views` и собственный `signup_view`.

### `core`

- Доменные модели: сессии, участники, реакции, презентации, слайды, виджеты, облака слов, квизы, варианты ответов, голосования — см. `core/models.py` и миграции в `core/migrations/`.

## 8. Маршрутизация (`quizslides/urls.py`)

| URL | Назначение |
|-----|------------|
| `/` | Редирект на `accounts:login` |
| `/admin/` | Админ-панель Django |
| `/accounts/` | Все маршруты приложения `accounts` |

Публичные URL для презентаций, гостевых экранов и API добавляются по мере разработки (через `path` / `include` в этом же файле или в отдельных `urls` приложений).

## 9. Админка Django

- **URL:** `/admin/` (при включённом `django.contrib.admin` и созданном суперпользователе: `python manage.py createsuperuser`).

### Регистрация моделей

В **`core/admin.py`** через `admin.site.register` зарегистрированы:

`Session`, `Member`, `Reaction`, `Presentation`, `Slide`, `Widget`, `Quiz`, `Vote`, `AnswerOption`, `WordCloud`.

Пользователи с правами staff/superuser могут вести эти сущности без отдельного внутреннего UI.

### Пользователи и группы

В **`accounts/admin.py`** отдельные модели приложения не регистрируются (файл по сути только подключает `admin`). Управление учётными записями — через стандартный раздел **Users** и **Groups** в админке (`django.contrib.auth`).

### Расширение

При необходимости: кастомные `ModelAdmin`, `list_display`, `search_fields`, `inlines` — в `admin.py` соответствующих приложений.

## 10. Аутентификация и авторизация

- Вход, выход, регистрация — приложение `accounts`, шаблоны в `templates/accounts/`.
- Ограничение доступа к view — по соглашению команды: `login_required`, проверки прав, группы из `auth`.
- Формы защищены CSRF (`CsrfViewMiddleware` + теги в шаблонах).

## 11. Доменная модель (кратко)

Сущности и связи описаны в **`core/models.py`**. Основные понятия:

- **Session** — сессия презентации (код, лимит участников, время, статус, владелец).
- **Member** — участник сессии.
- **Presentation**, **Slide** — презентация и слайды.
- **Widget** — виджет на слайде; к нему привязаны специализированные модели.
- **WordCloud**, **Quiz**, **AnswerOption**, **Vote** — облако слов, опрос, варианты, голоса.
- **Reaction** — реакции в сессии.

Точные поля и ограничения — в коде и миграциях.

## 12. Удалённые репозитории

Типичная схема у команды:

- **origin** — основной репозиторий (например `github.com/…/quizslides`).
- **myfork** — личный форк для pull request.

Точные URL смотрите в выводе `git remote -v`.

## 13. Ветки Git

Список веток на GitHub и в локальном клоне может различаться до обновления ссылок на коммиты.

Актуальный перечень удалённых веток:

```bash
git fetch --all --prune
git branch -r
```

**Примеры имён**, которые встречались на GitHub: `main` (по умолчанию), `DjangoWebsite`, `buttonExitFix`, `editing-presentations`, `feature/add-photos-to-main-page`, `feature/admin-assignment`, `feature/env`, `registration`.

**Дополнительно** в клонах у `origin` могут быть, например: `feature/authorization`, `feature/models&migration`, `feature/topic-4-use-cases`, `fix-readme`; у форка — свои ветки (например `feature/postgres-setup`).

Для документации удобно вести таблицу: ветка → краткое назначение по договорённости команды → последнее сообщение коммита (`git log -1 --oneline origin/<ветка>`).

## 14. Быстрый старт с админкой

1. Применить миграции: `python manage.py migrate`
2. Создать суперпользователя: `python manage.py createsuperuser`
3. Запустить сервер: `python manage.py runserver`
4. Открыть `http://127.0.0.1:8000/admin/` — разделы **Users** (auth) и модели из **Core**

## 15. Актуальность документа

Указывайте при сдаче отчётов или ревью: **ветка** и **хеш коммита**, для которых проверена архитектура.
