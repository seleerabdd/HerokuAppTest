# Medefer QA Technical Test - Interview Exercise

## Overview

This is a hands-on interview exercise with three phases:

1. **Phase 0: Build Foundation** - Fix compilation errors
2. **Phase 1: Test Authoring** - Write login tests and implement step definitions
3. **Phase 2: Debugging** - Find and fix framework bugs to make tests pass

## Your Mission

1. **Fix compilation errors** with `dotnet build`
2. **Write login test scenarios** and implement their step definitions
3. **Run tests** and identify failures
4. **Debug and fix** framework issues preventing tests from passing
5. **Verify all scenarios pass** with `dotnet test`

## Interview Flow

**Total Time:** 60 minutes recommended

- **Phase 0 (5 min):** Fix compilation errors with `dotnet build`
- **Phase 1 (25 min):** Write login scenarios + implement step definitions
- **Phase 2 (30 min):** Debug framework, fix bugs, get tests passing

---

## PHASE 0: BUILD FOUNDATION & COMPILATION

### TODO 0: Fix Compilation Errors (Priority: CRITICAL)

**Your job:**
- Run `dotnet build` to check compilation health
- Read compiler error messages and warnings carefully
- Identify and fix any syntax, type, or logic errors in the code
- Ensure the project compiles cleanly before proceeding

**Expected issues:**
- Compiler warnings or errors will appear in the output
- Stack traces will point you to exact file names and line numbers
- Use IDE IntelliSense to understand type and import requirements

**Success criteria:**
- `dotnet build` completes with 0 errors
- No compiler warnings remain (CS warnings like CS1998)
- Project is ready for test execution without compilation issues
---

## PHASE 1: TEST AUTHORING & STEP IMPLEMENTATION

### TODO 1: Write Login Test Scenarios (Priority: CRITICAL)

**Your job:**
- Write two Gherkin test scenarios in `Features/Login.feature`:
  1. **Successful login** - valid credentials, secure page access, logout
  2. **Failed login** - invalid credentials, error message
- Use the test site: `https://the-internet.herokuapp.com/login`

**How to approach:**
1. Write scenarios using proper Gherkin Given/When/Then format
2. Use existing step definitions where available
3. Create new step definitions as needed in `StepDefinitions/LoginSteps.cs`
4. Use data tables for complex assertions (secure area validation)
5. Test both happy path and error scenarios

**Success criteria:**
- Valid Gherkin syntax in both scenarios
- Proper use of Given/When/Then
- Both positive and negative test cases
- Logout flow in successful login scenario
- Appropriate assertions and data validation
---

### TODO 2: Implement Step Definitions (Priority: CRITICAL)

**Your job:**
- Implement each step definition method in `StepDefinitions/LoginSteps.cs`
- Use page objects to interact with UI elements
- Handle async/await properly
- Store state for assertions (e.g., flash messages)

**Implementation guide:**
- Access browser: `var page = _testContext.Page`
- Create page objects: `new LoginPage(page)`, `new SecureAreaPage(page)`
- Use FluentAssertions: `.Should().Contain()`, `.Should().Be()`
- Store messages in `_flashMessage` field for reuse
- Await async page operations

---

## PHASE 2: FRAMEWORK DEBUGGING & BUG FIXES

### TODO 3: Run and Identify Failures

**Your job:**
- Run `dotnet test` to execute login scenarios
- Read error messages and stack traces carefully
- Identify which bugs are blocking tests
- Document each bug as you discover it

**Success criteria:**
- Can run tests without crashes
- Understand why tests are failing

---

### TODO 4: Fix Step Binding Issues (Priority: HIGH)

**Symptoms:**
- Steps not binding: "No matching step definition found"
- Wrong namespace or missing `[Binding]` attribute
- File: `StepDefinitions/NavigationSteps.cs`

**Your job:**
- Verify `[Binding]` attributes exist on all step definition classes
- Fix any namespace/binding issues

**Success criteria:**
- All steps bind correctly
- No "undefined steps" errors

---

### TODO 5: Fix Page Object Locators (Priority: MEDIUM)

**Symptoms:**
- Playwright timeouts finding elements
- Wrong CSS selectors or element IDs
- Tests fail on element interaction

**Your job:**
- Use browser DevTools to inspect the live site
- Compare selectors in page objects with actual DOM
- Fix invalid selectors in:
  - `Pages/CheckboxesPage.cs` - checkbox selectors
  - `Pages/DropdownPage.cs` - dropdown ID
  - Any other page object issues

**Success criteria:**
- Element lookups succeed
- Navigation and interactions work reliably

---

### TODO 6: Fix Async/Await Issues (Priority: MEDIUM)

**Symptoms:**
- Intermittent test failures
- Timing-sensitive flakiness
- Operations run before UI is ready

**Your job:**
- Review async call chains in page objects and steps
- Find missing `await` keywords
- Ensure all async methods are properly awaited
- Check `WaitForAsync` calls are awaited

**Success criteria:**
- Tests pass consistently (run 3+ times)
- No race condition failures

---

## Deliverables

1. **Login.feature** - Two complete scenarios (positive + negative)
2. **LoginSteps.cs** - All 8 step methods implemented
3. **Fixed page objects** - All bugs resolved
4. **Test execution proof** - All scenarios passing
5. **FIXES.md** document including:
   - **Test Design** - Why you wrote scenarios this way
   - **Bug Fixes** - Each bug found, how fixed, why it mattered

---

## Getting Started

1. Start with TODO 0 - Fix compilation errors with `dotnet build`
2. Move to TODO 1 - Write your login test scenarios
3. Then TODO 2 - Implement the step definitions
4. Try `dotnet test` and observe failures
5. Work through TODO 3-7 to fix framework issues
6. Run `dotnet test` after each fix
7. Document your work in FIXES.md as you go

---

## Questions?

Feel free to ask clarifying questions about:
- Expected behavior or test requirements
- Framework design or patterns
- How to use specific tools (browser DevTools, IDE search, etc.)

**Good luck!**
