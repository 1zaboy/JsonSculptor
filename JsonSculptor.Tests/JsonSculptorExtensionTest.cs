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
        const string data = "{\"name\": true, \"age\": 30, \"address\": {\"city\": \"New York\"}}";
        const string schema = "User Name: {{$json.name}}, Age: {{$json.age}}, City: {{$json.address.city}}";
        var result = data.MapJson(schema);
        const string needResult = "User Name: true, Age: 30, City: \"New York\"";
        Assert.Equal(needResult, result);
    }
    
    [Fact]
    public void TestArray()
    {
        const string data = "{\"name\": [{\"age\": 4}, {\"age\": {\"w\": null}}, {\"age\": 5}]}";
        const string schema = "User Name: {{$json.name.[1].age.w}}";
        var result = data.MapJson(schema);
        const string needResult = "User Name: null";
        Assert.Equal(needResult, result);
    }
    
    [Fact]
    public void TestArray2()
    {
        const string data = "{\"name\": [{\"age\": 4}, {\"age\": {\"w\": null}}, {\"age\": 5}]}";
        const string schema = "User Name: {{$json.name.[2].age}}";
        var result = data.MapJson(schema);
        const string needResult = "User Name: 5";
        Assert.Equal(needResult, result);
    }
    
    [Fact]
    public void TestArray3()
    {
        const string data = "{\"name\": [{\"age\": 4}, {\"age\": {\"w\": null}}, {\"age\": 5}]}";
        const string schema = "User Name: {{$json.name}}";
        var result = data.MapJson(schema);
        const string needResult = "User Name: [{\"age\": 4}, {\"age\": {\"w\": null}}, {\"age\": 5}]";
        Assert.Equal(needResult, result);
    }
}