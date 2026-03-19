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
        // Scope to the checkboxes container to avoid global nth-of-type brittleness
        // Find all checkboxes within the container and get by index (1-based to 0-based)
        return _page.Locator("#checkboxes input[type='checkbox']").Nth(index - 1);
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
