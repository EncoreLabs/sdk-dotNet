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
    }
}
