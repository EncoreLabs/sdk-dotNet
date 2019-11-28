using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Basket;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Basket.Models.RequestModels;
using EncoreTickets.SDK.Inventory.Models;
using Newtonsoft.Json;

namespace EncoreTickets.ConsoleTester
{
    internal class BasketServiceTester
    {
        private const string DefaultBasketId = "5924228";
        private static string basketId;

        public static void TestBasketService(ApiContext context, string testReference)
        {
            var basketService = new BasketServiceApi(context);
            basketId = TestUpsertBasket(basketService, testReference) ?? DefaultBasketId;
            TestGetPromotionDetails(basketService);
            TestGetBasketDetails(basketService);
            TestApplyPromotion(basketService);
            TestDeleteReservation(basketService);
            TestClearBasket(basketService);
        }

        private static string TestUpsertBasket(BasketServiceApi basketService, string testReference)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Basket service upserting basket");
            Console.WriteLine(" ========================================================== ");

            var request = new UpsertBasketRequest
            {
                channelId = "test-channel",
                coupon = new Coupon {code = "PRODUCTDISCOUNT"},
                delivery = new Delivery
                {
                    charge = new Price
                    {
                        currency = "GBP",
                        decimalPlaces = 2,
                        value = 145
                    },
                    method = "postage"
                },
                hasFlexiTickets = false,
                shopperCurrency = "GBP",
                shopperReference = "test",
                reservations = new List<ReservationRequest>
                {
                    new ReservationRequest
                    {
                        date = new DateTimeOffset(2020, 4, 30, 19, 30, 0, TimeSpan.Zero),
                        productId = "1587",
                        venueId = "138",
                        quantity = 1,
                        items = new List<ItemRequest>
                        {
                            new ItemRequest
                            {
                                aggregateReference = testReference
                            }
                        }
                    }
                }
            };

            try
            {
                var basketDetails = basketService.UpsertBasket(request);
                if (basketDetails != null)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(basketDetails, Formatting.Indented));
                }
                return basketDetails?.reference;
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private static void TestGetPromotionDetails(BasketServiceApi basketService)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Basket service getting promotion details ");
            Console.WriteLine(" ========================================================== ");
            
            var promotionDetails = basketService.GetPromotionDetails("206000034");
            if (promotionDetails != null)
            {
                Console.WriteLine($"id: {promotionDetails.id}");
                Console.WriteLine($"name: {promotionDetails.name}");
                Console.WriteLine($"displayText: {promotionDetails.displayText}");
                Console.WriteLine($"description: {promotionDetails.description}");
                Console.WriteLine($"reference: {promotionDetails.reference}");
                Console.WriteLine($"reportingCode: {promotionDetails.reportingCode}");
                Console.WriteLine($"validFrom: {promotionDetails.validFrom}");
                Console.WriteLine($"validTo: {promotionDetails.validTo}");
            }
        }

        private static void TestGetBasketDetails(BasketServiceApi basketService)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Basket service getting basket details ");
            Console.WriteLine(" ========================================================== ");

            var basketDetails = basketService.GetBasketDetails(basketId);
            if (basketDetails != null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(basketDetails, Formatting.Indented));
            }
        }

        private static void TestApplyPromotion(BasketServiceApi basketService)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Basket service applying a coupon to basket ");
            Console.WriteLine(" ========================================================== ");

            var coupon = new Coupon {code = "PRODUCTDISCOUNT"};
            var basketDetails = basketService.UpsertPromotion(basketId, coupon);
            if (basketDetails != null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(basketDetails, Formatting.Indented));
            }
        }

        private static void TestDeleteReservation(BasketServiceApi basketService)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Basket service deleting a reservation in a basket ");
            Console.WriteLine(" ========================================================== ");

            var basketDetails = basketService.RemoveReservation(basketId, 1);
            if (basketDetails != null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(basketDetails, Formatting.Indented));
            }
        }

        private static void TestClearBasket(BasketServiceApi basketService)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Basket service clearing a basket ");
            Console.WriteLine(" ========================================================== ");

            var basketDetails = basketService.ClearBasket(basketId);
            if (basketDetails != null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(basketDetails, Formatting.Indented));
            }
        }
    }
}
