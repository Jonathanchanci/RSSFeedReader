using RSSFeedReader.Api.Models;
using RSSFeedReader.Api.Services;

namespace RSSFeedReader.Api.Endpoints;

public static class SubscriptionsEndpoints
{
    public static void MapSubscriptionsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/subscriptions");

        group.MapGet("", (SubscriptionStore store) => Results.Ok(store.GetAll()));

        group.MapPost("", (Subscription? subscription, SubscriptionStore store) =>
        {
            if (subscription is null || string.IsNullOrWhiteSpace(subscription.Url))
            {
                return Results.BadRequest();
            }

            var created = store.Add(subscription.Url);
            return Results.Ok(created);
        });
    }
}
