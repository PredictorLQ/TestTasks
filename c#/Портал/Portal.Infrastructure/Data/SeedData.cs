using Microsoft.EntityFrameworkCore;
using Portal.Domain.Entities;

namespace Portal.Infrastructure.Data;

public static class SeedData
{
    public static async System.Threading.Tasks.Task SeedAsync(PortalDbContext context)
    {
        if (await context.TaskTypes.AnyAsync())
            return;

        var taskTypes = new List<TaskType>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Разработка",
                Description = "Задачи, связанные с разработкой программного обеспечения",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Тестирование",
                Description = "Задачи, связанные с тестированием функциональности",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Документация",
                Description = "Задачи по написанию и обновлению документации",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Исправление ошибок",
                Description = "Задачи по исправлению найденных ошибок",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Рефакторинг",
                Description = "Задачи по улучшению структуры кода",
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.TaskTypes.AddRangeAsync(taskTypes);
        await context.SaveChangesAsync();
    }
}