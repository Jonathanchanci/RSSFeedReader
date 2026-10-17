# Data Model: RSS Subscription Management

## Subscription

Represents one submitted feed URL in the current API process.

| Field | Type | Rules |
|---|---|---|
| `url` | string | Required; null, empty, or whitespace-only values are rejected. Any other value is preserved exactly. URI/feed validity is not checked. |

### Collection behavior

- The API keeps subscriptions in insertion order in process memory.
- Duplicate URL values are distinct entries and remain in their submitted positions.
- A successful add is visible to subsequent list requests without restarting either process.
- The collection is empty after the API process starts and is discarded when that process stops.
- No database, persistence, removal, feed items, or user/account relationship is part of this model.
