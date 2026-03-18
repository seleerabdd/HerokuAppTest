using Reqnroll;
using ReqnrollLogin.Tests.Support;
using ReqnrollLogin.Tests.Pages;
using FluentAssertions;

namespace ReqnrollLogin.Tests.StepDefinitions;

[Binding]
public class LoginSteps
{
    private readonly UiTestContext _testContext;
    private string _flashMessage = string.Empty;
    private LoginPage _loginPage = null!;
    private SecureAreaPage _secureAreaPage = null!;
    private CommonPage _commonPage = null!;

    public LoginSteps(UiTestContext testContext)
    {
        _testContext = testContext;
    }

    [Given(@"I open the login page at ""(.*)""")]
    public async Task GivenIOpenTheLoginPageAt(string loginPageUrl)
    {
        var page = _testContext.Page;
        page.Should().NotBeNull("Browser page must be initialized in hooks");
        
        _loginPage = new LoginPage(page!);
        _secureAreaPage = new SecureAreaPage(page!);
        _commonPage = new CommonPage(page!);
        
        await _loginPage.OpenAsync(loginPageUrl);
    }

    [When(@"I log in with username ""(.*)"" and password ""(.*)""")]
    public async Task WhenILogInWithUsernameAndPassword(string username, string password)
    {
        username.Should().NotBeNullOrEmpty("Username must be provided");
        password.Should().NotBeNullOrEmpty("Password must be provided");
        
        await _loginPage.EnterUsernameAsync(username);
        await _loginPage.EnterPasswordAsync(password);
        await _loginPage.ClickLoginButtonAsync();
        
        // Capture flash message after login for assertion in subsequent steps
        _flashMessage = await _loginPage.GetFlashMessageAsync();
        _flashMessage.Should().NotBeNullOrEmpty("Flash message should be displayed after login attempt");
    }

    [Then(@"I should see a success message containing ""(.*)""")]
    public void ThenIShouldSeeASuccessMessageContaining(string expectedMessage)
    {
        expectedMessage.Should().NotBeNullOrEmpty("Expected message must be provided");
        _flashMessage.Should().Contain(expectedMessage, 
            $"Success message should contain '{expectedMessage}'");
    }

    [Then(@"I should see an error message containing ""(.*)""")]
    public void ThenIShouldSeeAnErrorMessageContaining(string expectedMessage)
    {
        expectedMessage.Should().NotBeNullOrEmpty("Expected error message must be provided");
        _flashMessage.Should().Contain(expectedMessage, 
            $"Error message should contain '{expectedMessage}'");
    }

    [Then(@"I should be on the secure area page")]
    public async Task ThenIShouldBeOnTheSecureAreaPage()
    {
        var pageUrl = _secureAreaPage.GetPageUrl();
        pageUrl.Should().Contain("/secure", "Current URL should be secure area page");
        
        var title = await _secureAreaPage.GetTitleAsync();
        title.Should().Be("Secure Area", "Page title should be 'Secure Area'");
    }

    [Then(@"I should see the following secure area data")]
    public async Task ThenIShouldSeeTheFollowingSecureAreaData(Table table)
    {
        table.Should().NotBeNull("Data table must be provided");
        table.Rows.Should().NotBeEmpty("Data table should contain at least one row");

        foreach (var row in table.Rows)
        {
            var field = row["Field"];
            var expectedValue = row["Value"];
            
            string actualValue = field switch
            {
                "Title" => await _secureAreaPage.GetTitleAsync(),
                "Message" => await _secureAreaPage.GetSecureAreaMessageAsync(),
                _ => throw new InvalidOperationException($"Unknown field '{field}'. Supported fields: Title, Message")
            };
            
            actualValue.Should().Contain(expectedValue, 
                $"Field '{field}' should contain '{expectedValue}'");
        }
    }

    [When(@"I log out from the secure area")]
    public async Task WhenILogOutFromTheSecureArea()
    {
        await _secureAreaPage.LogoutAsync();
        
        // Capture flash message after logout for assertion in subsequent steps
        _loginPage = new LoginPage(_testContext.Page!);
        _flashMessage = await _loginPage.GetFlashMessageAsync();
        _flashMessage.Should().NotBeNullOrEmpty("Flash message should be displayed after logout");
    }

    [Then(@"I should be redirected to the login page")]
    public void ThenIShouldBeRedirectedToTheLoginPage()
    {
        var pageUrl = _testContext.Page!.Url;
        pageUrl.Should().Contain("/login", "Should be redirected to login page after logout");
    }
}
