using EncoreTickets.SDK.EntertainApi.Model;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.EntertainApi.Models
{
    internal class EntertainApiJsonSeatingPlanTests
    {
        [Test]
        public void EntertainApi_JsonSeatingPlan_Constructor_InitializesCorrectly()
        {
            var item = new JsonSeatingPlan();
            Assert.IsNotNull(item.XmlErrors);
        }
    }
}
