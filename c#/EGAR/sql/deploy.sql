/*
    Полный деплой Задания 2: схема, данные, функция, проверка.
    Запуск: sqlcmd -S localhost -d master -E -i deploy.sql
    или выполнить в SSMS / Azure Data Studio по порядку.
*/

:r 01_schema_and_seed.sql
:r 02_fn_GetClientPaymentsByDay.sql
:r 03_verify_examples.sql
