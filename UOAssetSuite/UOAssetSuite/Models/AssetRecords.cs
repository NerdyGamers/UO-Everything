using System.Text;

namespace UOAssetSuite.Models;

public sealed record ClientVersionValidation(bool IsSupported, string Message);

public sealed record MapTilePreview(UOMapId Map, int X, int Y, string Description);

public static class AssetPreviewFormatter
{
    public static string FormatObject(object? value)
    {
        if (value is null)
        {
            return "No record returned.";
        }

        var type = value.GetType();
        if (value is System.Collections.IEnumerable enumerable && value is not string)
        {
            var lines = new List<string>();
            var index = 0;
            foreach (var item in enumerable)
            {
                lines.Add($"[{index++}] {FormatSingleObject(item)}");
                if (index >= 24)
                {
                    lines.Add("…");
                    break;
                }
            }

            return lines.Count == 0 ? $"{type.Name}: empty collection" : string.Join(Environment.NewLine, lines);
        }

        return FormatSingleObject(value);
    }

    private static string FormatSingleObject(object? value)
    {
        if (value is null)
        {
            return "<null>";
        }

        var type = value.GetType();
        if (type.IsPrimitive || value is string or decimal)
        {
            return value.ToString() ?? string.Empty;
        }

        var builder = new StringBuilder(type.Name);
        foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
        {
            if (property.GetIndexParameters().Length > 0)
            {
                continue;
            }

            object? propertyValue;
            try
            {
                propertyValue = property.GetValue(value);
            }
            catch
            {
                continue;
            }

            builder.AppendLine();
            builder.Append("  ").Append(property.Name).Append(": ").Append(propertyValue);
        }

        return builder.ToString();
    }
}
