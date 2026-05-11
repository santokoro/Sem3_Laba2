# Главный экран

> **Текущее состояние:** на данном этапе разработки выделенного «главного экрана» (dashboard) нет. Корневой URL `/` перенаправляет на страницу входа (`/accounts/login/`). После успешной авторизации пользователь также остаётся на странице входа (`LOGIN_REDIRECT_URL = 'accounts:login'`). Функционал главного экрана с панелью презентаций, конференций и профилем — **в планах** (см. `use_cases.md` и ветки `DjangoWebsite`, `editing-presentations` и др.).

---

## 1. Назначение и роль главного экрана

На текущем этапе роль «точки входа» выполняет **страница авторизации** — это первое, что видит пользователь при переходе на сайт.

**Что доступно сейчас:**

- вход в систему (логин / пароль);
- регистрация нового аккаунта;
- выход из системы;
- после входа — навигационная панель с приветствием и кнопкой «Выход»;
- управление данными — через **админ-панель Django** (`/admin/`).

**Что планируется (по `use_cases.md` и веткам):**

- создание и редактирование презентаций;
- запуск живых сессий с кодом доступа для участников;
- интерактивные виджеты (опросы, облако слов);
- просмотр списка своих презентаций и сессий.

---

## 2. Структура страницы

### 2.1. Общий каркас (`templates/base.html`)

Все страницы наследуют `base.html`. Его структура:

```
┌─────────────────────────────────────────────┐
│  <nav>                                      │
│  ┌────────────────────────────────────────┐  │
│  │  QuizSlides          Вход | Регистрация│  │
│  │          (или)  Привет, user! | Выход  │  │
│  └────────────────────────────────────────┘  │
├─────────────────────────────────────────────┤
│  <div class="container">                    │
│                                             │
│    [ flash-сообщения (если есть) ]          │
│                                             │
│    ┌──────────────────────────────────────┐ │
│    │  {% block content %}                 │ │
│    │  (содержимое конкретной страницы)    │ │
│    │  {% endblock %}                      │ │
│    └──────────────────────────────────────┘ │
│                                             │
└─────────────────────────────────────────────┘
```

### 2.2. Страница входа (`/accounts/login/`)

Наследует `base.html`, блок `content`:

```
┌──────────────────────────┐
│          Вход            │
│                          │
│  Имя пользователя: [  ] │
│  Пароль:           [  ] │
│                          │
│      [ Войти ]           │
│                          │
│  Нет аккаунта?           │
│  Зарегистрироваться →    │
└──────────────────────────┘
```

### 2.3. Страница регистрации (`/accounts/signup/`)

Наследует `base.html`, блок `content`:

```
┌──────────────────────────┐
│      Регистрация         │
│                          │
│  Имя пользователя: [  ] │
│  Пароль:           [  ] │
│  Подтверждение:    [  ] │
│                          │
│  [ Зарегистрироваться ]  │
│                          │
│  Уже есть аккаунт?      │
│  Войти →                 │
└──────────────────────────┘
```

---

## 3. Сценарии взаимодействия

### Сценарий 1. Первый визит (неавторизованный пользователь)

1. Пользователь заходит на `/`.
2. Django перенаправляет на `/accounts/login/`.
3. Навигация показывает ссылки **«Вход»** и **«Регистрация»**.
4. Пользователь нажимает **«Регистрация»** → переход на `/accounts/signup/`.
5. Заполняет форму, нажимает **«Зарегистрироваться»**.
6. Система создаёт аккаунт, показывает flash-сообщение *«Аккаунт … успешно создан!»* и перенаправляет на страницу входа.

### Сценарий 2. Вход в систему

1. Пользователь на странице `/accounts/login/` вводит логин и пароль.
2. Django проверяет данные через `AuthenticationForm`.
3. При успехе — авторизация, редирект на `LOGIN_REDIRECT_URL` (сейчас — снова `/accounts/login/`, но навигация уже показывает **«Привет, username!»** и **«Выход»**).
4. При ошибке — форма отображается повторно с сообщением об ошибке.

### Сценарий 3. Выход

1. Авторизованный пользователь нажимает **«Выход»** в навигации.
2. Django вызывает `LogoutView`, завершает сессию.
3. Перенаправление на `/accounts/login/` (`LOGOUT_REDIRECT_URL`).

### Сценарий 4. Работа с данными через админку

1. Суперпользователь переходит на `/admin/`.
2. Доступны все модели `core` (Session, Presentation, Slide и т.д.) для ручного управления.

---

## 4. Код, отвечающий за главный экран

### 4.1. Маршрутизация — `quizslides/urls.py`

```python
from django.contrib import admin
from django.urls import path, include
from django.views.generic.base import RedirectView

urlpatterns = [
    path('', RedirectView.as_view(pattern_name='accounts:login', permanent=False)),
    path('admin/', admin.site.urls),
    path('accounts/', include('accounts.urls')),
]
```

Корень `/` — редирект на именованный маршрут `accounts:login`.

### 4.2. Маршруты авторизации — `accounts/urls.py`

```python
from django.urls import path
from django.contrib.auth import views as auth_views
from . import views

app_name = 'accounts'

urlpatterns = [
    path('signup/', views.signup_view, name='signup'),
    path('login/', auth_views.LoginView.as_view(
        template_name='accounts/login.html'), name='login'),
    path('logout/', auth_views.LogoutView.as_view(), name='logout'),
]
```

### 4.3. View регистрации — `accounts/views.py`

```python
from django.shortcuts import render, redirect
from django.contrib import messages
from django.contrib.auth.forms import UserCreationForm

def signup_view(request):
    if request.method == 'POST':
        form = UserCreationForm(request.POST)
        if form.is_valid():
            user = form.save()
            username = form.cleaned_data.get('username')
            messages.success(request, f'Аккаунт {username} успешно создан!')
            return redirect('accounts:login')
    else:
        form = UserCreationForm()
    return render(request, 'accounts/signup.html', {'form': form})
```

### 4.4. Настройки редиректов — `quizslides/settings.py` (фрагмент)

```python
LOGIN_REDIRECT_URL = 'accounts:login'
LOGOUT_REDIRECT_URL = 'accounts:login'
LOGIN_URL = 'accounts:login'
```

### 4.5. Шаблоны

| Файл | Роль |
|------|------|
| `templates/base.html` | Общий каркас: `<nav>` (логотип, вход/выход), контейнер для flash-сообщений, блок `{% block content %}` |
| `templates/accounts/login.html` | Форма входа (username + password), ссылка на регистрацию |
| `templates/accounts/signup.html` | Форма регистрации (username + password × 2), ссылка на вход |

### 4.6. Приложение `core`

Файл `core/views.py` — **пустой** (заглушка Django). Ни одного view, URL или шаблона для работы с презентациями / сессиями через браузер на текущей ветке нет.
