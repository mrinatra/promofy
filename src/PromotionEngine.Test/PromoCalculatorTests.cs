using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using Xunit;

namespace PromotionEngine.Test
{
    public class PromoCalculatorTests
    {
        [ExcludeFromCodeCoverage]
        [Theory]
        [InlineData("itemsForScenarioA.json")]
        public void Test_When_AllPromos_Are_Applied_It_Should_Apply_To_All_Eligble_Items_Scenario_A_No_Eligible_Items(string itemsFileName)
        {
            var promotions = GetAllPromotions();

            var cartItems = GetCartItems(itemsFileName);

            var promoEngine = new PromoCalculator();

            dynamic modifiedCart = promoEngine.ApplyPromos(promotions, cartItems);

            Assert.Equal(modifiedCart.TotalPrice, modifiedCart.TotalOffPrice);
        }

        private object GetCartItems(string itemsFileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", itemsFileName);
            var fileData = File.ReadAllText(filePath);
            var items = JsonSerializer.Deserialize<List<Item>>(fileData);

            return items;
        }

        private object GetAllPromotions()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "promotions.json");
            var fileData = File.ReadAllText(filePath);
            var promotions = JsonSerializer.Deserialize<List<Promotion>>(fileData);

            return promotions;
        }
    }
}
