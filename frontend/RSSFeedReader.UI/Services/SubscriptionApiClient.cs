using System.Net.Http.Json;
using RSSFeedReader.UI.Models;

namespace RSSFeedReader.UI.Services;

public class SubscriptionApiClient
{
    private readonly HttpClient _httpClient;

    public SubscriptionApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Subscription>> GetSubscriptionsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Subscription>>("subscriptions") ?? new List<Subscription>();
    }

    public async Task<Subscription> AddSubscriptionAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("URL cannot be blank.", nameof(url));
        }

        var response = await _httpClient.PostAsJsonAsync("subscriptions", new Subscription { Url = url });

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Unable to add subscription: {response.StatusCode}");
        }

        return await response.Content.ReadFromJsonAsync<Subscription>() ?? new Subscription();
    }
}
