using BenchmarkDotNet.Attributes;

namespace JsonSculptor.Benchmark;

public class JsonSculptorBenchmark
{
    private string data;
    private string schema;

    [GlobalSetup]
    public void Setup()
    {
        data = "{\"name\": \"John\", \"age\": 30, \"address\": {\"city\": \"New York\"}}";
        schema = "User Name: {{$json.name}}, Age: {{$json.age}}, City: {{$json.address.city}}";
    }

    [Benchmark]
    public string BenchmarkMapJson()
    {
        return data.MapJson(schema);
    }
}