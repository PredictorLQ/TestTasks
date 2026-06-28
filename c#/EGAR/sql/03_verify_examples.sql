/*
    Проверка примеров из ТЗ.
    Ожидаемые результаты указаны в README.md.
*/

-- Пример 1: ClientId = 1, Sd = 2022-01-02, Ed = 2022-01-07
SELECT Dt, [Sum]
FROM client.fn_GetClientPaymentsByDay(1, '2022-01-02', '2022-01-07')
ORDER BY Dt;

-- Пример 2: ClientId = 2, Sd = 2022-01-04, Ed = 2022-01-11
SELECT Dt, [Sum]
FROM client.fn_GetClientPaymentsByDay(2, '2022-01-04', '2022-01-11')
ORDER BY Dt;
