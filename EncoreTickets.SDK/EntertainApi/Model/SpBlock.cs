using System.Collections.Generic;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class SpBlock
    {
        public List<string> Ids { get; set; }

        public string Name { get; set; }

        public List<SpRowLabel> SpRowLabels { get; set; }

        public List<SpSeat> SpSeats { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int XOffset { get; set; }

        public int YOffset { get; set; }
    }
}