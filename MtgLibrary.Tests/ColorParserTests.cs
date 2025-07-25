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
        
        public static IEnumerable<TestCaseData> EffectText()
        {
            yield return new TestCaseData("Return target creature you own to your hand. Flashback {W}", new List<string>() {"White"});
            yield return new TestCaseData("Forestwalk  If one or more tokens would be created under your control, those tokens plus that many 1/1 green Squirrel creature tokens are created instead. {B}, Sacrifice X Squirrels: Target creature gets +X/-X until end of turn.", new List<string>() {"Black"});
            yield return new TestCaseData("{T},{R}{W}{U}", new List<string>() {"White", "Red", "Blue"});
            yield return new TestCaseData("{1}: Permanents your opponents control lose hexproof, indestructable, protection,shroud, and ward until end of turn.", new List<string>() {});
            yield return new TestCaseData("", new List<string>() {});
            yield return new TestCaseData(null, new List<string>() {});
        }

        [Test(Description="Tests the Effect test parsing for colors")]
        [TestCaseSource(nameof(EffectText))]
        public void InvalidCost(string input, IEnumerable<string> expected)
        {
            IEnumerable<string> colors = ColorParser.ParseColorsFromEffect(input);
            Assert.That(colors, Is.EquivalentTo(expected), "The colors that came from the parser didnt match with what was expected");
        }
    }
}
