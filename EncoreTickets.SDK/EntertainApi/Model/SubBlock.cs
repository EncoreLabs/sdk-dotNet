using System.Collections.Generic;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class SubBlock
    {
        public int blockId { get; set; }

        public int subBlockId { get; set; }

        public int areaWidth { get; set; }

        public string shape { get; set; }

        public string entityName { get; set; }

        public int areaLeft { get; set; }

        public int areaTop { get; set; }

        public int areaHeight { get; set; }

        public List<List<string>> seats { get; set; }
    }
}