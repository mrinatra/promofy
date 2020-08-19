using Castle.Core.Internal;
using Moq;
using PromotionEngine.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using Xunit;

namespace PromotionEngine.Test
{
    [ExcludeFromCodeCoverage]
    public class PromoCalculatorTests
    {      
        [Theory]
        [InlineData("itemsForScenarioA.json")]
        public void Test_When_AllPromos_Are_Applied_It_Should_Apply_To_All_Eligble_Items_Scenario_A_No_Eligible_Items(string itemsFileName)
        {
            var promotions = GetAllPromotions();

            var cartItems = GetCartItems(itemsFileName);

            var promoEngine = new PromoCalculator();

            dynamic modifiedCart = promoEngine.ApplyPromos(promotions, cartItems);

            Assert.Equal(modifiedCart.TotalPrice, modifiedCart.TotalOffPrice);
            Assert.Equal(100, modifiedCart.TotalPrice);
            Assert.Equal(100, modifiedCart.TotalOffPrice);
        }

        [Theory]
        [InlineData("itemsForScenarioB.json")]
        public void Test_When_AllPromos_Are_Applied_It_Should_Apply_To_All_Eligble_Items_Scenario_B_Two_Eligible_Items(string itemsFileName)
        {
            var promotions = GetAllPromotions();

            var cartItems = GetCartItems(itemsFileName);

            var promoEngine = new PromoCalculator();

            dynamic modifiedCart = promoEngine.ApplyPromos(promotions, cartItems);

            Assert.Equal(420, modifiedCart.TotalPrice);
            Assert.Equal(370, modifiedCart.TotalOffPrice);
        }

        [Theory]
        [InlineData("itemsForScenarioC.json")]
        public void Test_When_AllPromos_Are_Applied_It_Should_Apply_To_All_Eligble_Items_Scenario_C_Three_Eligible_Items(string itemsFileName)
        {
            var promotions = GetAllPromotions();

            var cartItems = GetCartItems(itemsFileName);

            var promoEngine = new PromoCalculator();

            dynamic modifiedCart = promoEngine.ApplyPromos(promotions, cartItems);

            Assert.Equal(335, modifiedCart.TotalPrice);
            Assert.Equal(280, modifiedCart.TotalOffPrice);
        }

        [Theory]
        [InlineData("itemsForScenarioD.json")]
        public void Test_When_AllPromos_Are_Applied_It_Should_Apply_To_All_Eligble_Items_Scenario_D_Three_Eligible_Items(string itemsFileName)
        {
            var promotions = GetAllPromotions();

            var cartItems = GetCartItems(itemsFileName);

            var promoEngine = new PromoCalculator();

            dynamic modifiedCart = promoEngine.ApplyPromos(promotions, cartItems);

            Assert.Equal(100, modifiedCart.TotalPrice);
            Assert.Equal(70, modifiedCart.TotalOffPrice);
        }

        [Fact]
        public void Test_Get_Active_Promotions_Only()
        {
            var totalPromotions = GetAllPromotions();
           
            var promoEngine = new PromoCalculator();

            List<Promotion> activePromotions = promoEngine.GetActivePromotions(totalPromotions);

            Assert.NotNull(activePromotions);
            Assert.Equal(4, activePromotions.Count);
            Assert.Equal(5, totalPromotions.Count);
        }

        private List<Item> GetCartItems(string itemsFileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", itemsFileName);
            var fileData = File.ReadAllText(filePath);
            var items = JsonSerializer.Deserialize<List<Item>>(fileData);

            return items;
        }

        private List<Promotion> GetAllPromotions()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "promotions.json");
            var fileData = File.ReadAllText(filePath);
            var promotions = JsonSerializer.Deserialize<List<Promotion>>(fileData);

            return promotions;
        }
    }
}
