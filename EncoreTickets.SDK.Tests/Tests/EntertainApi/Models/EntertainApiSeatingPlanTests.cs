using System;
using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.EntertainApi.Model;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.EntertainApi.Models
{
    internal class EntertainApiSeatingPlanTests
    {
        [Test]
        public void EntertainApi_SeatingPlan_AdjustCoordinates_IfPositiveCoordinates_IsCorrect()
        {
            var sourceBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 1, Y = 7},
                        new SpRowLabel {X = 4, Y = 3}
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 10, Y = 100},
                        new SpSeat {X = 45, Y = 3000}
                    }
                },
            };
            var resultBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 0, Y = 4},
                        new SpRowLabel {X = 3, Y = 0}
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 9, Y = 97},
                        new SpSeat {X = 44, Y = 2997}
                    }
                },
            };
            EntertainApi_SeatingPlan_AdjustCoordinates_IsCorrect(sourceBlocks, resultBlocks);
        }

        [Test]
        public void EntertainApi_SeatingPlan_AdjustCoordinates_IfNegativeCoordinates_IsCorrect()
        {
            var sourceBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = -1, Y = -7},
                        new SpRowLabel {X = -4, Y = -3}
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = -10, Y = -100},
                        new SpSeat {X = -40, Y = -30}
                    }
                },
            };
            var resultBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 39, Y = 93},
                        new SpRowLabel {X = 36, Y = 97}
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 30, Y = 0},
                        new SpSeat {X = 0, Y = 70}
                    }
                },
            };
            EntertainApi_SeatingPlan_AdjustCoordinates_IsCorrect(sourceBlocks, resultBlocks);
        }

        [Test]
        public void EntertainApi_SeatingPlan_AdjustCoordinates_IfLabelX0_IsCorrect()
        {
            var sourceBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 0, Y = 7},
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 10, Y = 100},
                        new SpSeat {X = 45, Y = 3000}
                    }
                },
            };
            var resultBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 0, Y = 0},
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 10, Y = 93},
                        new SpSeat {X = 45, Y = 2993}
                    }
                },
            };
            EntertainApi_SeatingPlan_AdjustCoordinates_IsCorrect(sourceBlocks, resultBlocks);
        }

        [Test]
        public void EntertainApi_SeatingPlan_AdjustCoordinates_IfLabelY0_IsCorrect()
        {
            var sourceBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 1, Y = 0},
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 10, Y = 100},
                        new SpSeat {X = 45, Y = 3000}
                    }
                },
            };
            var resultBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 0, Y = 0},
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 9, Y = 100},
                        new SpSeat {X = 44, Y = 3000}
                    }
                },
            };
            EntertainApi_SeatingPlan_AdjustCoordinates_IsCorrect(sourceBlocks, resultBlocks);
        }

        [Test]
        public void EntertainApi_SeatingPlan_AdjustCoordinates_IfSeatX0_IsCorrect()
        {
            var sourceBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 1, Y = 7},
                        new SpRowLabel {X = 4, Y = 3}
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 0, Y = 100},
                    }
                },
            };
            var resultBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 1, Y = 4},
                        new SpRowLabel {X = 4, Y = 0}
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 0, Y = 97},
                    }
                },
            };
            EntertainApi_SeatingPlan_AdjustCoordinates_IsCorrect(sourceBlocks, resultBlocks);
        }

        [Test]
        public void EntertainApi_SeatingPlan_AdjustCoordinates_IfSeatY0_IsCorrect()
        {
            var sourceBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 1, Y = 7},
                        new SpRowLabel {X = 4, Y = 3}
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 10, Y = 0},
                    }
                },
            };
            var resultBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 0, Y = 7},
                        new SpRowLabel {X = 3, Y = 3}
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 9, Y = 0},
                    }
                },
            };
            EntertainApi_SeatingPlan_AdjustCoordinates_IsCorrect(sourceBlocks, resultBlocks);
        }

        [Test]
        public void EntertainApi_SeatingPlan_AdjustCoordinates_IfLabelX0AndY0_IsCorrect()
        {
            var sourceBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 1, Y = 0},
                        new SpRowLabel {X = 0, Y = 2},
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 10, Y = 100},
                        new SpSeat {X = 45, Y = 3000}
                    }
                },
            };
            var resultBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 1, Y = 0},
                        new SpRowLabel {X = 0, Y = 2},
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 10, Y = 100},
                        new SpSeat {X = 45, Y = 3000}
                    }
                },
            };
            EntertainApi_SeatingPlan_AdjustCoordinates_IsCorrect(sourceBlocks, resultBlocks);
        }

        [Test]
        public void EntertainApi_SeatingPlan_AdjustCoordinates_IfSeatX0AndY0_IsCorrect()
        {
            var sourceBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 1, Y = 7},
                        new SpRowLabel {X = 4, Y = 3}
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 10, Y = 0},
                        new SpSeat {X = 0, Y = 20},
                    }
                },
            };
            var resultBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 1, Y = 7},
                        new SpRowLabel {X = 4, Y = 3}
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 10, Y = 0},
                        new SpSeat {X = 0, Y = 20},
                    }
                },
            };
            EntertainApi_SeatingPlan_AdjustCoordinates_IsCorrect(sourceBlocks, resultBlocks);
        }

        [Test]
        public void EntertainApi_SeatingPlan_CalculateBlockOffsets_IsCorrect()
        {
            var sourceBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    Width = 10,
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 1, Y = 7},
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 10, Y = 100},
                    }
                },
                new SpBlock
                {
                    Width = 20,
                    SpRowLabels = new List<SpRowLabel>
                    {
                        new SpRowLabel {X = 1, Y = 7},
                    },
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat {X = 10, Y = 1},
                    }
                },
            };
            EntertainApi_SeatingPlan_CalculateBlockOffsets_IsCorrect(sourceBlocks, new[] {50, 225}, 317, 40);
        }

        [Test]
        public void EntertainApi_SeatingPlan_MatchAvailabilities_IfThereAreCertainTickets_IsCorrectAndPerformancesNotEmpty()
        {
            var sourceBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat
                        {
                            Row = "4",
                            Number = 10
                        }
                    },
                    Ids = new List<string>{ "testId" }
                }
            };
            var tickets = new List<Ticket>
            {
                new Ticket
                {
                    Row = "4",
                    First = "9",
                    Last = "11",
                    BlockId = "testId",
                    BlockDescription = "test",
                    Price = 1000,
                    Date = new DateTime(2000, 3, 2),
                    SeatLumps = new List<string>{"[10]"}
                },
                new Ticket
                {
                    Row = "4",
                    First = "10",
                    Last = "10",
                    BlockId = "testId",
                    BlockDescription = "RESTRICTED",
                    Price = 500,
                    Date = new DateTime(1900, 12, 11),
                    SeatLumps = new List<string>{ $"[10," }
                },
                new Ticket
                {
                    Row = "4",
                    First = "10",
                    Last = "10",
                    BlockId = "testId",
                    BlockDescription = "restricted",
                    Price = 500,
                    Date = new DateTime(1900, 11, 11),
                    SeatLumps = new List<string>{ ",10]" }
                },
                new Ticket
                {
                    Row = "4",
                    First = "10",
                    Last = "10",
                    BlockId = "testId",
                    BlockDescription = "RESTRICTED++",
                    Price = 59,
                    Date = new DateTime(2909, 10, 11),
                    SeatLumps = new List<string>{ ",10," }
                },
                new Ticket
                {
                    Row = "4",
                    First = "10",
                    Last = "10",
                    BlockId = "testId",
                    BlockDescription = "",
                    Price = 59,
                    Date = new DateTime(2999, 10, 11),
                    SeatLumps = new List<string>{ "[10,", ",10,"}
                },
                new Ticket
                {
                    Row = "4",
                    First = "10",
                    Last = "10",
                    BlockId = "testId",
                    BlockDescription = "",
                    Price = 59,
                    Date = new DateTime(2009, 10, 11),
                    SeatLumps = new List<string>{ "[10]", ",10,"}
                }
            };
            var resultBlocksToCheckPerformance = new List<SpBlock>
            {
                new SpBlock
                {
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat
                        {
                            SeatPerformances = new List<SpSeatPerformance>
                            {
                                new SpSeatPerformance
                                {
                                    PerformanceId = 1,
                                    BlockDescription = "restricted",
                                    BlockId = "testId",
                                    Date = new DateTime(1900, 11, 11),
                                    Price = 500,
                                    RestrictedView = true,
                                    SeatLumps = ",10]"
                                },
                                new SpSeatPerformance
                                {
                                    PerformanceId = 2,
                                    BlockDescription = "RESTRICTED",
                                    BlockId = "testId",
                                    Date = new DateTime(1900, 12, 11),
                                    Price = 500,
                                    RestrictedView = true,
                                    SeatLumps = "[10,"
                                },
                                new SpSeatPerformance
                                {
                                    PerformanceId = 3,
                                    BlockDescription = "test",
                                    BlockId = "testId",
                                    Date = new DateTime(2000, 3, 2),
                                    Price = 1000,
                                    RestrictedView = false,
                                    SeatLumps = "[10]"
                                },
                                new SpSeatPerformance
                                {
                                    PerformanceId = 4,
                                    BlockDescription = "",
                                    BlockId = "testId",
                                    Date = new DateTime(2009, 10, 11),
                                    Price = 59,
                                    RestrictedView = false,
                                    SeatLumps = "[10]:,10,"
                                },
                                new SpSeatPerformance
                                {
                                    PerformanceId = 5,
                                    BlockDescription = "RESTRICTED++",
                                    BlockId = "testId",
                                    Date = new DateTime(2909, 10, 11),
                                    Price = 59,
                                    RestrictedView = true,
                                    SeatLumps = ",10,"
                                },
                                new SpSeatPerformance
                                {
                                    PerformanceId = 6,
                                    BlockDescription = "",
                                    BlockId = "testId",
                                    Date = new DateTime(2999, 10, 11),
                                    Price = 59,
                                    RestrictedView = false,
                                    SeatLumps = "[10,:,10,"
                                },
                            }
                        }
                    }
                }
            };
            var resultPriceMapping = new Dictionary<int, int>
            {
                {59, 1},
                {500, 2},
                {1000, 3},
            };
            EntertainApi_SeatingPlan_MatchAvailabilities_IsCorrect(sourceBlocks, tickets, resultPriceMapping, resultBlocksToCheckPerformance);
        }

        [Test]
        public void EntertainApi_SeatingPlan_MatchAvailabilities_IfThereAreNotCertainTickets_IsCorrectAndPerformancesNotEmpty()
        {
            var sourceBlocks = new List<SpBlock>
            {
                new SpBlock
                {
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat
                        {
                            Row = "4",
                            Number = 10
                        }
                    },
                    Ids = new List<string>{ "testId" }
                }
            };
            var tickets = new List<Ticket>
            {
                new Ticket //Row 
                {
                    First = "9",
                    Last = "11",
                    BlockId = "testId",
                    BlockDescription = "test",
                    Price = 1000,
                    Date = new DateTime(2000, 3, 2),
                    SeatLumps = new List<string>{"[10]"}
                },
                new Ticket //First
                {
                    Row = "4",
                    First = "10000",
                    Last = "10",
                    BlockId = "testId",
                    BlockDescription = "RESTRICTED",
                    Price = 500,
                    Date = new DateTime(1900, 12, 11),
                    SeatLumps = new List<string>{ "[10," }
                },
                new Ticket //Last
                {
                    Row = "4",
                    First = "10",
                    Last = "1",
                    BlockId = "testId",
                    BlockDescription = "restricted",
                    Price = 500,
                    Date = new DateTime(1900, 11, 11),
                    SeatLumps = new List<string>{ ",10]" }
                },
                new Ticket //BlockId
                {
                    Row = "4",
                    First = "10",
                    Last = "10",
                    BlockDescription = "RESTRICTED++",
                    Price = 59,
                    Date = new DateTime(2909, 10, 11),
                    SeatLumps = new List<string>{ ",10," }
                },
            };
            var resultBlocksToCheckPerformance = new List<SpBlock>
            {
                new SpBlock
                {
                    SpSeats = new List<SpSeat>
                    {
                        new SpSeat
                        {
                            SeatPerformances = new List<SpSeatPerformance>()
                        }
                    }
                }
            };
            var resultPriceMapping = new Dictionary<int, int>
            {
                {59, 1},
                {500, 2},
                {1000, 3},
            };
            EntertainApi_SeatingPlan_MatchAvailabilities_IsCorrect(sourceBlocks, tickets, resultPriceMapping, resultBlocksToCheckPerformance);
        }

        private void EntertainApi_SeatingPlan_AdjustCoordinates_IsCorrect(List<SpBlock> sourceBlocks, List<SpBlock> resultBlocks)
        {
            var item = new SeatingPlan
            {
                SpBlocks = sourceBlocks
            };
            Assert.DoesNotThrow(item.AdjustCoordinates);
            for (var i = 0; i < item.SpBlocks.Count; i++)
            {
                for (var j = 0; j < item.SpBlocks[i].SpRowLabels.Count; j++)
                {
                    Assert.AreEqual(resultBlocks[i].SpRowLabels[j].X, item.SpBlocks[i].SpRowLabels[j].X);
                    Assert.AreEqual(resultBlocks[i].SpRowLabels[j].Y, item.SpBlocks[i].SpRowLabels[j].Y);
                }
                for (var j = 0; j < item.SpBlocks[i].SpSeats.Count; j++)
                {
                    Assert.AreEqual(resultBlocks[i].SpSeats[j].X, item.SpBlocks[i].SpSeats[j].X);
                    Assert.AreEqual(resultBlocks[i].SpSeats[j].Y, item.SpBlocks[i].SpSeats[j].Y);
                }
            }
        }

        private void EntertainApi_SeatingPlan_CalculateBlockOffsets_IsCorrect(List<SpBlock> sourceBlocks,
            int[] resultYOffsets, int resultHeight, int resultWidth)
        {
            var item = new SeatingPlan
            {
                SpBlocks = sourceBlocks
            };
            Assert.DoesNotThrow(item.CalculateBlockOffsets);
            Assert.AreEqual(resultHeight, item.Height);
            Assert.AreEqual(resultWidth, item.Width);
            for (var i = 0; i < item.SpBlocks.Count; i++)
            {
                Assert.AreEqual(resultYOffsets[i], item.SpBlocks[i].YOffset);
            }
        }

        private void EntertainApi_SeatingPlan_MatchAvailabilities_IsCorrect(List<SpBlock> sourceBlocks,
            List<Ticket> tickets, Dictionary<int, int> resultPriceMapping, List<SpBlock> resultBlocksToCheckPerformance)
        {
            var item = new SeatingPlan
            {
                SpBlocks = sourceBlocks
            };
            Assert.DoesNotThrow(() => item.MatchAvailabilities(tickets));

            var priceMappingKeys = item.PriceMap.Keys.ToList();
            var priceMappingValues = item.PriceMap.Values.ToList();
            var resultPriceMappingKeys = resultPriceMapping.Keys.ToList();
            var resultPriceMappingValues = resultPriceMapping.Values.ToList();
            for (var i = 0; i < resultPriceMapping.Count; i++)
            {
                Assert.AreEqual(resultPriceMappingKeys[i], priceMappingKeys[i]);
                Assert.AreEqual(resultPriceMappingValues[i], priceMappingValues[i]);
            }

            if (resultBlocksToCheckPerformance == null)
            {
                return;
            }

            for (var i = 0; i < item.SpBlocks.Count; i++)
            {
                for (var j = 0; j < item.SpBlocks[i].SpSeats.Count; j++)
                {
                    for (var k = 0; k < item.SpBlocks[i].SpSeats[j].SeatPerformances.Count; k++)
                    {
                        var resultPerformance = resultBlocksToCheckPerformance[i].SpSeats[j].SeatPerformances[k];
                        var itemPerformance = item.SpBlocks[i].SpSeats[j].SeatPerformances[k];
                        AssertExtension.PropertyValuesAreEquals(resultPerformance, itemPerformance);
                    }
                }
            }
        }
    }
}
