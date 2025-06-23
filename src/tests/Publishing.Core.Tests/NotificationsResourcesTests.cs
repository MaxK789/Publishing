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
            var key = ((System.Collections.DictionaryEntry)entry).Key!.ToString();
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
