using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Resources;

namespace Publishing.Core.Tests;

[TestClass]
public class UIResourcesTests
{
    [TestMethod]
    public void AllKeys_HaveUkrainianTranslation()
    {
        var baseRes = new ResourceManager("Publishing.UI.Resources.Resources", typeof(Publishing.Services.SilentUiNotifier).Assembly);
        foreach (var entry in baseRes.GetResourceSet(System.Globalization.CultureInfo.InvariantCulture, true, true)!)
        {
            var key = ((System.Collections.DictionaryEntry)entry).Key.ToString();
            string? translated = baseRes.GetString(key, new System.Globalization.CultureInfo("uk"));
            Assert.IsFalse(string.IsNullOrEmpty(translated), $"Missing translation for {key}");
        }
    }
}
