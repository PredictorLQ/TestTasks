# Order Tracking Application — Backend

Веб-API для создания, получения и отслеживания заказов с асинхронной публикацией событий в Kafka и real-time уведомлениями через SSE/WebSocket.

> Репозиторий реализует **Задание 1** из технического задания «Fullstack-разработчик» (Order Tracking Application).  
> Общее ТЗ и навигация: [c#/EGAR/README.md](https://github.com/PredictorLQ/TestTasks/blob/master/c%23/EGAR/README.md)

---

## Требования к задаче (из ТЗ)

### Функциональные требования — Backend

| Требование | Статус |
|------------|--------|
| Создание и получение заказов | ✅ |
| Поля: `OrderNumber`, `Description`, `Status`, `CreatedAt`, `UpdatedAt` | ✅ |
| Статусы: создан, отправлен, доставлен, отменён | ✅ |
| Асинхронная отправка событий в Kafka при изменении статуса | ✅ |
| Подписка на изменения статуса (WebSocket / SSE) | ✅ |

### Технический стек (из ТЗ)

| Компонент | Реализация |
|-----------|------------|
| .NET 8, ASP.NET Core Web API | ✅ |
| EF Core | ✅ |
| PostgreSQL | ✅ |
| Kafka (альтернатива RabbitMQ из ТЗ) | ✅ |
| Логирование (консоль + файл) | ✅ Serilog |

### Дополнительные требования (из ТЗ)

| Требование | Статус |
|------------|--------|
| Чистый, читаемый код, SOLID, разделение на компоненты и слои | ✅ |
| Docker + docker-compose | ✅ |
| Unit-тесты | ✅ 16 тестов |
| XML-документация (Swagger summary) | ✅ |
| OpenTelemetry | ✅ |

### Задание 2 (SQL, из ТЗ)

> **Задание 2** — табличная SQL-функция расчёта суммы платежей клиента по дням (`ClientPayments`, `ClientId`, интервал дат).  
> Реализовано отдельно: [c#/EGAR/sql/README.md](https://github.com/PredictorLQ/TestTasks/blob/master/c%23/EGAR/sql/README.md)

---

## Архитектура

Проект построен по принципам **Clean Architecture** с **CQRS** и **Repository pattern**.

```
OrderTrackingApplication/
├── src/
│   ├── OrderTrackingApplication.Domain/        # Сущности, enum, DTO, события
│   ├── OrderTrackingApplication.Application/   # CQRS (MediatR), контракты, валидация
│   ├── OrderTrackingApplication.Infrastructure/# EF Core, Kafka, Outbox, репозитории
│   └── OrderTrackingApplication.Api/           # Web API, Swagger, SSE, WebSocket
└── tests/
    ├── OrderTrackingApplication.UnitTests/
    └── OrderTrackingApplication.IntegrationTests/
```

### Слои

| Слой | Ответственность |
|------|-----------------|
| **Domain** | `Order`, `OutboxMessage`, `OrderStatus`, доменные события |
| **Application** | Команды/запросы (MediatR), интерфейсы репозиториев, FluentValidation |
| **Infrastructure** | PostgreSQL (EF Core), Kafka producer, Outbox processor, Polly circuit breaker |
| **Api** | HTTP-контроллеры, middleware, OpenTelemetry, Serilog |

### Паттерны

- **CQRS** — команды (`CreateOrder`, `UpdateOrderStatus`) и запросы (`GetAllOrders`, `GetOrderById`, …) через MediatR
- **Repository** — `IOrderRepository`, `IOutboxRepository`
- **Transactional Outbox** — событие сохраняется в БД в одной транзакции с заказом, затем асинхронно публикуется в Kafka
- **Circuit Breaker** — Polly при сбоях Kafka
- **Идемпотентность** — уникальный `IdempotencyKey` в outbox + `EnableIdempotence` в Kafka producer

### Поток изменения статуса

```
PATCH /api/orders/{id}/status
        │
        ▼
UpdateOrderStatusCommand
        │
        ├──► PostgreSQL: UPDATE order + INSERT outbox (одна транзакция)
        │
        ├──► OrderNotificationService → SSE / WebSocket (клиенты)
        │
        └──► OutboxProcessor (фон) → Kafka (topic: order-status-changed)
```

---

## API

| Метод | URL | Описание |
|-------|-----|----------|
| `POST` | `/api/orders` | Создать заказ |
| `GET` | `/api/orders` | Список заказов |
| `GET` | `/api/orders/{id}` | Заказ по Id |
| `GET` | `/api/orders/by-number/{orderNumber}` | Заказ по номеру |
| `PATCH` | `/api/orders/{id}/status` | Изменить статус |
| `GET` | `/api/ordernotifications/stream?orderId=` | SSE-подписка |
| `WS` | `/ws/orders?orderId=` | WebSocket-подписка |
| `GET` | `/health` | Health check |
| `GET` | `/swagger` | Swagger UI (Development) |

### Пример создания заказа

```json
POST /api/orders
{
  "orderNumber": "ORD-001",
  "description": "Тестовый заказ"
}
```

### Пример смены статуса

```json
PATCH /api/orders/{id}/status
{
  "status": "Sent"
}
```

Допустимые значения `status`: `Created`, `Sent`, `Delivered`, `Cancelled`.

---

## Запуск

### Требования

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (для PostgreSQL и Kafka)

### Вариант 1: Docker Compose (рекомендуется)

```bash
cd D:\projects\TestTasks\c#\EGAR\OrderTrackingApplication

# Запуск всей инфраструктуры + API
docker compose up -d --build

# Только PostgreSQL и Kafka (для локального dotnet run)
docker compose up -d postgres kafka zookeeper jaeger
```

| Сервис | URL / порт |
|--------|------------|
| API | http://localhost:5080 |
| Swagger | http://localhost:5080/swagger |
| PostgreSQL | `localhost:5433` |
| Kafka | `localhost:9092` |
| Jaeger UI | http://localhost:16686 |

Миграции EF Core применяются **автоматически** при старте приложения (`MigrateDatabaseAsync`).

### Вариант 2: Локальный запуск

```bash
# 1. Поднять инфраструктуру
docker compose up -d postgres kafka zookeeper

# 2. Запустить API
dotnet run --project src/OrderTrackingApplication.Api
```

API по умолчанию: http://localhost:5080

Строка подключения (Development): `Host=localhost;Port=5433;Database=order_tracking;Username=postgres;Password=postgres`

### Конфигурация

Основные настройки в `src/OrderTrackingApplication.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=order_tracking;..."
  },
  "Kafka": {
    "BootstrapServers": "localhost:9092",
    "OrderStatusTopic": "order-status-changed"
  },
  "OpenTelemetry": {
    "Enabled": true,
    "OtlpEndpoint": "http://localhost:4317"
  }
}
```

Логи Serilog: консоль + `logs/order-tracking-.log`

---

## Тесты

```bash
dotnet test
```

| Проект | Тестов |
|--------|--------|
| UnitTests | 16 |
| IntegrationTests | 7 |
| **Итого** | **23** |

---

## Связанный frontend

- [front/EGAR/OrderTrackingApplication/README.md](https://github.com/PredictorLQ/TestTasks/blob/master/front/EGAR/OrderTrackingApplication/README.md)
- [front/EGAR/OrderTrackingApplication](https://github.com/PredictorLQ/TestTasks/tree/master/front/EGAR/OrderTrackingApplication)

Запуск frontend: `npm run dev` → http://localhost:5173 (проксирует `/api` на backend).
