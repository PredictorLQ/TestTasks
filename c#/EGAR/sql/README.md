# Задание 2 — SQL (из ТЗ Fullstack-разработчик)

> Решение **Задания 2** из документа «Техническое задание для fullstack-разработчика».  
> Общее ТЗ и навигация: [c#/EGAR/README.md](https://github.com/PredictorLQ/TestTasks/blob/master/c%23/EGAR/README.md)

**СУБД:** Microsoft SQL Server (T-SQL). Типы `datetime2(0)`, `money`, табличная inline-функция.

---

## Требования из ТЗ

### Задача

Написать **табличную функцию SQL**, которая по `ClientId` и интервалу дат (тип `Date`) возвращает **суммарные суммы платежей по каждому дню**.

- Если в указанный день платежей не было — функция возвращает **0**.
- В один день может быть несколько платежей — их суммы **складываются**.
- Интервал дат может охватывать **несколько лет** (все дни от `@Sd` до `@Ed` включительно должны присутствовать в результате).

### Структура таблицы

Таблица `ClientPayments` (в скрипте — схема `client`, имя `Payments`, как в примере ТЗ):

| Поле     | Тип           | Описание              |
|----------|---------------|-----------------------|
| Id       | bigint        | Первичный ключ        |
| ClientId | bigint        | Id клиента            |
| Dt       | datetime2(0)  | Дата и время платежа  |
| Amount   | money         | Сумма платежа         |

### Тестовые данные (из ТЗ)

| Id | ClientId | Dt                  | Amount |
|----|----------|---------------------|--------|
| 1  | 1        | 2022-01-03 17:24:00 | 100    |
| 2  | 1        | 2022-01-05 17:24:14 | 200    |
| 3  | 1        | 2022-01-05 18:23:34 | 250    |
| 4  | 1        | 2022-01-07 10:12:38 | 50     |
| 5  | 2        | 2022-01-05 17:24:14 | 278    |
| 6  | 2        | 2022-01-10 12:39:29 | 300    |

### Примеры ожидаемого результата

**Пример 1**

- Параметры: `ClientId = 1`, `Sd = 2022-01-02`, `Ed = 2022-01-07`

| Dt         | Sum |
|------------|-----|
| 2022-01-02 | 0   |
| 2022-01-03 | 100 |
| 2022-01-04 | 0   |
| 2022-01-05 | 450 |
| 2022-01-06 | 0   |
| 2022-01-07 | 50  |

**Пример 2**

- Параметры: `ClientId = 2`, `Sd = 2022-01-04`, `Ed = 2022-01-11`

| Dt         | Sum |
|------------|-----|
| 2022-01-04 | 0   |
| 2022-01-05 | 278 |
| 2022-01-06 | 0   |
| 2022-01-07 | 0   |
| 2022-01-08 | 0   |
| 2022-01-09 | 0   |
| 2022-01-10 | 300 |
| 2022-01-11 | 0   |

---

## Реализация

### Файлы

| Файл | Назначение |
|------|------------|
| `01_schema_and_seed.sql` | Схема `client`, таблица `Payments`, тестовые данные |
| `02_fn_GetClientPaymentsByDay.sql` | Табличная функция `client.fn_GetClientPaymentsByDay` |
| `03_verify_examples.sql` | Запросы проверки примеров из ТЗ |
| `deploy.sql` | Последовательный запуск всех скриптов (sqlcmd `:r`) |

### Функция

```sql
client.fn_GetClientPaymentsByDay(@ClientId BIGINT, @Sd DATE, @Ed DATE)
```

**Возвращает:** `Dt` (date), `Sum` (money).

**Логика:**

1. CTE `N` генерирует все даты от `@Sd` до `@Ed` без рекурсии (поддерживает длинные интервалы).
2. Платежи клиента агрегируются по календарному дню (`CAST(Dt AS DATE)`).
3. К каждой дате интервала выполняется `LEFT JOIN`; отсутствующие дни получают `0`.

---

## Запуск

### SQL Server Management Studio / Azure Data Studio

Выполните скрипты **по порядку**:

1. `01_schema_and_seed.sql`
2. `02_fn_GetClientPaymentsByDay.sql`
3. `03_verify_examples.sql`

### sqlcmd

```powershell
cd D:\projects\TestTasks\c#\EGAR\sql
sqlcmd -S localhost -E -i deploy.sql
```

Для именованного экземпляра: `-S localhost\SQLEXPRESS`.

### Docker (SQL Server)

```powershell
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Your_strong_Password123" `
  -p 1433:1433 --name mssql-tz -d mcr.microsoft.com/mssql/server:2022-latest

sqlcmd -S localhost,1433 -U sa -P "Your_strong_Password123" -i 01_schema_and_seed.sql
sqlcmd -S localhost,1433 -U sa -P "Your_strong_Password123" -i 02_fn_GetClientPaymentsByDay.sql
sqlcmd -S localhost,1433 -U sa -P "Your_strong_Password123" -i 03_verify_examples.sql
```

### Пример вызова

```sql
SELECT Dt, [Sum]
FROM client.fn_GetClientPaymentsByDay(1, '2022-01-02', '2022-01-07')
ORDER BY Dt;
```

---

## Связь с другими частями ТЗ

- **Общее ТЗ:** [c#/EGAR/README.md](https://github.com/PredictorLQ/TestTasks/blob/master/c%23/EGAR/README.md)
- **Задание 1 (backend):** [OrderTrackingApplication/README.md](https://github.com/PredictorLQ/TestTasks/blob/master/c%23/EGAR/OrderTrackingApplication/README.md) · [c#/EGAR/OrderTrackingApplication](https://github.com/PredictorLQ/TestTasks/tree/master/c%23/EGAR/OrderTrackingApplication)
- **Frontend:** [front/EGAR/OrderTrackingApplication/README.md](https://github.com/PredictorLQ/TestTasks/blob/master/front/EGAR/OrderTrackingApplication/README.md) · [front/EGAR/OrderTrackingApplication](https://github.com/PredictorLQ/TestTasks/tree/master/front/EGAR/OrderTrackingApplication)

Задание 2 — отдельное SQL-упражнение и не интегрировано в Order Tracking backend (там используется PostgreSQL, а не SQL Server).
