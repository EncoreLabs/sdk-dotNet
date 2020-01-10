using EncoreTickets.SDK.Utilities.BaseTypesExtensions;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.BaseTypesExtensions
{
    public class StringExtensionTests
    {
        [TestCase("Circle", "cIrClE")]
        [TestCase("Dress Circle", "Dress cirCle")]
        [TestCase("Gbp", "GBP")]
        public void ToTitleCase_Successful(string expected, string source)
        {
            var result = source.ToTitleCase();

            Assert.AreEqual(expected, result);
        }

        [TestCase("Circle", "cIrClE", false)]
        [TestCase("Dress Circle (Restricted View)", "Dress cirCle", true)]
        public void ToUserFriendlyBlockName_Successful(string expected, string source, bool restrictedView)
        {
            var result = source.ToUserFriendlyBlockName(restrictedView);

            Assert.AreEqual(expected, result);
        }

        [TestCase("test", true)]
        [TestCase("test test", true)]
        [TestCase("  t", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("     ", false)]
        [TestCase("\t", false)]
        [TestCase("\n", false)]
        public void IsWithNotEmptyCharacters_Successful(string source, bool expected)
        {
            var result = source.IsWithNotEmptyCharacters();

            Assert.AreEqual(expected, result);
        }
    }
}
