using Microsoft.Playwright;

namespace WebAutomationInfrastructure.Actions;

/// <summary>
/// Encapsulates all common web element actions provided by the browser.
/// Each method accepts an ILocator (representing the web element) and safely returns
/// a boolean (or a tuple containing a boolean) to indicate the success or failure of the action.
/// </summary>
public class WebElementActions
{
    /// <summary>
    /// Safely clicks on a web element.
    /// </summary>
    public async Task<bool> ClickAsync(ILocator locator, int timeoutMs = 5000)
    {
        try
        {
            await locator.ClickAsync(new LocatorClickOptions { Timeout = timeoutMs });
            return true;
        }
        catch (Exception ex) when (ex is TimeoutException || ex is PlaywrightException)
        {
            Console.WriteLine($"[ClickAsync] Failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Safely sets (fills) text on an input element.
    /// </summary>
    public async Task<bool> SetTextAsync(ILocator locator, string text, int timeoutMs = 5000)
    {
        try
        {
            await locator.FillAsync(text, new LocatorFillOptions { Timeout = timeoutMs });
            return true;
        }
        catch (Exception ex) when (ex is TimeoutException || ex is PlaywrightException)
        {
            Console.WriteLine($"[SetTextAsync] Failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Safely retrieves text from a web element.
    /// Returns a tuple indicating success and the resulting text (if any).
    /// </summary>
    public async Task<(bool IsSuccess, string? Text)> GetTextAsync(ILocator locator, int timeoutMs = 5000)
    {
        try
        {
            var text = await locator.InnerTextAsync(new LocatorInnerTextOptions { Timeout = timeoutMs });
            return (true, text);
        }
        catch (Exception ex) when (ex is TimeoutException || ex is PlaywrightException)
        {
            Console.WriteLine($"[GetTextAsync] Failed: {ex.Message}");
            return (false, null);
        }
    }

    /// <summary>
    /// Safely hovers over a web element.
    /// </summary>
    public async Task<bool> HoverAsync(ILocator locator, int timeoutMs = 5000)
    {
        try
        {
            await locator.HoverAsync(new LocatorHoverOptions { Timeout = timeoutMs });
            return true;
        }
        catch (Exception ex) when (ex is TimeoutException || ex is PlaywrightException)
        {
            Console.WriteLine($"[HoverAsync] Failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Checks a checkbox or radio button.
    /// </summary>
    public async Task<bool> CheckAsync(ILocator locator, int timeoutMs = 5000)
    {
        try
        {
            await locator.CheckAsync(new LocatorCheckOptions { Timeout = timeoutMs });
            return true;
        }
        catch (Exception ex) when (ex is TimeoutException || ex is PlaywrightException)
        {
            Console.WriteLine($"[CheckAsync] Failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Selects an option in a dropdown by its value or label.
    /// </summary>
    public async Task<bool> SelectOptionAsync(ILocator locator, string optionValue, int timeoutMs = 5000)
    {
        try
        {
            await locator.SelectOptionAsync(new[] { optionValue }, new LocatorSelectOptionOptions { Timeout = timeoutMs });
            return true;
        }
        catch (Exception ex) when (ex is TimeoutException || ex is PlaywrightException)
        {
            Console.WriteLine($"[SelectOptionAsync] Failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Waits for an element to be visible and returns true if it becomes visible within the timeout.
    /// </summary>
    public async Task<bool> IsVisibleAsync(ILocator locator, int timeoutMs = 5000)
    {
        try
        {
            await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = timeoutMs });
            return true;
        }
        catch (Exception ex) when (ex is TimeoutException || ex is PlaywrightException)
        {
            Console.WriteLine($"[IsVisibleAsync] Failed or Not Visible: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Checks if the element is currently enabled for interaction.
    /// </summary>
    public async Task<bool> IsEnabledAsync(ILocator locator, int timeoutMs = 5000)
    {
        try
        {
            // Playwright's IsEnabledAsync is an immediate check and doesn't wait,
            // but we can ensure it is attached and visible first.
            await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Attached, Timeout = timeoutMs });
            return await locator.IsEnabledAsync();
        }
        catch (Exception ex) when (ex is TimeoutException || ex is PlaywrightException)
        {
            Console.WriteLine($"[IsEnabledAsync] Failed: {ex.Message}");
            return false;
        }
    }
}
