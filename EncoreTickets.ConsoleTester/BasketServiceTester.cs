using System;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Basket;
using EncoreTickets.SDK.Basket.Models;
using Newtonsoft.Json;

namespace EncoreTickets.ConsoleTester
{
    internal class BasketServiceTester
    {
        private const string BasketId = "5924228";

        public static void TestBasketService(ApiContext context)
        {
            var basketService = new BasketServiceApi(context);
            TestGetPromotionDetails(basketService);
            TestGetBasketDetails(basketService);
            TestApplyPromotion(basketService);
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

            var basketDetails = basketService.GetBasketDetails("5924228");
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
            var basketDetails = basketService.UpsertPromotion(BasketId, coupon);
            if (basketDetails != null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(basketDetails, Formatting.Indented));
            }
        }
    }
}
