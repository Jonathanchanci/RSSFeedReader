<!--
Sync Impact Report
Version change: unversioned scaffold -> 1.0.0
Modified principles: none; all five principles established for this project
Added sections: Project Constraints; Development and Quality Gates
Removed sections: none
Follow-up TODOs: confirm original ratification date
-->
# RSS Feed Reader Constitution

## Core Principles

### I. Security at Network Boundaries
All externally supplied values MUST be treated as untrusted data. The MVP MUST NOT fetch or parse feed URLs. When feed fetching is introduced, requests MUST use safe HTTP handling, enforce applicable network and response limits, and never render feed-provided HTML as trusted markup. Secrets and environment-specific endpoints MUST NOT be committed to source control.

### II. Deliver the Agreed Phase
Each change MUST fit the MVP or explicitly approved later phase described in the stakeholder documents. The MVP is limited to adding a subscription URL and displaying the in-memory subscription list; feed fetching, parsing, persistence, removal, and background polling are out of scope. Work outside the current phase MUST be deferred rather than added as incidental complexity.

### III. Keep Responsibilities Maintainable
The ASP.NET Core API MUST own subscription state and expose it through explicit API contracts; the Blazor WebAssembly UI MUST use those contracts rather than duplicate backend state rules. Configuration such as API endpoints MUST be externalized from application logic. Implementations MUST prefer the simplest design that meets the current phase and avoid abstractions for deferred features.

### IV. Verify Observable Behavior
Changes MUST be checked at the narrowest useful level, including tests for changed behavior when practical. Subscription changes MUST preserve the add-and-list workflow across API and UI boundaries. Before a change is considered complete, contributors MUST run applicable build and test checks; when automated coverage is unavailable, they MUST perform and report a relevant manual check.

### V. Make Quality Changes Reviewable
Code MUST use clear names, focused components, and explicit error behavior appropriate to the current feature. Changes MUST preserve existing public contracts unless a change is required and documented. Reviews and implementation checks MUST consider security, maintainability, scope, and verification rather than treating successful compilation alone as sufficient.

## Project Constraints

The application uses an ASP.NET Core Web API backend and a Blazor WebAssembly frontend. During the MVP, subscriptions are held in memory, URLs are accepted without feed validation, and no feed network requests or parsing are performed. Later feed refresh functionality is manual only unless the scope is amended. Stakeholder documents define product phases and stack details; this constitution governs how those requirements are implemented.

## Development and Quality Gates

For changes affecting API/UI integration, verify the configured backend URL, CORS origin, and both applications' startup behavior. Run the relevant .NET build and tests for changed projects. Tests SHOULD cover subscription addition and retrieval, and UI changes SHOULD verify that the displayed list reflects API state. Any skipped check or known limitation MUST be stated with the change.

## Governance

This constitution is the project-wide standard for implementation and review. Amendments MUST state their rationale and affected principles, and MUST update the version and last-amended date. Versioning follows semantic rules: MAJOR for incompatible governance changes, MINOR for new or materially expanded principles or sections, and PATCH for non-semantic clarifications. Contributors and reviewers MUST check changes against these principles and the active product phase. If stakeholder requirements conflict, clarify the intended scope before implementation.

**Version**: 1.0.0 | **Ratified**: TODO(RATIFICATION_DATE): original adoption date is unknown | **Last Amended**: 2026-10-17
