using System.Collections.Generic;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class Block
    {
        public int xAvailableText { get; set; }

        public PlanArea PlanArea { get; set; }

        public int yAvailableText { get; set; }

        public int areaTop { get; set; }

        public int areaHeight { get; set; }

        public List<SubBlock> subBlocks { get; set; }

        public string type { get; set; }

        public List<List<string>> rowLabels { get; set; }

        public string blockId { get; set; }

        public string ThebsBlockIds { get; set; }

        public SeatFields seatFields { get; set; }

        public int areaWidth { get; set; }

        public int yTitle { get; set; }

        public string hallId { get; set; }

        public string entityName { get; set; }

        public int xTitle { get; set; }

        public string name { get; set; }

        public int areaLeft { get; set; }
    }
}