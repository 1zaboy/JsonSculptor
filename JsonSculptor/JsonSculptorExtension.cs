using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace JsonSculptor;

public static partial class JsonSculptorExtension
{
    public static string MapJson(this string data, string schema)
    {
        var items = JsonBreadcrumbsRegex().Matches(schema);
        if (items.Count == 0)
            throw new Exception("Cannot find matches");

        JsonDocument document;
        try
        {
            document = JsonDocument.Parse(data);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        var result = new StringBuilder(schema);
        foreach (Match item in items)
        {
            var breadcrumbs = item.Value[8..^2];
            var breadcrumbsArray = breadcrumbs.Split('.');
            var newValue = FindInJson(document, breadcrumbsArray);
            result = result.Replace(item.Value, newValue);
        }

        return result.ToString();
    }

    private static string FindInJson(JsonDocument node, string[] items)
    {
        ArgumentNullException.ThrowIfNull(node);

        var result = node.RootElement;
        foreach (var item in items)
        {
            if (result.ValueKind == JsonValueKind.Undefined)
                throw new Exception($"Incorrect breadcrumbs");

            if (result.ValueKind == JsonValueKind.Array)
            {
                if (item.StartsWith('[') && item.EndsWith(']'))
                {
                    result = result[Convert.ToInt32(item[1..^1])];
                }
                else
                {
                    throw new Exception("Array index is invalid");
                }
            }
            else
            {
                if (result.TryGetProperty(item, out var value))
                {
                    result = value;
                }
                else
                {
                    throw new Exception("Property cannot found");
                }
            }
        }

        return result.ValueKind switch
        {
            JsonValueKind.Undefined => throw new Exception($"Incorrect breadcrumbs"),
            JsonValueKind.Null => "null",
            JsonValueKind.String => $"\"{result.ToString()}\"",
            JsonValueKind.True => result.ToString().ToLower(),
            JsonValueKind.False => result.ToString().ToLower(),
            _ => result.ToString()
        };
    }

    [GeneratedRegex(@"\{\{\$json\..+?\}\}")]
    private static partial Regex JsonBreadcrumbsRegex();
}