using PromotionEngine.Models;
using System.Collections.Generic;

namespace PromotionEngine
{
    public class MarkedItem
    {
        public Item Item { get; set; }
        public Dictionary<string, bool> MarkedBuys { get; set; }
        public Dictionary<string, float> MarkedGets { get; set; }
    }
}