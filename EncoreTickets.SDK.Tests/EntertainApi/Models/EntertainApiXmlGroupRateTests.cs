using EncoreTickets.SDK.EntertainApi.Model;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.EntertainApi.Models
{
    public class EntertainApiXmlGroupRateTests
    {
        [Test]
        public void EntertainApi_XmlGroupRate_Constructor_InitializesCorrectly()
        {
            var item = new XmlGroupRate();
            Assert.IsNotNull(item.XmlGroupRateDays);
            Assert.IsNotNull(item.XmlErrors);
        }
    }
}
