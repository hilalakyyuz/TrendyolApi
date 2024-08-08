using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrendyolDeneme
{

    public partial class Product
    {
        public string Barcode { get; set; }
        public string SellerBarcode { get; set; }
        public double? RrpPrice { get; set; }
        public double? BuyingPrice { get; set; }
        public long? Stock { get; set; }
        public string Origin { get; set; }
        public string Composition { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string Gtip { get; set; }
        public SellingPrice[] SellingPrices { get; set; }
    }

    public partial class SellingPrice
    {
        public string Type { get; set; }
        public string Currency { get; set; }
        public double? Price { get; set; }
    }

}
