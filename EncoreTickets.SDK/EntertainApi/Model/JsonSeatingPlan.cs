using System.Collections.Generic;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class JsonSeatingPlan //: IXmlError
    {
        public List<Block> blocks { get; set; }

        public int hallId { get; set; }

        public List<XmlError> XmlErrors { get; set; }

        public JsonSeatingPlan()
        {
            XmlErrors = new List<XmlError>();
        }
    }
}