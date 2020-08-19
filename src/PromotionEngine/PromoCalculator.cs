using PromotionEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var appliedBuysItems = ApplyBuys(promo, markedItems);

            if (!appliedBuysItems.applied)
                return markedItems;

            markedItems = appliedBuysItems.items;

            markedItems = ApplyGets(promo, markedItems);

            return markedItems;
        }

        private (bool applied, List<MarkedItem> items) ApplyBuys(Promotion promo, List<MarkedItem> markedItems)
        {
            bool applied = true;

            foreach (var buy in promo.Buy)
            {
                var appliedBuy = applyBuy(buy, promo, markedItems);
                applied = applied && appliedBuy.applied;
            }
            return (applied, markedItems);
        }

        private (bool applied,List<MarkedItem> items) applyBuy(Buys buy, Promotion promo, List<MarkedItem> markedItems)
        {
            List<MarkedItem> matchedItems = new List<MarkedItem>();

            var skuList = markedItems.Where(i => i.Item.Sku == buy.Sku).Select(i => i).ToList();

            if (skuList == null || skuList.Count == 0)
            {
                return (false, markedItems);
            }

            foreach (var sku in markedItems)
            {
                if (sku.Item.Sku == buy.Sku)
                {
                    if (sku.Item.Quantity < buy.Quantity)
                    {
                        return (false, markedItems);
                    }
                    sku.MarkedBuys[promo.Id] = true;
                }
            }
            return (true, skuList);
        }

        private List<MarkedItem> ApplyGets(Promotion promo, List<MarkedItem> markedItems)
        {
            throw new NotImplementedException();
        }
    }
}
