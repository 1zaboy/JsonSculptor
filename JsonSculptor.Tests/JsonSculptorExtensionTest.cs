using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using JetBrains.Annotations;

namespace JsonSculptor.Tests;

[TestSubject(typeof(JsonSculptorExtension))]
public class JsonSculptorExtensionTest
{

    [Fact]
    public void Test()
    {
        const string data = """
                            {
                               "nam": "vlad"
                            }
                            """;

        const string schema = """
                              {
                                 "name": {{$json.nam}}
                              }
                              """;
        var result = data.MapJson(schema);
        const string needResult = """
                                  {
                                     "name": "vlad"
                                  }
                                  """;
        Assert.Equal(needResult, result);
    }
    
    
    [Fact]
    public void BenchmarkMethod()
    {
        var data = "{\"name\": \"John\", \"age\": 30, \"address\": {\"city\": \"New York\"}}";
        var schema = "User Name: {{$json.name}}, Age: {{$json.age}}, City: {{$json.address.city}}";
        var result = JsonSculptorExtension.MapJson(data, schema);
        const string needResult = "User Name: \"John\", Age: 30, City: \"New York\"";
        Assert.Equal(needResult, result);
    }
    
    [Fact]
    public void TestArray()
    {
        var data = "{\"name\": [{\"age\": 4}, {\"age\": 6}, {\"age\": 5}]}";
        var schema = "User Name: {{$json.name.[2].age}}";
        var result = JsonSculptorExtension.MapJson(data, schema);
        const string needResult = "User Name: 6";
        Assert.Equal(needResult, result);
    }
}