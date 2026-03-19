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

---

## CI/CD Pipeline (GitHub Actions)

Automated testing and reporting is configured via GitHub Actions. The pipeline runs on:
- Every push to `main` and `develop` branches
- Every pull request to `main` and `develop` branches

### Pipeline Jobs

#### 1. Build and Test (Primary Job)
- **Triggers:** Push or PR to main/develop
- **Runs on:** `ubuntu-latest`
- **Steps:**
  1. Checks out code
  2. Sets up .NET 9
  3. Restores dependencies
  4. Installs Playwright browsers for headless testing
  5. Builds the solution
  6. Runs all tests in headless mode (`HEADLESS=true`)
  7. Generates Allure HTML report from test results
  8. Uploads artifacts:
     - `test-results-trx` - NUnit XML test results (30 days retention)
     - `allure-results-json` - Allure JSON results (30 days retention)
     - `allure-report-html` - HTML report for viewing (30 days retention)
     - `failure-diagnostics` - Screenshots/HTML dumps on failure (30 days retention)
  9. Publishes test results to GitHub Actions UI
  10. Comments on PRs with test summary

#### 2. Test Matrix (Multi-Platform)
- **Triggers:** Same as primary job
- **Runs on:** Ubuntu, macOS, Windows (parallel)
- **Purpose:** Ensures tests pass across multiple operating systems
- **Skips on failure:** Individual OS failures don't block others

#### 3. Code Quality Checks
- **Triggers:** Same as primary job
- **Runs on:** `ubuntu-latest`
- **Steps:**
  1. Builds with `TreatWarningsAsErrors=true`
  2. Runs .NET analyzers
  3. Enforces strict code quality rules

#### 4. Code Coverage Analysis
- **Triggers:** Same as primary job
- **Runs on:** `ubuntu-latest`
- **Steps:**
  1. Runs tests with coverage collection (Coverlet)
  2. Generates Cobertura XML report
  3. Uploads to Codecov for trend analysis

### Accessing Results

#### From GitHub UI
1. Go to **Actions** tab in repository
2. Select the workflow run
3. View:
   - Test results summary in the job output
   - Check status badge on PR
   - PR comment with test metrics

#### Downloading Artifacts
1. In the workflow run page, scroll to **Artifacts** section
2. Download:
   - `allure-report-html` - Extract and open `index.html` in browser for interactive report
   - `failure-diagnostics` - Screenshots and HTML dumps for failed tests
   - `test-results-trx` - Import into IDE or test reporting tools

#### Codecov Integration
- Coverage reports are automatically uploaded to Codecov
- View coverage trends at: `https://codecov.io/gh/[owner]/[repo]`

### Local Test Execution vs CI

| Aspect | Local | CI |
|--------|-------|-----|
| **Headless** | Optional (default: headed) | Always headless |
| **Browsers** | Interactive window | None (headless only) |
| **Reports** | Manual generation | Auto-generated |
| **Artifacts** | Local only | GitHub uploaded |
| **Coverage** | Optional | Always collected |

### Debugging CI Failures

If tests pass locally but fail in CI:

1. **Check Allure Report:**
   - Download `allure-report-html` artifact
   - Open `index.html` to see failure details, screenshots, and logs

2. **Check Diagnostics:**
   - Download `failure-diagnostics` artifact
   - Review screenshots and page HTML at failure moment

3. **Run in Headless Mode Locally:**
   ```bash
   $env:HEADLESS = "true"
   dotnet test
   ```

4. **Check Environment Differences:**
   - CI runs on Ubuntu; test locally on same OS if possible
   - Use test-matrix job results to identify OS-specific issues

### Workflow Configuration

**File:** `.github/workflows/test-ci.yml`

**Key Environment Variables:**
- `DOTNET_VERSION`: .NET 9.0.x
- `CONFIGURATION`: Debug
- `HEADLESS`: true (in CI)

**Customization:**
- Edit `.github/workflows/test-ci.yml` to modify triggers, matrix OS, or retention policies
- Branches can be added to `on.push.branches` and `on.pull_request.branches`
