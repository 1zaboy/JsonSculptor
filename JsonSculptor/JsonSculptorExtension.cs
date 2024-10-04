using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace JsonSculptor;

public static partial class JsonSculptorExtension
{
    public static string MapJson(this string data, string schema)
    {
        var items = JsonBreadcrumbsRegex().Matches(schema);
        if (items.Count == 0)
            throw new Exception("Cannot find matches");

        var node = JsonNode.Parse(data);

        if (node == null)
            throw new Exception($"{nameof(data)} is not json string");

        var result = new StringBuilder(schema);
        foreach (Match item in items)
        {
            var breadcrumbs = item.Value[8..^2];
            var breadcrumbsArray = breadcrumbs.Split('.');
            var newValue = FindInJson(node, breadcrumbsArray);
            result = result.Replace(item.Value, newValue);
        }

        return result.ToString();
    }

    private static string FindInJson(JsonNode node, string[] items)
    {
        ArgumentNullException.ThrowIfNull(node);

        var result = node;
        foreach (var item in items)
        {
            if (result == null)
                throw new Exception($"Incorrect breadcrumbs");

            result = item.StartsWith('[') && item.EndsWith(']')
                ? result.AsArray()[item[1..^1]]
                : result[item]!;
        }

        return result!.AsValue().ToJsonString();
    }

    [GeneratedRegex(@"\{\{\$json\..+?\}\}")]
    private static partial Regex JsonBreadcrumbsRegex();
}