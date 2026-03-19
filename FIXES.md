# Medefer QA Technical Test - Fixes and Test Design

## Summary

This exercise was completed in three phases:
1. Build stability and compilation fixes
2. Login scenario authoring and step implementation
3. Framework debugging to remove runtime failures and flakiness

Final state:
1. Build succeeds with 0 errors
2. All implemented scenarios pass
3. Login and navigation workflows run reliably with async-safe page interactions

## Phase 0: Build Foundation

### Issue 1: Compilation error in secure area page
- Symptom: `dotnet build` failed with CS1519 errors in secure area page.
- Root cause: Invalid method signature syntax in `SecureAreaPage`.
- Fix: Corrected method declaration to a valid async generic return type in `ReqnrollLogin.Tests/Pages/SecureAreaPage.cs`.
- Why it mattered: Project could not compile, so no tests could run until this was fixed.

### Result
- Build moved from failing to successful and enabled test execution.

## Phase 1: Test Authoring and Step Definitions

## Test Design

Two login scenarios were created in `ReqnrollLogin.Tests/Features/Login.feature`:
1. Successful login with valid credentials, secure-page checks, and logout
2. Failed login with invalid credentials and error validation

Design rationale:
1. Positive and negative paths were both covered to validate authentication behavior comprehensively.
2. Secure area data table assertions validate page content, not only URL changes.
3. Logout validation confirms complete session lifecycle, not just login success.

## Step Implementation

Login step definitions were implemented in `ReqnrollLogin.Tests/StepDefinitions/LoginSteps.cs` using:
1. Page objects from the Pages layer
2. Async/await for all browser operations
3. Shared flash message state for cross-step assertions
4. FluentAssertions for readable and reliable validations

## Phase 2: Debugging and Framework Fixes

### Issue 2: Page not initialized in UI scenarios
- Symptom: Runtime failure indicating page was not initialized.
- Root cause: UI page setup in hooks was not being executed for tagged scenarios.
- Fix: Ensured scenario setup creates browser page in `ReqnrollLogin.Tests/Hooks/TestHooks.cs` and scenarios use the `@ui` tag in `ReqnrollLogin.Tests/Features/Login.feature` and `ReqnrollLogin.Tests/Features/Navigation.feature`.
- Why it mattered: Without initialization, every UI step fails immediately.

### Issue 3: Missing login page interaction methods
- Symptom: Login actions could not be executed reliably from steps.
- Root cause: Login page object lacked needed interaction helpers.
- Fix: Added username/password entry, submit action, and flash message retrieval in `ReqnrollLogin.Tests/Pages/LoginPage.cs`.
- Why it mattered: Step definitions depend on these methods for reusable and maintainable UI actions.

### Issue 4: Dropdown locator mismatch causing timeout
- Symptom: Dropdown scenario timed out while selecting option.
- Root cause: Incorrect dropdown locator.
- Fix: Updated dropdown locator usage and selected option retrieval in `ReqnrollLogin.Tests/Pages/DropdownPage.cs`.
- Why it mattered: Incorrect locator caused hard timeout and scenario failure.

### Issue 5: Step binding integrity
- Symptom: Risk of undefined step bindings during framework debugging.
- Root cause: Binding setup must be explicit and consistent across step classes.
- Fix: Verified binding attribute and active step class wiring in `ReqnrollLogin.Tests/StepDefinitions/NavigationSteps.cs` and `ReqnrollLogin.Tests/StepDefinitions/LoginSteps.cs`.
- Why it mattered: Missing or incorrect bindings break scenario execution before test logic is evaluated.

### Issue 6: Async reliability improvements
- Symptom: Potential timing-related failures in UI assertions.
- Root cause: UI reads can occur before elements are visible.
- Fix: Added and used explicit waits in page methods such as `ReqnrollLogin.Tests/Pages/CommonPage.cs`, `ReqnrollLogin.Tests/Pages/SecureAreaPage.cs`, and `ReqnrollLogin.Tests/Pages/LoginPage.cs`.
- Why it mattered: Explicit synchronization reduced race conditions and improved stability.

## Verification

1. Build check completed successfully.
2. Test suite execution completed with all scenarios passing.
3. Core deliverables validated:
- Login feature scenarios present in `ReqnrollLogin.Tests/Features/Login.feature`
- Login step methods implemented in `ReqnrollLogin.Tests/StepDefinitions/LoginSteps.cs`
- Page object and hook fixes applied across Pages and Hooks folders

## Optional Enhancement Completed

Allure reporting support was added for richer test result visualization:
- Package integration in `ReqnrollLogin.Tests/MedeferQATechTest.csproj`
- Usage documentation in `README.md`

