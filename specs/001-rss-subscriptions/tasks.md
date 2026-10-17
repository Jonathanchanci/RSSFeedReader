---
description: "Task list for RSS subscription management"
---

# Tasks: RSS Subscription Management

**Input**: Design documents from `/specs/001-rss-subscriptions/`

**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/subscriptions.yaml`, `quickstart.md`

**Tests**: The specification defines independent acceptance scenarios; API integration tests are included using the xUnit approach in `research.md`.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel with another task because it touches different files and has no dependency on incomplete work
- **[Story]**: User story served by the task (US1)
- All task descriptions identify their target file paths.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Scaffold the API, UI, and test projects using the .NET 8 toolchain.

- [ ] T001 [P] Create `RSSFeedReader.sln` and scaffold the ASP.NET Core Web API project targeting `net8.0` in `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj`.
- [ ] T002 [P] Scaffold the standalone Blazor WebAssembly project targeting `net8.0` in `frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj`.
- [ ] T003 Add `frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj` to `RSSFeedReader.sln`.
- [ ] T004 Create the xUnit test project in `backend/RSSFeedReader.Api.Tests/RSSFeedReader.Api.Tests.csproj`, reference `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj`, and add it to `RSSFeedReader.sln`.

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Remove template routing conflicts and establish the local API/UI connection before feature implementation.

- [ ] T005 Remove the Blazor demo pages `frontend/RSSFeedReader.UI/Pages/Home.razor`, `frontend/RSSFeedReader.UI/Pages/Counter.razor`, and `frontend/RSSFeedReader.UI/Pages/Weather.razor`; update `frontend/RSSFeedReader.UI/Layout/NavMenu.razor` to remove links to deleted pages and retain only MVP navigation, then verify that only the intended page uses `@page "/"`.
- [ ] T006 Configure the API launch URL as `http://localhost:5151` in `backend/RSSFeedReader.Api/Properties/launchSettings.json`, the UI launch URL as `http://localhost:5213` in `frontend/RSSFeedReader.UI/Properties/launchSettings.json`, the API base URL in `frontend/RSSFeedReader.UI/wwwroot/appsettings.json`, and API CORS origins in `backend/RSSFeedReader.Api/Program.cs` to allow the configured UI origin.

**Checkpoint**: Both projects build, template routes do not conflict, and the configured UI origin can call the API.

---

## Phase 3: User Story 1 - Add and view feed subscriptions (Priority: P1) - MVP

**Goal**: Let the user submit opaque, nonblank feed URL text and see every submitted value in insertion order during the API process lifetime, including duplicates.

**Independent Test**: Start the API and UI, add two different nonblank URLs and the same URL twice, and confirm all four entries appear in submission order without reloading. Submit blank/whitespace-only input and confirm it is not added. Restart the API and confirm the list is empty.

### Tests for User Story 1

- [ ] T007 [US1] Add API integration tests in `backend/RSSFeedReader.Api.Tests/SubscriptionsEndpointTests.cs` for empty-list retrieval, successful add and retrieval, exact URL preservation, duplicate entries, blank-input `400` with no mutation, and an empty collection after a fresh API process starts; ensure these tests fail before implementing the endpoints.

### Implementation for User Story 1

- [ ] T008 [US1] Create the `Subscription` response/request model with its `Url` field in `backend/RSSFeedReader.Api/Models/Subscription.cs`.
- [ ] T009 [US1] Implement an insertion-ordered in-memory subscription store that rejects null, empty, and whitespace-only values while preserving every other URL exactly and allowing duplicates in `backend/RSSFeedReader.Api/Services/SubscriptionStore.cs`.
- [ ] T010 [US1] Implement `GET /api/subscriptions` and `POST /api/subscriptions` in `backend/RSSFeedReader.Api/Endpoints/SubscriptionsEndpoints.cs` and map the endpoint group from `backend/RSSFeedReader.Api/Program.cs`; return the ordered array for GET, the created subscription with `200 OK` for POST, and `400 Bad Request` for blank input without mutating state. Do not validate, fetch, or parse feed URLs.
- [ ] T011 [US1] Implement and register a Blazor API client that calls the configured `/api/subscriptions` endpoint using `HttpClient` and JSON in `frontend/RSSFeedReader.UI/Services/SubscriptionApiClient.cs` and `frontend/RSSFeedReader.UI/Program.cs`.
- [ ] T012 [US1] Implement the root subscriptions page in `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor` with URL entry, blank-input handling, API-backed list loading, and immediate list refresh after add; display submitted text as-is and retain duplicates.

**Checkpoint**: User Story 1 is independently functional through the UI and API, with the required behavior covered by API integration tests.

---

## Phase 4: Polish & Cross-Cutting Concerns

**Purpose**: Verify the delivered workflow and local configuration against the feature quickstart.

- [ ] T013 Run the API test suite and build the API and UI projects from `backend/RSSFeedReader.Api.Tests/RSSFeedReader.Api.Tests.csproj`, `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj`, and `frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj`; complete the manual add/list/duplicate/blank-input/API-restart checks in `specs/001-rss-subscriptions/quickstart.md`.

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies; scaffold the projects first.
- **Foundational (Phase 2)**: Depends on project scaffolding and blocks User Story 1.
- **User Story 1 (Phase 3)**: Starts after the foundational checkpoint; this is the only story in the feature.
- **Polish (Phase 4)**: Depends on User Story 1 implementation.

### User Story Dependencies

- **User Story 1 (P1)**: Depends on Setup and Foundational completion; has no dependency on another story.

### Within User Story 1

- Write and run the API integration tests before implementing the model, store, or endpoints.
- Implement the model before the in-memory store, and the store before endpoint behavior.
- Implement the API before the UI client and page integration.

### Parallel Opportunities

- **T001 and T002** can run in parallel because they scaffold separate projects; add the UI project to the solution after both exist.
- No user-story implementation tasks are marked parallel: API behavior is a prerequisite for end-to-end UI integration, and the store is a prerequisite for the endpoints.

## Implementation Strategy

### MVP First

1. Complete project setup and foundational route/configuration work.
2. Implement and verify User Story 1, including its API integration tests.
3. Complete the build, test, and manual quickstart checks; this single story is the full MVP scope.

### Incremental Delivery

The feature contains one user story, so there are no later story increments. Keep feed fetching, parsing, persistence, removal, and background polling out of scope.

## Notes

- `[P]` marks only independent tasks targeting separate project files.
- Preserve URL text exactly except that null, empty, and whitespace-only submissions are rejected.
- The API process lifetime defines the in-memory subscription session; closing only the browser tab does not clear API state.