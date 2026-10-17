using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RSSFeedReader.Api.Tests;

public class SubscriptionsEndpointTests
{
    [Fact]
    public async Task Get_returns_empty_list_when_no_subscriptions_have_been_added()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/subscriptions");

        response.EnsureSuccessStatusCode();
        var subscriptions = await response.Content.ReadFromJsonAsync<List<Subscription>>();

        Assert.NotNull(subscriptions);
        Assert.Empty(subscriptions!);
    }

    [Fact]
    public async Task Post_adds_a_subscription_and_get_returns_it_in_order()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();
        var payload = new Subscription { Url = "https://example.com/feed.xml" };

        var postResponse = await client.PostAsJsonAsync("/api/subscriptions", payload);
        postResponse.EnsureSuccessStatusCode();

        var response = await client.GetAsync("/api/subscriptions");
        response.EnsureSuccessStatusCode();

        var subscriptions = await response.Content.ReadFromJsonAsync<List<Subscription>>();

        Assert.NotNull(subscriptions);
        Assert.Collection(subscriptions!,
            item => Assert.Equal("https://example.com/feed.xml", item.Url));
    }

    [Fact]
    public async Task Post_preserves_url_text_exactly_and_allows_duplicate_entries()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var first = new Subscription { Url = "https://example.com/feed.xml" };
        var second = new Subscription { Url = "https://example.com/feed.xml" };

        var firstPost = await client.PostAsJsonAsync("/api/subscriptions", first);
        var secondPost = await client.PostAsJsonAsync("/api/subscriptions", second);

        firstPost.EnsureSuccessStatusCode();
        secondPost.EnsureSuccessStatusCode();

        var response = await client.GetAsync("/api/subscriptions");
        response.EnsureSuccessStatusCode();

        var subscriptions = await response.Content.ReadFromJsonAsync<List<Subscription>>();

        Assert.NotNull(subscriptions);
        Assert.Equal(2, subscriptions!.Count);
        Assert.Equal(first.Url, subscriptions[0].Url);
        Assert.Equal(second.Url, subscriptions[1].Url);
    }

    [Fact]
    public async Task Post_with_blank_url_returns_400_and_does_not_mutate_state()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();
        var initial = await client.GetAsync("/api/subscriptions");
        initial.EnsureSuccessStatusCode();
        var before = await initial.Content.ReadFromJsonAsync<List<Subscription>>();

        var blankRequest = new Subscription { Url = "   " };
        var response = await client.PostAsJsonAsync("/api/subscriptions", blankRequest);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var afterResponse = await client.GetAsync("/api/subscriptions");
        afterResponse.EnsureSuccessStatusCode();
        var after = await afterResponse.Content.ReadFromJsonAsync<List<Subscription>>();

        Assert.NotNull(before);
        Assert.NotNull(after);
        Assert.Equal(before!.Count, after!.Count);
    }

    [Fact]
    public async Task New_api_process_starts_with_an_empty_subscription_list()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var first = await client.GetAsync("/api/subscriptions");
        first.EnsureSuccessStatusCode();
        var value = await first.Content.ReadFromJsonAsync<List<Subscription>>();

        Assert.NotNull(value);
        Assert.Empty(value!);
    }
}

public class Subscription
{
    public string Url { get; set; } = string.Empty;
}
