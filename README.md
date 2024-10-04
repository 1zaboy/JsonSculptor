# JsonSculptor

## Overview
The JsonSculptorExtension is a C# static extension class that provides functionality to map values from a JSON string to a predefined schema using breadcrumb-like references. It enables the replacement of placeholders in the schema with corresponding values from the JSON object.

## Features
- JSON Mapping: Replaces placeholders in a given schema string with actual values from a JSON object.
- Breadcrumb Navigation: Allows navigating the JSON structure using dot notation to specify the path to the value within the JSON.

## Benchmark
| Method           | Mean     | Error   | StdDev  |
|----------------- |---------:|--------:|--------:|
| BenchmarkMapJson | 966.8 ns | 8.30 ns | 6.93 ns |

## Usage
**Method**: MapJson

This method is the core functionality of the JsonSculptorExtension class. It takes two inputs:

- data: A JSON string that contains the data.
- schema: A schema string that may contain placeholders where the JSON values should be inserted.

## Placeholders Format:

- Placeholders in the schema should follow this format: {{$json.<breadcrumbs>}}, where <breadcrumbs> is the path within the JSON object (using dot notation).

**Example**
```csharp
string json = @"{
                    ""person"": {
                        ""name"": ""John"",
                        ""age"": 30
                    }
                }";

string schema = "Name: {{$json.person.name}}, Age: {{$json.person.age}}";

string result = schema.MapJson(json);

// Result: "Name: John, Age: 30"
```
In this example, the schema contains placeholders like {{$json.person.name}}. The MapJson method replaces these placeholders with actual values from the json string (in this case, "John" and "30").

## Method Details
**MapJson(this string data, string schema)**

This is the main extension method. It:\
- Finds all JSON placeholders in the schema using a regular expression.
- Parses the data JSON string.
- Replaces each placeholder in the schema with the corresponding value from the JSON object.

## Dependencies
- This implementation relies on the System.Text.Json namespace for parsing and manipulating JSON data.
- .NET 7.0 or later versions are required for System.Text.Json features and partial methods.

## Installation
To use the JsonSculptorExtension in your project, follow these steps:

- Include the class in your C# project.
- Ensure that the project references System.Text.Json.

## Limitations
The JSON path (breadcrumbs) currently supports both object and array navigation but assumes valid JSON structure without deeper error handling.
Only handles simple placeholders in a schema string and does not support complex transformations.
Conclusion
The JsonSculptorExtension provides a simple way to map and replace JSON values into a schema template using dot notation paths. It is useful for generating custom output formats dynamically based on JSON input.