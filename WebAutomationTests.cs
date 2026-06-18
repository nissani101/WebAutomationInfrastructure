using Microsoft.Playwright;

namespace WebAutomationInfrastructure;

public class WebAutomationBaseTest
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;
    
    protected IPage Page { get; private set; } = null!;
    protected string Channel { get; }

    public WebAutomationBaseTest(string channel)
    {
        Channel = channel;
    }

    [SetUp]
    public async Task BaseSetup()
    {
        // Initialize Playwright driver
        _playwright = await Playwright.CreateAsync();
        
        // Launch Chrome or Edge based on the channel parameter using Chromium driver base
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Channel = Channel,
            Headless = true // Change to false to see the browser window execution
        });
        
        // Create browser context and page
        _context = await _browser.NewContextAsync();
        Page = await _context.NewPageAsync();
    }

    [TearDown]
    public async Task BaseTearDown()
    {
        // Properly dispose resources to avoid memory leaks or dangling processes
        if (_context != null) await _context.DisposeAsync();
        if (_browser != null) await _browser.DisposeAsync();
        _playwright?.Dispose();
    }
}

[TestFixture("chrome")]
[TestFixture("msedge")]
public class WebAutomationTests : WebAutomationBaseTest
{
    public WebAutomationTests(string channel) : base(channel)
    {
    }

    [Test]
    public async Task PlaywrightHomepage_ShouldLoadSuccessfully()
    {
        // Navigate to the Playwright official website
        await Page.GotoAsync("https://playwright.dev");

        // Verify the title contains "Playwright"
        var title = await Page.TitleAsync();
        Assert.That(title, Does.Contain("Playwright"));

        // Click the "Get started" link
        await Page.ClickAsync("text=Get started");

        // Verify that the URL now contains "intro"
        Assert.That(Page.Url, Does.Contain("intro"));
    }
}
