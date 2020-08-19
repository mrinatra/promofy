using PromotionEngine.Models;
using System;
using System.Collections.Generic;

namespace PromotionEngine
{
    public class PromoCalculator
    {
        public object ApplyPromos(List<Promotion> promotions, List<Item> cartItems)
        {
            List<MarkedItem> markedItems = new List<MarkedItem>();

            foreach (var item in cartItems)
            {
                MarkedItem mItem = new MarkedItem()
                {
                    Item = item,
                    MarkedBuys = new Dictionary<string, bool>(),
                    MarkedGets = new Dictionary<string, float>()
                };
                markedItems.Add(mItem);
            }

            foreach (var promo in promotions)
            {
                markedItems = ApplyPromo(promo, markedItems);
            }

            throw new NotImplementedException();
        }

        private List<MarkedItem> ApplyPromo(Promotion promo, List<MarkedItem> markedItems)
        {
            var appliedBuysItems = applyBuys(promo, markedItems);

            markedItems = appliedBuysItems;
            markedItems = applyGets(promo, markedItems);


            return markedItems;
        }

        private List<MarkedItem> applyGets(Promotion promo, List<MarkedItem> markedItems)
        {
            throw new NotImplementedException();
        }

        private List<MarkedItem> applyBuys(Promotion promo, List<MarkedItem> markedItems)
        {
            throw new NotImplementedException();
        }
    }
}
