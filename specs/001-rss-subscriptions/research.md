# Research: RSS Subscription Management

## Decisions

### API owns the subscription collection

- **Decision**: Keep one ordered collection in an API singleton service; expose it through `GET /api/subscriptions` and `POST /api/subscriptions`.
- **Rationale**: The TechStack document assigns storage and list retrieval to the backend, and the constitution requires the API to own subscription state. In-memory storage naturally clears when the API process exits.
- **Alternatives considered**: Keep state only in the browser, which would contradict the API ownership requirement; use a database, which would violate the MVP's no-persistence scope.

### Treat URLs as opaque submitted text

- **Decision**: Reject null, empty, and whitespace-only values. Preserve every other value exactly, in insertion order, and allow duplicates. Do not validate URI syntax or contact the URL.
- **Rationale**: This implements FR-004 through FR-008 and avoids introducing feed operations into subscription management.
- **Alternatives considered**: Trimming, normalization, URL validation, or duplicate detection would alter explicitly required behavior and are excluded.

### Keep the HTTP contract small

- **Decision**: `GET` returns an array of subscription objects with a single `url` field. `POST` accepts that object and returns the created object with `200 OK`; blank input returns `400 Bad Request` without mutating the list.
- **Rationale**: The endpoints directly support the add-and-list UI and make the observable behavior easy to test. No identifier is needed because deletion and per-subscription operations are out of scope.
- **Alternatives considered**: A `201 Created` response with a `Location` header would require inventing a resource identifier that the MVP does not use; returning a bare string array would make POST/GET payload shapes inconsistent.

### Configure the separate local origins explicitly

- **Decision**: Configure the Blazor client's API base URL and allow the UI origin in API CORS settings. Use the stakeholder defaults of API port 5151 and UI port 5213, and verify that both launch settings and configuration agree.
- **Rationale**: A browser WebAssembly client calling a separate API origin is subject to same-origin/CORS enforcement.
- **Alternatives considered**: Same-origin hosting or a development proxy could avoid CORS, but neither is needed for the stated two-process local setup.

### Interpret app-session lifetime as API-process lifetime

- **Decision**: Subscriptions live until the local application is stopped, including its API process. Closing only the browser tab does not clear the API's memory; restarting the API starts with an empty collection.
- **Rationale**: The chosen backend-owned in-memory store has process lifetime, and both stakeholder documents call for in-memory state lost when the app stops. The quickstart therefore verifies reset by stopping and restarting the API.
- **Alternatives considered**: Detecting browser-tab closure to clear shared API memory is unreliable when multiple tabs are open and adds lifecycle signaling beyond MVP scope. If “close the app” must mean closing a tab while leaving the API running, the spec and state ownership need clarification before implementation.

### Use the installed .NET 8 toolchain

- **Decision**: Target `net8.0`; use the .NET shared framework for ASP.NET Core and `System.Net.Http.Json`. Use xUnit for API tests when scaffolding the solution.
- **Rationale**: SDK 8.0.101 is installed, the stakeholder stack is compatible, and no solution or test conventions exist in the repository yet.
- **Alternatives considered**: Selecting a newer target without an installed SDK adds a toolchain prerequisite not present in this MVP setup. Revisit the target framework when the solution is scaffolded if the project establishes a different supported SDK.

## Validation and implementation references

- [Call a web API from ASP.NET Core Blazor](https://learn.microsoft.com/aspnet/core/blazor/call-web-api?view=aspnetcore-10.0): client-side `HttpClient`/JSON calls and CORS behavior.
- [ASP.NET Core dependency injection](https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-10.0): service lifetimes for API-owned state.
- Stakeholder port defaults and MVP limitations: `StakeholderDocuments/TechStack.md` and `StakeholderDocuments/ProjectGoals.md`.
