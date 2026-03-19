# Medefer QA Technical Test - Interview Exercise

## Overview

This is a hands-on interview exercise with three phases:

1. **Phase 0: Build Foundation** - Fix compilation errors

""@MacBookPro MedeferQATechTest-Assessment % dotnet build
Restore complete (1.0s)
  MedeferQATechTest net9.0 failed with 2 error(s) (3.4s)
    /Users/""/Downloads/MedeferQATechTest-Assessment/ReqnrollLogin.Tests/Pages/SecureAreaPage.cs(25,23): error CS1519: Invalid token 'string' in a member declaration
    /Users/""/Downloads/MedeferQATechTest-Assessment/ReqnrollLogin.Tests/Pages/SecureAreaPage.cs(25,29): error CS1519: Invalid token '>' in a member declaration

Build failed with 2 error(s) in 4.8s


Fixed the error on line 25 in SecureAreaPage.cs.  ---  public async Task string>.   to public async Task<string>

""@MacBookPro MedeferQATechTest-Assessment % dotnet build
Restore complete (0.9s)
  MedeferQATechTest net9.0 succeeded (4.6s) → ReqnrollLogin.Tests/bin/Debug/net9.0/MedeferQATechTest.dll

Build succeeded in 6.0s

2. **Phase 1: Test Authoring** - Write login tests and implement step definitions

Have written the login feature and implmented the step definitions.


3. **Phase 2: Debugging** - Find and fix framework bugs to make tests pass

 Given I open the main page at "https://the-internet.herokuapp.com"
       -> error: Page was not initialized. Ensure the scenario uses the @ui tag. (0.0s)
       When I navigate to page link "Inputs"
       -> skipped because of previous errors
       Then I should be on a page with header "Inputs"
       -> skipped because of previous errors
       When I enter "2026" into the number input
       -> skipped because of previous errors
       Then the number input value should be "2026"
       -> skipped because of previous errors

      Line 20 in TestHooks.cs was commented -- _testContext.Page = await BrowserFactory.CreatePageAsync();  so Uncommented the code.

Have added missing methods to the LoginPage. --  EnterUsernameAsync , EnterPasswordAsync, ClickLoginButtonAsync, GetFlashMessageAsync

Test summary: total: 5, failed: 1, succeeded: 4, skipped: 0, duration: 76.9s
Build failed with 1 error(s) in 79.5s

 Given I open the main page at "https://the-internet.herokuapp.com"
       -> done: NavigationSteps.GivenIOpenTheMainPageAt("https://the-inter...") (1.7s)
       When I navigate to page link "Dropdown"
       -> done: NavigationSteps.WhenINavigateToPageLink("Dropdown") (1.2s)
       Then I should be on a page with header "Dropdown List"
       -> done: NavigationSteps.ThenIShouldBeOnAPageWithHeader("Dropdown List") (0.1s)
       When I select "Option 2" from dropdown
       -> error: Timeout 30000ms exceeded.

Issue with the dropdown elemement .. updated the dropdown element in dropdown page

 public async Task SelectByTextAsync(string text)
    {
        await _page.Locator("#dropdownlist").SelectOptionAsync(new[] { text });
    }

 public async Task SelectByTextAsync(string text)
    {
        await _page.Locator("#dropdown").SelectOptionAsync(new[] { text });
    }

Same with the  public async Task<string> GetSelectedTextAsync()




 // Normalize: remove close button marker (×) and collapse excess whitespace
        if (string.IsNullOrEmpty(text))
            return string.Empty;
        
        // Remove the close button symbol and normalize whitespace
        var normalized = System.Text.RegularExpressions.Regex.Replace(text, @"[×✕]", "").Trim();
        // Collapse multiple spaces into single space
        normalized = System.Text.RegularExpressions.Regex.Replace(normalized, @"\s+", " ");
        return normalized;

