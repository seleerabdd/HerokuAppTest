# Medefer QA Technical Test

⚠️ **THIS VERSION CONTAINS INTENTIONAL BUGS FOR INTERVIEW TESTING** ⚠️

## Your Task

Write login test scenarios and fix all issues to get all tests passing. See [INTERVIEW_TASKS.md](INTERVIEW_TASKS.md) for detailed instructions.

## Quick Start

```powershell
dotnet test
```

You will see errors. Your job is to fix them all.

---

# Original Framework Documentation

Simple BDD login test framework using:
- .NET 9
- NUnit
- Reqnroll
- Microsoft Playwright

## Project structure

- `ReqnrollLogin.Tests/Features` - Gherkin scenarios
- `ReqnrollLogin.Tests/StepDefinitions` - Step bindings
- `ReqnrollLogin.Tests/Pages` - Page objects
- `ReqnrollLogin.Tests/Hooks` - Scenario setup/teardown
- `ReqnrollLogin.Tests/Drivers` - WebDriver factory
- `ReqnrollLogin.Tests/Support` - Shared test context

## Run tests

From the solution folder:

```powershell
dotnet test
```

## Run headless

```powershell
$env:HEADLESS = "true"
dotnet test
```

## Notes

- The sample test target is `https://the-internet.herokuapp.com`
- Scenarios include login flows and multi-page navigation tests
- Framework uses Page Object Model design pattern
