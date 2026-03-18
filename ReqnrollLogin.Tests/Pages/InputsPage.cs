using Microsoft.Playwright;

namespace ReqnrollLogin.Tests.Pages;

public class InputsPage
{
    private readonly IPage _page;

    public InputsPage(IPage page)
    {
        _page = page;
    }

    public async Task EnterValueAsync(string value)
    {
        var input = _page.Locator("input[type='number']");
        await input.FillAsync(value);
    }

    public async Task<string> GetValueAsync()
    {
        return await _page.Locator("input[type='number']").InputValueAsync();
    }
}
