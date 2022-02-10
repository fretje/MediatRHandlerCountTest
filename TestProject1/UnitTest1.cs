using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Xunit;

namespace TestProject1;

public class UnitTest1
{
    [Fact]
    public async Task TestSomethingHappened()
    {
        WebApplicationFactory<Program>? application =
            new WebApplicationFactory<Program>();

        HttpClient? client = application.CreateClient();

        HandlerCounter? counts = await client.GetFromJsonAsync<HandlerCounter>("/somethingHappened");

        counts.ShouldNotBeNull();
        counts.SomethingHappenedCount.ShouldBe(1);
        counts.SimpleCount.ShouldBe(0);
        counts.GenericCount.ShouldBe(1);
    }

    [Fact]
    public async Task TestSimple()
    {
        WebApplicationFactory<Program>? application =
            new WebApplicationFactory<Program>();

        HttpClient? client = application.CreateClient();

        HandlerCounter? counts = await client.GetFromJsonAsync<HandlerCounter>("/simple");

        counts.ShouldNotBeNull();
        counts.SomethingHappenedCount.ShouldBe(0);
        counts.SimpleCount.ShouldBe(1);
        counts.GenericCount.ShouldBe(1);
    }

    [Fact]
    public async Task TestBoth()
    {
        WebApplicationFactory<Program>? application =
            new WebApplicationFactory<Program>();

        HttpClient? client = application.CreateClient();

        HandlerCounter? counts = await client.GetFromJsonAsync<HandlerCounter>("/both");

        counts.ShouldNotBeNull();
        counts.SomethingHappenedCount.ShouldBe(1);
        counts.SimpleCount.ShouldBe(1);
        counts.GenericCount.ShouldBe(2);
    }
}