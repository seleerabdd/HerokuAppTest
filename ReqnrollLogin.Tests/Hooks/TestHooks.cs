using ReqnrollLogin.Tests.Drivers;
using ReqnrollLogin.Tests.Support;
using Reqnroll;

namespace ReqnrollLogin.Tests.Hooks;

[Binding]
public class TestHooks
{
    private readonly UiTestContext _testContext;

    public TestHooks(UiTestContext testContext)
    {
        _testContext = testContext;
    }

    [BeforeScenario("ui")]
    public async Task BeforeUiScenario()
    {
        _testContext.Page = await BrowserFactory.CreatePageAsync();
    }

    [AfterScenario("ui")]
    public async Task AfterUiScenario()
    {
        if (_testContext.Page != null)
        {
            await _testContext.Page.CloseAsync();
            await _testContext.Page.Context.Browser!.CloseAsync();
        }
    }
}
