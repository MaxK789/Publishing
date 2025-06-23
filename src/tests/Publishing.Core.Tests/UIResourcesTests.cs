using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Resources;

namespace Publishing.Core.Tests;

#if WINDOWS
[TestClass]
public class UIResourcesTests
{
    [TestMethod]
    public void AllKeys_HaveUkrainianTranslation()
    {
        var baseRes = new ResourceManager(
            "Publishing.Resources.Resources",
            typeof(Publishing.Program).Assembly);
        foreach (var entry in baseRes.GetResourceSet(System.Globalization.CultureInfo.InvariantCulture, true, true)!)
        {
            // Skip entries without a valid string key to avoid nullable warnings.
            if (((System.Collections.DictionaryEntry)entry).Key is not string key)
            {
                continue;
            }

            string? translated = baseRes.GetString(key, new System.Globalization.CultureInfo("uk"));
            Assert.IsFalse(string.IsNullOrEmpty(translated), $"Missing translation for {key}");
        }
    }
}
#endif
