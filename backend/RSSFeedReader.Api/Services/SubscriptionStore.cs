using RSSFeedReader.Api.Models;

namespace RSSFeedReader.Api.Services;

public class SubscriptionStore
{
    private readonly List<Subscription> _subscriptions = new();

    public IReadOnlyList<Subscription> GetAll() => _subscriptions.ToList();

    public Subscription Add(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("Subscription URL cannot be null, empty, or whitespace.", nameof(url));
        }

        var item = new Subscription { Url = url };
        _subscriptions.Add(item);
        return item;
    }
}
