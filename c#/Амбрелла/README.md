# Umbrella - Система отчетности на базе Stimulsoft

## Описание проекта

Umbrella - это веб-приложение для управления, генерации и планирования отчетов на основе конструктора Stimulsoft Reports, с поддержкой подключения к различным источникам данных.

## Архитектура проекта

Проект построен на основе **чистой архитектуры** (Clean Architecture) с использованием следующих подходов:

### Структура слоев

1. **Umbrella.Domain** - Доменный слой
   - Entities (сущности БД)
   - DTOs (для передачи данных)
   - Models (для операций создания/обновления)
   - Enums (перечисления)

2. **Umbrella.Application** - Слой приложения
   - Commands (Create, Update, Delete) - CQRS паттерн
   - Queries (GetAll, GetById) - CQRS паттерн
   - Contracts (интерфейсы для Repository и Service)
   - Behaviors (валидация через FluentValidation)

3. **Umbrella.Infrastructure** - Слой инфраструктуры
   - Repositories (реализация репозиториев)
   - Services (реализация сервисов)
   - Data (DbContext и QueryContext)
   - Migrations (миграции БД)
   - Mappings (Mapster конфигурации)

4. **Umbrella.Api** - API слой
   - Controllers (REST API endpoints)
   - Bindings (модели для API)
   - Extensions (Swagger, DI)

### Используемые технологии и подходы

- **CQRS** - разделение команд и запросов через MediatR
- **MediatR** - медиатор для обработки команд и запросов
- **Mapster** - маппинг между объектами
- **FluentValidation** - валидация данных
- **Entity Framework Core** - ORM для работы с БД
- **PostgreSQL** - база данных
- **Dapper** - для прямых SQL запросов (при необходимости)
- **Keycloak** - аутентификация через OIDC
- **Stimulsoft Reports** - генерация отчетов
- **Swagger** - документация API

## Требования

- .NET 9.0 SDK
- Docker и Docker Compose
- PostgreSQL 16+
- Keycloak (через Docker)

## Запуск проекта

### 1. Запуск через Docker Compose

```bash
docker-compose up -d
```

Это запустит:
- PostgreSQL на порту 5432
- Keycloak на порту 8080
- API на порту 5000

### 2. Запуск вручную

1. Установите PostgreSQL и создайте базу данных `umbrella`
2. Обновите строку подключения в `appsettings.json`
3. Запустите миграции:
```bash
dotnet ef database update --project Umbrella.Infrastructure --startup-project Umbrella.Api
```
4. Запустите API:
```bash
dotnet run --project Umbrella.Api
```

## API Endpoints

После запуска приложения Swagger UI доступен по адресу: `http://localhost:5000`

### Основные endpoints:

- `GET /api/DataSources` - получить все источники данных
- `POST /api/DataSources` - создать источник данных
- `GET /api/Reports` - получить все отчеты
- `POST /api/Reports` - создать отчет
- `GET /api/ScheduledReports` - получить все запланированные отчеты
- `POST /api/ScheduledReports` - создать запланированный отчет

## Структура базы данных

### Таблицы:

- **Users** - пользователи системы
- **DataSources** - источники данных
- **Reports** - отчеты Stimulsoft
- **ScheduledReports** - запланированные отчеты

## Особенности реализации

- **Мягкое удаление** - все сущности используют soft delete (IsDeleted)
- **Версионирование** - автоматическое отслеживание версий сущностей
- **Валидация** - все команды валидируются через FluentValidation
- **Логирование** - подробное логирование всех операций
- **XML комментарии** - полная документация для Swagger

## Фронтенд

Фронтенд находится в директории `front/` и использует:
- SCSS с БЭМ методологией
- Tailwind CSS
- Резиновая верстка с Grid и Flex
- Единицы измерения: rem (без px)

## Лицензия

Проект создан в рамках тестового задания.

