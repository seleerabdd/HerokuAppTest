using Microsoft.Playwright;

namespace ReqnrollLogin.Tests.Pages;

public class CommonPage
{
    private readonly IPage _page;

    public CommonPage(IPage page)
    {
        _page = page;
    }

    public async Task<string> GetHeaderAsync()
    {
        var header = _page.Locator("h3");
        await header.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5000 });
        var text = await header.TextContentAsync();
        return text?.Trim() ?? string.Empty;
    }
}
