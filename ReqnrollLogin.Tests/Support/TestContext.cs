using Microsoft.Playwright;

namespace ReqnrollLogin.Tests.Support;

public class UiTestContext
{
    public IPage? Page { get; set; }
    public IBrowser? Browser { get; set; }
}
