using System;
using System.Collections.Generic;
using System.Linq;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class SeatingPlan
    {
        public int Height { get; set; }

        public int Width { get; set; }

        public Dictionary<decimal, int> PriceMap { get; set; }

        public List<SpBlock> SpBlocks { get; set; }

        public List<SpPerformance> SpPerformances { get; set; }

        public SeatingPlan()
        {
            SpBlocks = new List<SpBlock>();
        }

        public void AdjustCoordinates()
        {
            const int deltaCoefficient = -1;
            foreach (var spBlock in SpBlocks)
            {
                var (x, y) = GetBlockMinCoordinates(spBlock);
                if (x != 0 || y != 0)
                {
                    AdjustCoordinates(spBlock, x * deltaCoefficient, y * deltaCoefficient);
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
                yOffset += GetBlockMaxY(spBlock) + ySpacer;
            }

            Height = yOffset + 10; // Hack to add extra space at end of svg for the stage
            Width = SpBlocks.Max(b => b.Width) + xSpacer;
        }

        public void MatchAvailabilities(List<Ticket> tickets)
        {
            SpPerformances = CreatePerformancesFromTickets(tickets);
            foreach (var spBlock in SpBlocks)
            {
                foreach (var spSeat in spBlock.SpSeats)
                {
                    spSeat.SeatPerformances = new List<SpSeatPerformance>();
                    foreach (var spPerformance in SpPerformances)
                    {
                        var ticket = tickets.FirstOrDefault(t =>
                            t.Date == spPerformance.Date && spSeat.Row == t.Row && spBlock.Ids.Contains(t.BlockId) &&
                            t.FirstAsInt <= spSeat.Number && spSeat.Number <= t.LastAsInt);

                        if (ticket != null)
                        {
                            var spSeatPerformance = CreateSeatPerformance(spPerformance, spSeat, ticket);
                            spSeat.SeatPerformances.Add(spSeatPerformance);
                        }
                    }
                }
            }
            BuildPriceMap(tickets);
        }

        private (int x, int y) GetBlockMinCoordinates(SpBlock spBlock)
        {
            var labelMinX = spBlock.SpRowLabels.Min(l => l.X);
            var labelMinY = spBlock.SpRowLabels.Min(l => l.Y);
            var seatMinX = spBlock.SpSeats.Min(s => s.X);
            var seatMinY = spBlock.SpSeats.Min(s => s.Y);
            var minX = Math.Min(labelMinX, seatMinX);
            var minY = Math.Min(labelMinY, seatMinY);
            return (minX, minY);
        }

        private int GetBlockMaxY(SpBlock spBlock)
        {
            var labelMaxY = spBlock.SpRowLabels.Max(l => l.Y);
            var seatMaxY = spBlock.SpSeats.Max(l => l.Y);
            return Math.Max(labelMaxY, seatMaxY);
        }

        private void AdjustCoordinates(SpBlock spBlock, int deltaX, int deltaY)
        {
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

        private List<SpPerformance> CreatePerformancesFromTickets(IEnumerable<Ticket> tickets)
        {
            return tickets
                .OrderBy(t => t.Date)
                .GroupBy(g => new { g.Date })
                .Select((p, index) => new SpPerformance
                {
                    Id = index + 1,
                    Date = p.Key.Date
                })
                .ToList();
        }

        private SpSeatPerformance CreateSeatPerformance(SpPerformance spPerformance, SpSeat spSeat, Ticket ticket)
        {
            return new SpSeatPerformance
            {
                BlockDescription = ticket.BlockDescription,
                BlockId = ticket.BlockId,
                Date = ticket.Date,
                Discounted = ticket.Discounted,
                PerformanceId = spPerformance.Id,
                Price = ticket.Price,
                SeatKey = ticket.SeatKey,
                RestrictedView = ticket.BlockDescription.ToLower().Contains("restricted"),
                SeatLumps = GetSeatLumps(spSeat.Number, ticket),
            };
        }

        private string GetSeatLumps(int seatNumber, Ticket ticket)
        {
            var patterns = new List<string>
            {
                $"[{seatNumber}]",
                $"[{seatNumber},",
                $",{seatNumber}]",
                $",{seatNumber},",
            };
            var seatLumps = ticket.SeatLumps.Where(sl => patterns.Any(sl.Contains));
            return string.Join(":", seatLumps.ToArray());
        }

        private void BuildPriceMap(IEnumerable<Ticket> tickets)
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
}