---

## Phase 3: Production Hardening - Resource Lifecycle & Deterministic Disposal

### Issue 7: Playwright resource leaks and incomplete cleanup
- **Symptom:** Resources not disposed cleanly; potential memory leaks on CI after repeated test runs; Playwright singleton not disposed.
- **Root cause:** Resource ownership was split across factory (creation) and hooks (cleanup). Page was closed but browser context and Playwright instance were never disposed.
- **Fix:** 
  1. Created `ReqnrollLogin.Tests/Support/PlaywrightSession.cs` implementing `IAsyncDisposable` to own all Playwright resources (Playwright → Browser → Context → Page) in a single object with deterministic cleanup in reverse creation order.
  2. Updated `ReqnrollLogin.Tests/Support/TestContext.cs` to hold a `PlaywrightSession` instead of separate `IPage` and `IBrowser` fields, with a convenience property forwarding to `Session.Page`.
  3. Refactored `ReqnrollLogin.Tests/Drivers/WebDriverFactory.cs` to return a `PlaywrightSession` instead of raw `IPage`, with all resource creation encapsulated.
  4. Updated `ReqnrollLogin.Tests/Hooks/TestHooks.cs` to create session in `BeforeScenario` and call `DisposeAsync()` in `AfterScenario`, ensuring proper cleanup with error handling.
- **Why it mattered:** 
  - Prevents resource leaks in CI environments where tests run repeatedly
  - Guarantees cleanup order: Page → Context → Browser → Playwright (reverse of creation)
  - IAsyncDisposable pattern enforces proper async cleanup semantics
  - Centralizes lifecycle responsibility in one object (single responsibility principle)
  - Error handling during cleanup prevents cascading failures

### Verification

1. Build check: `dotnet build` - 0 errors, 0 warnings ✓
2. Test suite: `dotnet test` - All 5 scenarios passing ✓
3. Resource cleanup: Page → Context → Browser → Playwright disposal order enforced ✓
4. Unused context fields removed; Browser field eliminated in favor of session ownership ✓

### Impact

| Aspect | Before | After |
|--------|--------|-------|
| **Playwright disposal** | Never disposed (leak) | Properly disposed in `DisposeAsync()` |
| **Resource ownership** | Split across factory and hooks | Centralized in `PlaywrightSession` |
| **Cleanup order** | Page → Browser (context leaked) | Page → Context → Browser → Playwright (correct) |
| **Async disposal pattern** | Manual, incomplete | `IAsyncDisposable` enforced with try-catch |
| **Unused fields** | `IBrowser` field in context unused | Removed; all resources owned by session |
| **Testability** | Hard to mock resources | Session can be swapped/mocked for testing |

---

### Issue 8: No failure diagnostics for debugging CI failures
- **Symptom:** When tests fail in CI, only error message is available; no visual context of application state at failure.
- **Root cause:** Hooks only created/disposed resources; no diagnostic capture on failure.
- **Fix:**
  1. Updated `ReqnrollLogin.Tests/Hooks/TestHooks.cs` to inject `ScenarioContext` to detect test failures.
  2. Added `CaptureFailureDiagnosticsAsync()` method to capture on failure:
     - Full-page screenshot (saved as PNG)
     - Page HTML dump (saved as HTML)
     - Page title, URL, and error message (saved as context.txt)
  3. Added `AttachArtifactToAllure()` method to attach each artifact to Allure report using `AllureApi.AddAttachment()`.
  4. Artifacts stored in `failure-diagnostics/` directory with timestamp and scenario name for easy identification.
  5. Error handling ensures diagnostic failures don't cascade or fail the test.
- **Why it mattered:**
  - Accelerates root cause analysis for test failures, especially in headless CI runs
  - Visual evidence (screenshot) shows page state at failure moment
  - HTML dump reveals DOM structure and element visibility issues
  - Context info (URL, title) helps correlate with test step
  - Allure integration embeds diagnostics directly in test report for quick review
  - Artifacts saved locally for offline analysis if needed

### Verification

1. Build check: `dotnet build` - 0 errors, 0 warnings ✓
2. All 5 test scenarios passing - diagnostics don't interfere with passing tests ✓
3. Failure diagnostics integrated into Allure report lifecycle ✓
4. Error handling prevents diagnostic capture from breaking cleanup ✓

### Diagnostic Workflow

When a scenario fails:
```
Scenario Failure → ScenarioContext.TestError detected → 
  Capture screenshot → Capture HTML → Capture context info → 
  Attach to Allure → Dispose session → Report result
```

No diagnostics are captured for passing tests (performance optimized).


