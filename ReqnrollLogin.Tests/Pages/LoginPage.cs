using Microsoft.Playwright;

namespace ReqnrollLogin.Tests.Pages;

public class LoginPage
{
    private readonly IPage _page;

    public LoginPage(IPage page)
    {
        _page = page;
    }

    public async Task OpenAsync(string loginUrl)
    {
        await _page.GotoAsync(loginUrl);
    }

    public async Task EnterUsernameAsync(string username)
    {
        await _page.FillAsync("#username", username);
    }

    public async Task EnterPasswordAsync(string password)
    {
        await _page.FillAsync("#password", password);
    }

    public async Task ClickLoginButtonAsync()
    {
        await _page.ClickAsync("button[type='submit']");
    }

    public async Task<string> GetFlashMessageAsync()
    {
        var flash = _page.Locator("#flash");
        await flash.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5000 });
        var text = await flash.TextContentAsync();
        return text?.Trim() ?? string.Empty;
    }
}
