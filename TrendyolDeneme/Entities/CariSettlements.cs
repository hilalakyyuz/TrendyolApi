using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrendyolDeneme
{
    
     public class CariSettlements
   {

     public long Page { get; set; }   
     public long Size { get; set; }
     public long TotalPages { get; set; }
     public long TotalElements { get; set; }
     [JsonProperty("content")]
     public ContentCari[] ContentCari { get; set; }

   }

     public class ContentCari
 {
    public string Id { get; set; }
    public long? TransactionDate { get; set; }
    public string Barcode { get; set; }
    public string TransactionType { get; set; }
    public long? ReceiptId { get; set; }
    public string Description { get; set; }
    public double? Debt { get; set; }
    public double? Credit { get; set; }
    public long? PaymentPeriod { get; set; }
    public double? CommissionRate { get; set; }
    public double? CommissionAmount { get; set; }
    public string CommissionInvoiceSerialNumber { get; set; }
    public double? SellerRevenue { get; set; }
    public string OrderNumber { get; set; }
    public long? OrderDate { get; set; }
    public long? PaymentOrderId { get; set; }
    public long? PaymentDate { get; set; }
    public long? SellerId { get; set; }
    public long? StoreId { get; set; }
    public string StoreName { get; set; }
    public string StoreAddress { get; set; }
    public string Country { get; set; }
 }

    }

