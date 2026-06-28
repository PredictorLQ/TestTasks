/*
    Задание 2 — схема и тестовые данные (Microsoft SQL Server).
    Таблица client.Payments из ТЗ.
*/

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'client')
    EXEC(N'CREATE SCHEMA client');
GO

IF OBJECT_ID(N'client.Payments', N'U') IS NOT NULL
    DROP TABLE client.Payments;
GO

CREATE TABLE client.Payments
(
    Id       BIGINT         NOT NULL,
    ClientId BIGINT         NOT NULL,
    Dt       DATETIME2(0)   NOT NULL,
    Amount   MONEY          NOT NULL,
    CONSTRAINT PK_client_Payments PRIMARY KEY (Id)
);
GO

INSERT INTO client.Payments (Id, ClientId, Dt, Amount)
VALUES
    (1, 1, '2022-01-03 17:24:00', 100),
    (2, 1, '2022-01-05 17:24:14', 200),
    (3, 1, '2022-01-05 18:23:34', 250),
    (4, 1, '2022-01-07 10:12:38',  50),
    (5, 2, '2022-01-05 17:24:14', 278),
    (6, 2, '2022-01-10 12:39:29', 300);
GO
