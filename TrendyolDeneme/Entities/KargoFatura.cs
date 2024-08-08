using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrendyolDeneme
{
        public partial class KargoFatura
        {
            public long? Page { get; set; }
            public long? Size { get; set; }
            public long? TotalPages { get; set; }
            public long? TotalElements { get; set; }

           [JsonProperty("content")]
           public KargoFaturaDetay [] KargoFaturaDetay { get; set; }
        }

        public partial class KargoFaturaDetay
    {
            public string ShipmentPackageType { get; set; }
            public long? ParcelUniqueId { get; set; }
            public long? OrderNumber { get; set; }
            public double? Amount { get; set; }
            public long? Desi { get; set; }
        }
    }

