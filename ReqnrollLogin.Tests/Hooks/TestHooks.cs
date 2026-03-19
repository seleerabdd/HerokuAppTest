using ReqnrollLogin.Tests.Drivers;
using ReqnrollLogin.Tests.Support;
using Reqnroll;
using Allure.Net.Commons;

namespace ReqnrollLogin.Tests.Hooks;

/// <summary>
/// Handles scenario lifecycle: creates Playwright session before UI scenarios,
/// ensures deterministic disposal after scenarios complete, and captures failure diagnostics.
/// </summary>
[Binding]
public class TestHooks
{
    private readonly UiTestContext _testContext;
    private readonly ScenarioContext _scenarioContext;
    private static readonly string ArtifactsDir = Path.Combine(
        AppContext.BaseDirectory, "failure-diagnostics");

    public TestHooks(UiTestContext testContext, ScenarioContext scenarioContext)
    {
        _testContext = testContext;
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario("ui")]
    public async Task BeforeUiScenario()
    {
        _testContext.Session = await BrowserFactory.CreateSessionAsync();
    }

    [AfterScenario("ui")]
    public async Task AfterUiScenario()
    {
        try
        {
            // Capture failure diagnostics if scenario failed
            if (_scenarioContext.TestError != null && _testContext.Session?.Page != null)
            {
                await CaptureFailureDiagnosticsAsync(_scenarioContext);
            }
        }
        catch (Exception ex)
        {
            // Log diagnostic capture errors but don't fail the test
            System.Diagnostics.Debug.WriteLine($"Error capturing failure diagnostics: {ex}");
        }
        finally
        {
            // Dispose the session, which will clean up all Playwright resources
            // in the correct order: Page → Context → Browser → Playwright
            if (_testContext.Session != null)
            {
                await _testContext.Session.DisposeAsync();
                _testContext.Session = null;
            }
        }
    }

    /// <summary>
    /// Captures failure diagnostics: screenshot, page HTML, and Playwright trace.
    /// Attaches artifacts to Allure report for analysis.
    /// </summary>
    private async Task CaptureFailureDiagnosticsAsync(ScenarioContext scenarioContext)
    {
        var page = _testContext.Session?.Page;
        if (page == null) return;

        EnsureArtifactsDirectory();
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_fff");
        var scenarioName = scenarioContext.ScenarioInfo.Title.Replace(" ", "_");

        try
        {
            // Capture screenshot
            var screenshotPath = Path.Combine(ArtifactsDir, $"{scenarioName}_{timestamp}_screenshot.png");
            await page.ScreenshotAsync(new()
            {
                Path = screenshotPath,
                FullPage = true
            });
            AttachArtifactToAllure(screenshotPath, "Screenshot", "image/png");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to capture screenshot: {ex.Message}");
        }

        try
        {
            // Capture page HTML dump
            var htmlPath = Path.Combine(ArtifactsDir, $"{scenarioName}_{timestamp}_page.html");
            var content = await page.ContentAsync();
            await File.WriteAllTextAsync(htmlPath, content);
            AttachArtifactToAllure(htmlPath, "Page HTML", "text/html");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to capture page HTML: {ex.Message}");
        }

        try
        {
            // Capture page title and URL for context
            var title = await page.TitleAsync();
            var url = page.Url;
            var contextInfo = $"Title: {title}\nURL: {url}\nError: {scenarioContext.TestError?.Message}";
            var contextPath = Path.Combine(ArtifactsDir, $"{scenarioName}_{timestamp}_context.txt");
            await File.WriteAllTextAsync(contextPath, contextInfo);
            AttachArtifactToAllure(contextPath, "Page Context", "text/plain");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to capture page context: {ex.Message}");
        }
    }

    /// <summary>
    /// Attaches an artifact file to the Allure report for test result annotation.
    /// </summary>
    private void AttachArtifactToAllure(string filePath, string name, string mimeType)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                System.Diagnostics.Debug.WriteLine($"Artifact file not found: {filePath}");
                return;
            }

            var fileBytes = File.ReadAllBytes(filePath);
            AllureApi.AddAttachment(name, mimeType, fileBytes);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to attach {name} to Allure: {ex.Message}");
        }
    }

    /// <summary>
    /// Ensures the failure diagnostics directory exists.
    /// </summary>
    private static void EnsureArtifactsDirectory()
    {
        if (!Directory.Exists(ArtifactsDir))
        {
            Directory.CreateDirectory(ArtifactsDir);
        }
    }
}
