namespace Umbrella.Infrastructure.Extensions;

public static class EnumExtensions
{
    public static T ParseEnum<T>(this string value) where T : struct, Enum
    {
        if (Enum.TryParse<T>(value, out var result))
            return result;
        throw new ArgumentException($"Не удалось преобразовать '{value}' в {typeof(T).Name}");
    }
}