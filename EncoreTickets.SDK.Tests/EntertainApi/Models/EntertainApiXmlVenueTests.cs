using EncoreTickets.SDK.EntertainApi.Model;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.EntertainApi.Models
{
    public class EntertainApiXmlVenueTests
    {
        [Test]
        public void EntertainApi_XmlVenue_Constructor_InitializesCorrectly()
        {
            var item = new XmlVenue();
            Assert.IsNotNull(item.XmlFacilities);
            Assert.IsNotNull(item.XmlImages);
            Assert.IsNotNull(item.XmlErrors);
        }
    }
}
