# Order Tracking Application — Frontend

React-приложение для отображения и отслеживания статусов заказов. Backend API: [c#/EGAR/OrderTrackingApplication](https://github.com/PredictorLQ/TestTasks/tree/master/c%23/EGAR/OrderTrackingApplication)

> Репозиторий реализует **frontend-часть Задания 1** из технического задания «Fullstack-разработчик» (Order Tracking Application).  
> Общее ТЗ и навигация: [c#/EGAR/README.md](https://github.com/PredictorLQ/TestTasks/blob/master/c%23/EGAR/README.md)

---

## Требования к задаче (из ТЗ)

### Функциональные требования — Frontend

| Требование | Статус |
|------------|--------|
| Список заказов | ✅ |
| Форма создания нового заказа | ✅ |
| Страница деталей заказа | ✅ |
| Автообновление статуса через WebSocket/SSE | ✅ SSE |
| Список отслеживаемых заказов (опционально) | ✅ Zustand + persist |

### Технический стек (из ТЗ)

| Компонент | Реализация |
|-----------|------------|
| React + TypeScript | ✅ |
| WebSocket / SSE | ✅ SSE (Server-Sent Events) |
| Redux / Zustand | ✅ Zustand |
| UI | ✅ Bootstrap 5 + React Bootstrap |

### Общие требования (из ТЗ)

| Требование | Статус |
|------------|--------|
| Чистый, читаемый код, SOLID, разделение на компоненты | ✅ |
| README с инструкцией запуска | ✅ |
| Unit-тесты | ✅ 24 теста |

### Задание 2 (SQL, из ТЗ)

> **Задание 2** (SQL-функция по таблице `ClientPayments`): [c#/EGAR/sql/README.md](https://github.com/PredictorLQ/TestTasks/blob/master/c%23/EGAR/sql/README.md)

---

## Архитектура

```
src/
├── api/              # REST-клиент и EventSource (SSE)
├── types/            # TypeScript-модели
├── store/            # Zustand (состояние заказов, отслеживаемые заказы)
├── hooks/            # useOrderStatusStream (SSE)
├── components/       # UI-компоненты (Bootstrap)
├── pages/            # OrdersPage, OrderDetailPage
└── utils/            # Лейблы статусов, форматирование дат
```

### Страницы

| Маршрут | Описание |
|---------|----------|
| `/` | Список заказов, форма создания, панель отслеживаемых |
| `/orders/:id` | Детали заказа, смена статуса, live-обновление через SSE |

### Управление состоянием (Zustand)

- `orders` — список заказов из API
- `trackedOrderIds` — Id отслеживаемых заказов (сохраняется в `localStorage`)
- Actions: `fetchOrders`, `createOrder`, `updateOrderStatus`, `addTrackedOrder`, …

### Real-time (SSE)

При открытии страницы деталей заказа устанавливается подключение:

```
GET /api/ordernotifications/stream?orderId={id}
```

При изменении статуса backend отправляет событие — UI обновляется без перезагрузки страницы.

На главной странице SSE используется для отслеживаемых заказов (подписка без фильтра `orderId`).

---

## Запуск

### Требования

- [Node.js](https://nodejs.org/) 18+
- Запущенный **backend API** (см. README backend-проекта)

### 1. Backend

```bash
cd D:\projects\TestTasks\c#\EGAR\OrderTrackingApplication
docker compose up -d postgres kafka zookeeper
dotnet run --project src/OrderTrackingApplication.Api
```

Backend: http://localhost:5080

### 2. Frontend

```bash
cd D:\projects\TestTasks\front\EGAR\OrderTrackingApplication
npm install
npm run dev
```

Frontend: http://localhost:5173

Vite проксирует запросы `/api` и `/ws` на `http://localhost:5080` (см. `vite.config.ts`).

### Сборка production

```bash
npm run build
npm run preview
```

---

## Тесты

```bash
npm test          # однократный прогон
npm run test:watch  # watch-режим
```

Стек: **Vitest** + **React Testing Library**

| Область | Файлы |
|---------|-------|
| Утилиты | `statusLabels.test.ts` |
| Store | `orderStore.test.ts` |
| API | `ordersApi.test.ts` |
| Hooks | `useOrderStatusStream.test.ts` |
| Компоненты | `StatusBadge`, `OrderForm`, `OrderTable`, `TrackedOrdersPanel` |

**24 теста**, все проходят.

---

## Переменные окружения

| Переменная | Описание | По умолчанию |
|------------|----------|--------------|
| `VITE_API_URL` | Базовый URL API | пусто (используется Vite proxy) |

Для production без proxy задайте, например: `VITE_API_URL=http://localhost:5080`

---

## Связанный backend

- [c#/EGAR/OrderTrackingApplication/README.md](https://github.com/PredictorLQ/TestTasks/blob/master/c%23/EGAR/OrderTrackingApplication/README.md)
- [c#/EGAR/OrderTrackingApplication](https://github.com/PredictorLQ/TestTasks/tree/master/c%23/EGAR/OrderTrackingApplication)

Подробности по API, Docker, Kafka, OpenTelemetry — в README backend-проекта.
