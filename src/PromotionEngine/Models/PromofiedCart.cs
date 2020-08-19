using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionEngine.Models
{
    public class PromofiedCart
    {
        public List<MarkedItem> items { get; set; }
        public float TotalPrice { get; set; }
        public float TotalOffPrice { get; set; }
    }
}
