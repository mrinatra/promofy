using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
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
            throw new NotImplementedException();
        }

        private object GetAllPromotions()
        {
            throw new NotImplementedException();
        }
    }
}
