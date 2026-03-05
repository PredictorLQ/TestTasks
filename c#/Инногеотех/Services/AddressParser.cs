using task.Entities;

namespace task.Services;

/// <summary>
/// Утилита для разбора строкового адреса в поля сущности <see cref="Office"/>.
/// </summary>
public static class AddressParser
{
    /// <summary>
    /// Заполняет адресные поля офиса на основе строкового адреса.
    /// </summary>
    public static void ParseAddress(string? address, Office office)
    {
        if (string.IsNullOrWhiteSpace(address))
            return;

        var parts = address
         .Split(',', StringSplitOptions.RemoveEmptyEntries)
         .Select(p => p.Trim())
         .Where(p => !string.IsNullOrWhiteSpace(p))
         .ToList();

        if (parts.Count == 0)
            return;

        if (parts.Count > 0 && IsPostalCode(parts[0]))
        {
            parts.RemoveAt(0);
        }

        if (parts.Count == 0)
            return;

        string? region = null;
        string? city = null;
        string? street = null;
        string? houseNumber = null;

        foreach (var part in parts)
        {
            if (region == null && IsRegionPart(part))
            {
                region = part;
            }
        }

        for (int i = parts.Count - 1; i >= 0; i--)
        {
            if (IsCityPart(parts[i]))
            {
                city = parts[i];
                break;
            }
        }

        var streetIndex = -1;
        for (int i = 0; i < parts.Count; i++)
        {
            if (IsStreetPart(parts[i]))
            {
                street = parts[i];
                streetIndex = i;
                break;
            }
        }

        if (streetIndex >= 0 && streetIndex + 1 < parts.Count)
        {
            houseNumber = ExtractHouseNumber(parts[streetIndex + 1]);
        }

        if (string.IsNullOrWhiteSpace(houseNumber))
        {
            foreach (var part in parts)
            {
                houseNumber = ExtractHouseNumber(part);
                if (!string.IsNullOrWhiteSpace(houseNumber))
                    break;
            }
        }

        office.AddressRegion = region;
        office.AddressCity = city;
        office.AddressStreet = street;
        office.AddressHouseNumber = houseNumber;
    }

    private static bool IsPostalCode(string part)
    {
        var digitsOnly = new string(part.Where(char.IsDigit).ToArray());
        return !string.IsNullOrEmpty(digitsOnly) && digitsOnly.Length >= 5;
    }

    private static bool IsRegionPart(string part)
    {
        var lower = part.ToLowerInvariant();

        string[] regionMarkers = [" обл", "область", " край", "край", " республика", " респ", " ао"];

        return regionMarkers.Any(marker => lower.Contains(marker));
    }

    private static bool IsCityPart(string part)
    {
        var lower = part.ToLowerInvariant();

        string[] cityContainsMarkers = [" г,", " г.", " п.", "поселок", "пос.", "пгт", " с.", "деревня", "д."];
        string[] cityEndsWithMarkers = [" г", " п", " с"];

        return cityContainsMarkers.Any(marker => lower.Contains(marker)) ||
               cityEndsWithMarkers.Any(marker => lower.EndsWith(marker));
    }

    private static bool IsStreetPart(string part)
    {
        var lower = part.ToLowerInvariant();

        // Подстроки, по которым определяем, что часть относится к улице
        string[] streetMarkers = [" ул", "улица", " пр-кт", "проспект", " просп.", " пер", "переулок",
            " проезд", " ш", "шоссе", " б-р", "бульвар", " дорога", "тракт", "линия", "район", "мкад", "автодорога"];

        return streetMarkers.Any(marker => lower.Contains(marker));
    }

    private static string? ExtractHouseNumber(string part)
    {
        if (string.IsNullOrWhiteSpace(part))
            return null;

        var match = System.Text.RegularExpressions.Regex.Match(
         part,
         @"домовладение\s*(\S+)|дом\s*№?\s*([\d\w\/\-]+)|д\.\s*([\d\w\/\-]+)|з\/у\s*([\d\w\/\-]+)|стр\s*([\d\w\/\-]+)",
         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        if (match.Success)
        {
            for (int i = 1; i < match.Groups.Count; i++)
            {
                if (match.Groups[i].Success && !string.IsNullOrWhiteSpace(match.Groups[i].Value))
                    return match.Groups[i].Value.Trim();
            }
        }

        if (part.Any(char.IsDigit) && !IsRegionPart(part) && !IsCityPart(part) && !IsStreetPart(part))
            return part.Trim();

        return null;
    }
}