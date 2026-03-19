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
        // Select by label explicitly to avoid value/text mismatches
        var dropdown = _page.Locator("#dropdown");
        await dropdown.SelectOptionAsync(new SelectOptionValue { Label = text });
    }

    public async Task<string> GetSelectedTextAsync()
    {
        // Get the selected option's text directly from the :checked pseudo-selector
        var selectedOptionText = await _page.Locator("#dropdown option:checked").TextContentAsync();
        return selectedOptionText?.Trim() ?? string.Empty;
    }
}
