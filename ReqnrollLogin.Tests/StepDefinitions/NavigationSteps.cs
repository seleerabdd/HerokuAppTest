using FluentAssertions;
using Reqnroll;
using ReqnrollLogin.Tests.Pages;
using ReqnrollLogin.Tests.Support;

namespace ReqnrollLogin.Tests.Steps;

[Binding]
public class NavigationSteps
{
    private readonly UiTestContext _testContext;

    public NavigationSteps(UiTestContext testContext)
    {
        _testContext = testContext;
    }

    [Given(@"I open the main page at ""(.*)""")]
    public async Task GivenIOpenTheMainPageAt(string url)
    {
        var page = _testContext.Page ?? throw new InvalidOperationException("Page was not initialized. Ensure the scenario uses the @ui tag.");
        var homePage = new HomePage(page);
        await homePage.OpenAsync(url);
    }

    [When(@"I navigate to page link ""(.*)""")]
    public async Task WhenINavigateToPageLink(string linkText)
    {
        var page = _testContext.Page ?? throw new InvalidOperationException("Page was not initialized. Ensure the scenario uses the @ui tag.");
        var homePage = new HomePage(page);
        await homePage.NavigateByLinkTextAsync(linkText);
    }

    [Then(@"I should be on a page with header ""(.*)""")]
    public async Task ThenIShouldBeOnAPageWithHeader(string expectedHeader)
    {
        var page = _testContext.Page ?? throw new InvalidOperationException("Page was not initialized. Ensure the scenario uses the @ui tag.");
        var commonPage = new CommonPage(page);

        var header = await commonPage.GetHeaderAsync();
        header.Should().Be(expectedHeader);
    }

    [Then(@"checkbox (.*) should be unchecked")]
    public async Task ThenCheckboxShouldBeUnchecked(int checkboxNumber)
    {
        var page = _testContext.Page ?? throw new InvalidOperationException("Page was not initialized. Ensure the scenario uses the @ui tag.");
        var checkboxPage = new CheckboxesPage(page);

        var isChecked = await checkboxPage.IsCheckedAsync(checkboxNumber);
        isChecked.Should().BeFalse();
    }

    [Then(@"checkbox (.*) should be checked")]
    public async Task ThenCheckboxShouldBeChecked(int checkboxNumber)
    {
        var page = _testContext.Page ?? throw new InvalidOperationException("Page was not initialized. Ensure the scenario uses the @ui tag.");
        var checkboxPage = new CheckboxesPage(page);

        var isChecked = await checkboxPage.IsCheckedAsync(checkboxNumber);
        isChecked.Should().BeTrue();
    }

    [When(@"I set checkbox (.*) to checked")]
    public async Task WhenISetCheckboxToChecked(int checkboxNumber)
    {
        var page = _testContext.Page ?? throw new InvalidOperationException("Page was not initialized. Ensure the scenario uses the @ui tag.");
        var checkboxPage = new CheckboxesPage(page);

        await checkboxPage.SetCheckedAsync(checkboxNumber, true);
    }

    [When(@"I select ""(.*)"" from dropdown")]
    public async Task WhenISelectFromDropdown(string optionText)
    {
        var page = _testContext.Page ?? throw new InvalidOperationException("Page was not initialized. Ensure the scenario uses the @ui tag.");
        var dropdownPage = new DropdownPage(page);

        await dropdownPage.SelectByTextAsync(optionText);
    }

    [Then(@"selected dropdown option should be ""(.*)""")]
    public async Task ThenSelectedDropdownOptionShouldBe(string expectedText)
    {
        var page = _testContext.Page ?? throw new InvalidOperationException("Page was not initialized. Ensure the scenario uses the @ui tag.");
        var dropdownPage = new DropdownPage(page);

        var selected = await dropdownPage.GetSelectedTextAsync();
        selected.Should().Be(expectedText);
    }

    [When(@"I enter ""(.*)"" into the number input")]
    public async Task WhenIEnterIntoTheNumberInput(string value)
    {
        var page = _testContext.Page ?? throw new InvalidOperationException("Page was not initialized. Ensure the scenario uses the @ui tag.");
        var inputsPage = new InputsPage(page);

        await inputsPage.EnterValueAsync(value);
    }

    [Then(@"the number input value should be ""(.*)""")]
    public async Task ThenTheNumberInputValueShouldBe(string expectedValue)
    {
        var page = _testContext.Page ?? throw new InvalidOperationException("Page was not initialized. Ensure the scenario uses the @ui tag.");
        var inputsPage = new InputsPage(page);

        var value = await inputsPage.GetValueAsync();
        value.Should().Be(expectedValue);
    }
}
