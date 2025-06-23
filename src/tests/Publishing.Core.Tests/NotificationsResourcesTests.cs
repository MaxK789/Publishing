using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Resources;

namespace Publishing.Core.Tests;

[TestClass]
public class NotificationsResourcesTests
{
    [TestMethod]
    public void LocalizedResources_HaveAllKeys()
    {
        var baseRes = new ResourceManager("Publishing.Services.Resources.Notifications", typeof(Publishing.Services.SilentUiNotifier).Assembly);
        foreach (var entry in baseRes.GetResourceSet(System.Globalization.CultureInfo.InvariantCulture, true, true)!)
        {
            // DictionaryEntry.Key is of type object and might technically be null
            // but resource keys are expected to be strings. Cast defensively and
            // skip entries without a valid key to satisfy nullable analysis.
            if (((System.Collections.DictionaryEntry)entry).Key is not string key)
            {
                continue;
            }

            AssertKey("uk-UA", key, baseRes);
            AssertKey("en-US", key, baseRes);
        }
    }

    private static void AssertKey(string culture, string key, ResourceManager manager)
    {
        string? translated = manager.GetString(key, new System.Globalization.CultureInfo(culture));
        Assert.IsFalse(string.IsNullOrEmpty(translated), $"Missing translation for {key} in {culture}");
    }
}
