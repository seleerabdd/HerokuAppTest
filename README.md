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

## Allure reporting

Allure is enabled via the `Allure.Reqnroll` package in the test project.

1. Run tests:

```powershell
dotnet test
```

2. Install the Allure CLI (one-time):

```bash
brew install allure
```

3. Generate and open the report (from solution root):

```bash
allure serve ReqnrollLogin.Tests/bin/Debug/net9.0/allure-results
```

If you run tests in a different configuration/framework, point `allure serve` at that run's `allure-results` directory.

## Run headless

```powershell
$env:HEADLESS = "true"
dotnet test
```

## Notes

- The sample test target is `https://the-internet.herokuapp.com`
- Scenarios include login flows and multi-page navigation tests
- Framework uses Page Object Model design pattern
