using System;
using System.Collections.Generic;
using System.Linq;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class SeatingPlan
    {
        public SeatingPlan()
        {
            SpBlocks = new List<SpBlock>();
        }

        public int Height { get; set; }
        public int Width { get; set; }
        public Dictionary<decimal, int> PriceMap { get; set; }
        public List<SpBlock> SpBlocks { get; set; }
        public List<SpPerformance> SpPerformances { get; set; }

        public void AdjustCoordinates()
        {
            foreach (var spBlock in SpBlocks)
            {
                var minX = Math.Min(spBlock.SpRowLabels.Min(l => l.X), spBlock.SpSeats.Min(s => s.X));
                var minY = Math.Min(spBlock.SpRowLabels.Min(l => l.Y), spBlock.SpSeats.Min(s => s.Y));

                if (minX != 0 || minY != 0)
                {
                    var deltaX = minX * -1;
                    var deltaY = minY * -1;

                    foreach (var spRowLabel in spBlock.SpRowLabels)
                    {
                        spRowLabel.X += deltaX;
                        spRowLabel.Y += deltaY;
                    }

                    foreach (var spSeat in spBlock.SpSeats)
                    {
                        spSeat.X += deltaX;
                        spSeat.Y += deltaY;
                    }
                }
            }
        }

        public void CalculateBlockOffsets()
        {
            const int xSpacer = 20;
            const int ySpacer = 75;
            var yOffset = 50; // Hack to add space to beginning of svg

            foreach (var spBlock in SpBlocks)
            {
                spBlock.YOffset = yOffset;
                yOffset += Math.Max(spBlock.SpRowLabels.Max(l => l.Y), spBlock.SpSeats.Max(s => s.Y)) + ySpacer;
            }

            Height = yOffset + 10; // Hack to add extra space at end of svg for the stage
            Width = SpBlocks.Max(b => b.Width) + xSpacer;
        }

        public void MatchAvailabilities(List<Ticket> tickets)
        {
            // Group performances
            SpPerformances = tickets
                .OrderBy(t => t.Date)
                .GroupBy(g => new { g.Date })
                .Select((p, index) => new SpPerformance { Id = index + 1, Date = p.Key.Date })
                .ToList();

            foreach (var spBlock in SpBlocks)
            {
                foreach (var spSeat in spBlock.SpSeats)
                {
                    spSeat.SeatPerformances = new List<SpSeatPerformance>();

                    foreach (var spPerformance in SpPerformances)
                    {
                        var ticket = tickets
                            .FirstOrDefault(t => t.Date == spPerformance.Date && spSeat.Row == t.Row && t.FirstAsInt <= spSeat.Number && spSeat.Number <= t.LastAsInt && spBlock.Ids.Contains(t.BlockId));

                        if (ticket == null) continue;

                        var spSeatPerformance = new SpSeatPerformance
                        {
                            BlockDescription = ticket.BlockDescription,
                            BlockId = ticket.BlockId,
                            Date = ticket.Date,
                            Discounted = ticket.Discounted,
                            PerformanceId = spPerformance.Id,
                            Price = ticket.Price,
                            SeatKey = ticket.SeatKey,
                            RestrictedView = ticket.BlockDescription.ToLower().Contains("restricted"),
                        };

                        var pattern1 = string.Format("[{0}]", spSeat.Number);
                        var pattern2 = string.Format("[{0},", spSeat.Number);
                        var pattern3 = string.Format(",{0}]", spSeat.Number);
                        var pattern4 = string.Format(",{0},", spSeat.Number);
                        var seatLumps = ticket.SeatLumps.Where(sl => sl.Contains(pattern1) || sl.Contains(pattern2) || sl.Contains(pattern3) || sl.Contains(pattern4)).ToArray();

                        spSeatPerformance.SeatLumps = string.Join(":", seatLumps);
                        spSeat.SeatPerformances.Add(spSeatPerformance);
                    }
                }
            }

            BuildPriceMap(tickets);
        }

        private void BuildPriceMap(List<Ticket> tickets)
        {
            var index = 1;
            var priceMap = tickets
                .GroupBy(t => t.Price)
                .Select(g => g.Key)
                .OrderBy(k => k)
                .ToDictionary(k => k, k => index++);

            PriceMap = priceMap;
        }
    }

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

    public class SpRowLabel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Label { get; set; }
    }

    public class SpSeat
    {
        public int Number { get; set; }
        public string Row { get; set; }
        public List<SpSeatPerformance> SeatPerformances { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class SpPerformance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }

    public class SpSeatPerformance
    {
        public string BlockDescription { get; set; }
        public string BlockId { get; set; }
        public DateTime Date { get; set; }
        public bool Discounted { get; set; }
        public int PerformanceId { get; set; }
        public decimal Price { get; set; }
        public bool RestrictedView { get; set; }
        public string SeatKey { get; set; }
        public string SeatLumps { get; set; }
    }
}