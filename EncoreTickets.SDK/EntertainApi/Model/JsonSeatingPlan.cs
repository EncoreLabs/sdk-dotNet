using System.Collections.Generic;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class JsonSeatingPlan  //: IXmlError
    {
        public List<Block> blocks { get; set; }
        public int hallId { get; set; }
        public List<XmlError> XmlErrors { get; set; }

        public JsonSeatingPlan()
        {
            XmlErrors = new List<XmlError>();
        }
    }
    
    public class PlanArea
    {
        public string top { get; set; }
        public string left { get; set; }
        public string width { get; set; }
        public string height { get; set; }
    }

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

    public class SeatFields
    {
        public string seat { get; set; }
        public int xPosition { get; set; }
        public int subBlockId { get; set; }
        public bool restrictedView { get; set; }
        public int yPosition { get; set; }
        public int seatId { get; set; }
        public string seatName { get; set; }
        public string row { get; set; }
    }

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