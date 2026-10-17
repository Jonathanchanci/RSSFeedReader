# Quickstart: Validate RSS Subscription Management

## Prerequisites

- .NET 8 SDK
- A browser
- The backend and frontend projects scaffolded according to `plan.md`

## Build and test

From the repository root, build and run backend tests:

```powershell
dotnet build backend/RSSFeedReader.Api.Tests
dotnet test backend/RSSFeedReader.Api.Tests
```

## Run the local apps

Start the API in one terminal on the configured API port:

```powershell
dotnet run --project backend/RSSFeedReader.Api --urls http://localhost:5151
```

Start the Blazor WebAssembly UI in another terminal using its configured launch profile:

```powershell
dotnet run --project frontend/RSSFeedReader.UI
```

Confirm `frontend/RSSFeedReader.UI/wwwroot/appsettings.json` points to `http://localhost:5151/api/` and API CORS allows the actual UI origin (stakeholder default: `http://localhost:5213`). Open the UI and submit three nonblank URLs, including the same URL twice. Every submitted value should appear in order, duplicates included, without a reload.

## Verify the API contract

With the API running, these PowerShell requests should show the add/list and blank-input behavior:

```powershell
$base = 'http://localhost:5151/api/subscriptions'
Invoke-RestMethod -Method Get -Uri $base
Invoke-RestMethod -Method Post -Uri $base -ContentType 'application/json' -Body '{"url":"https://example.com/feed.xml"}'
Invoke-RestMethod -Method Post -Uri $base -ContentType 'application/json' -Body '{"url":"https://example.com/feed.xml"}'
Invoke-RestMethod -Method Get -Uri $base
```

The final GET should contain two separate entries in insertion order. A POST with `{"url":"   "}` should return `400 Bad Request`; a subsequent GET should be unchanged. No test should cause a request to either submitted feed URL.

## Verify session reset

Stop the API process, start it again, and GET `/api/subscriptions`. The response should be an empty array. Closing only the browser tab while leaving the API process running does not reset API-owned in-memory state.
