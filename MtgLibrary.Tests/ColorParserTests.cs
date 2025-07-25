using NUnit;
using NUnit.Framework;
using Spiff.MtgTracker.Helpers;
using System.Linq;
using System.Collections.Generic;

namespace Spiff.MtgLibrary.Tests
{
    [TestFixture]
    public class ColorParserTests
    {   
        public static IEnumerable<TestCaseData> ValidCosts ()
        {   
            yield return new TestCaseData("2G", new List<string>() {"Green"});
            yield return new TestCaseData("4", new List<string>(){});
            yield return new TestCaseData("1WU", new List<string>() {"White", "Blue"});
            yield return new TestCaseData("WBR", new List<string>() {"White","Black", "Red"});
            yield return new TestCaseData(null, new List<string>(){});
            yield return new TestCaseData("", new List<string>(){});
            yield return new TestCaseData("{2}{B}{R}", new List<string>() {"Black", "Red"});
        }
            
        [SetUp]
        public void Setup()
        {
        }

        [Test(Description="Tests a valid cost to ensure we get the correct colors")]
        [TestCaseSource(nameof(ValidCosts))]
        public void ValidCost(string input, IEnumerable<string> expected)
        {
            List<string> colors = ColorParser.ParseColorsFromCost(input);
            Assert.That(colors, Is.EquivalentTo(expected), "The colors are not what we expected");
        }

        [Test(Description="")]
        public void InvalidCost()
        {
        }
    }
}
