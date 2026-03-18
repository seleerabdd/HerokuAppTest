using Microsoft.Playwright;

namespace ReqnrollLogin.Tests.Pages;

public class DropdownPage
{
    private readonly IPage _page;

    public DropdownPage(IPage page)
    {
        _page = page;
    }

    public async Task SelectByTextAsync(string text)
    {
        await _page.Locator("#dropdown").SelectOptionAsync(new[] { text });
    }

    public async Task<string> GetSelectedTextAsync()
    {
        var value = await _page.Locator("#dropdown").InputValueAsync();
        var selectedOption = _page.Locator($"#dropdown option[value='{value}']");
        var text = await selectedOption.TextContentAsync();
        return text?.Trim() ?? string.Empty;
    }
}
