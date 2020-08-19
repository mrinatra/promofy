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

            var prices = ComputeTotalPrices(markedItems);

            PromofiedCart modifiedCart = new PromofiedCart()
            {
                items = markedItems,
                TotalPrice = prices.Item1,
                TotalOffPrice = prices.Item2               
            };

            return modifiedCart;
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

        #region Check if buy criteria is satisfied
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
        #endregion

        private List<MarkedItem> ApplyGets(Promotion promo, List<MarkedItem> markedItems)
        {
            foreach (var get in promo.Get)
            {
                markedItems = ApplyGet(get, markedItems, promo);
            }
            return markedItems;
        }

        private List<MarkedItem> ApplyGet(Gets get, List<MarkedItem> markedItems, Promotion promo)
        {
            foreach (var m in markedItems)
            {
                if (m.Item.Sku == get.Sku)
                {
                    var isMarkedBuy = m.MarkedBuys.ContainsKey(promo.Id);
                    var isMarkedGet = m.MarkedGets.ContainsKey(promo.Id);

                    if (isMarkedBuy && !isMarkedGet)
                    {
                        m.MarkedGets[promo.Id] = ComputeOffPrice(m.Item, get);
                    }
                }
            }
            return markedItems;
        }

        private float ComputeOffPrice(Item item, Gets get)
        {
            if(get.Off.Discount != 0)
            {
                return (item.Price * item.Quantity) * (1 - get.Off.Discount / 100);
            }
            else
            {
                float offPrice = get.Off.Fixed;
                int leftCount = item.Quantity - get.Quantity;
                offPrice = GetPrice(offPrice, item, get, leftCount);

                return offPrice;
            }
            
        }

        private float GetPrice(float offPrice, Item item, Gets get, int leftCount)
        {

            if (leftCount < get.Quantity)
            {
                offPrice += leftCount * item.Price;
            }
            else
            {
                offPrice += get.Off.Fixed;
                leftCount -= get.Quantity;
                offPrice = GetPrice(offPrice, item, get, leftCount);
            }
            return offPrice;
        }

        private (float, float) ComputeTotalPrices(List<MarkedItem> markedItems)
        {
            float totalPrice = 0;
            float totalOffPrice = 0;
            foreach (var item in markedItems)
            {
                totalPrice = totalPrice + item.Item.Price * item.Item.Quantity;

                totalOffPrice = totalOffPrice + GetOffPrice(item.Item.Price * item.Item.Quantity, item);
            }
            return (totalPrice, totalOffPrice);
        }

        private float GetOffPrice(float originalPrice, MarkedItem item)
        {
            if (item.MarkedGets.Count == 0)
            {
                return originalPrice;

            }

            float offPrice = 0;

            foreach (var get in item.MarkedGets)
            {
                offPrice += get.Value;
            }
            return offPrice;
        }
    }
}
