/*
    Задание 2 — табличная функция SQL.

    Возвращает по ClientId и интервалу дат [@Sd, @Ed] сумму платежей
    за каждый календарный день. Дни без платежей — 0.
*/

IF OBJECT_ID(N'client.fn_GetClientPaymentsByDay', N'IF') IS NOT NULL
    DROP FUNCTION client.fn_GetClientPaymentsByDay;
GO

CREATE FUNCTION client.fn_GetClientPaymentsByDay
(
    @ClientId BIGINT,
    @Sd       DATE,
    @Ed       DATE
)
RETURNS TABLE
AS
RETURN
(
    WITH N AS
    (
        SELECT TOP (DATEDIFF(DAY, @Sd, @Ed) + 1)
               ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1 AS DayOffset
        FROM sys.all_objects AS a
        CROSS JOIN sys.all_objects AS b
    ),
    DateRange AS
    (
        SELECT DATEADD(DAY, n.DayOffset, @Sd) AS Dt
        FROM N AS n
    ),
    DailyPayments AS
    (
        SELECT
            CAST(p.Dt AS DATE) AS PaymentDate,
            SUM(p.Amount)      AS Amount
        FROM client.Payments AS p
        WHERE p.ClientId = @ClientId
          AND CAST(p.Dt AS DATE) BETWEEN @Sd AND @Ed
        GROUP BY CAST(p.Dt AS DATE)
    )
    SELECT
        d.Dt,
        ISNULL(dp.Amount, CAST(0 AS MONEY)) AS [Sum]
    FROM DateRange AS d
    LEFT JOIN DailyPayments AS dp
        ON d.Dt = dp.PaymentDate
);
GO
