using System.Collections.Generic;

namespace PromotionEngine.Models
{
    public class Promotion
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public List<Buys> Buy { get; set; }
        public List<Gets> Get { get; set; }
    }

    public class Buys
    {
        public string Sku { get; set; }
        public int Quantity { get; set; }
    }

    public class Gets
    {
        public string Sku { get; set; }
        public int Quantity { get; set; }

        public Off Off { get; set; }
    }

    public class Off
    {
        public float Fixed { get; set; }
    }
}