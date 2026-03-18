using Microsoft.Playwright;

namespace ReqnrollLogin.Tests.Pages;

public class CheckboxesPage
{
    private readonly IPage _page;

    public CheckboxesPage(IPage page)
    {
        _page = page;
    }

    private ILocator GetCheckbox(int index)
    {
        return _page.Locator($"input[type='checkbox']:nth-of-type({index})");
    }

    public async Task<bool> IsCheckedAsync(int index)
    {
        return await GetCheckbox(index).IsCheckedAsync();
    }

    public async Task SetCheckedAsync(int index, bool shouldBeChecked)
    {
        var checkbox = GetCheckbox(index);
        var isChecked = await checkbox.IsCheckedAsync();
        if (isChecked != shouldBeChecked)
        {
            await checkbox.ClickAsync();
        }
    }
}
