using EncoreTickets.SDK.EntertainApi.Model;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.EntertainApi.Models
{
    internal class EntertainApiXmlShowTests
    {
        [Test]
        public void EntertainApi_XmlShow_Constructor_InitializesCorrectly()
        {
            var item = new XmlShow();
            Assert.IsNotNull(item.XmlImages);
            Assert.IsNotNull(item.XmlVideos);
            Assert.IsNotNull(item.XmlErrors);
        }
    }
}
