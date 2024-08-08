using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;

namespace TrendyolDeneme
{
    public partial class CariEkstre : Form
    {
        public CariEkstre()
        {
            InitializeComponent();
        }
        private void CariEkstre_Load(object sender, EventArgs e)
        {
            DateTime simdikiZaman = DateTime.Now;

            dateEditBitis.DateTime = simdikiZaman.Date.AddDays(-40);
            dateEditBaslangic.DateTime = dateEditBitis.DateTime.AddDays(-15);

            List<EntityStatus> sliste = new List<EntityStatus>();
            sliste.Add(new EntityStatus() { Kodu = " ", Adi = " " });
            sliste.Add(new EntityStatus() { Kodu = "Sale", Adi = "Satış" });
            sliste.Add(new EntityStatus() { Kodu = "Return", Adi = "İade" });
            sliste.Add(new EntityStatus() { Kodu = "Discount", Adi = "İndirim" });
            sliste.Add(new EntityStatus() { Kodu = "DiscountCancel", Adi = "İndirim İptali" });
            sliste.Add(new EntityStatus() { Kodu = "Coupon", Adi = "Kupon" });
            sliste.Add(new EntityStatus() { Kodu = "CouponCancel", Adi = "Kupon İptali" });
            sliste.Add(new EntityStatus() { Kodu = "TYDiscount", Adi = "Trendyol İndirimi" });
            sliste.Add(new EntityStatus() { Kodu = "TYDiscountCancel", Adi = "Trendyol İndirimi İptali" });
            sliste.Add(new EntityStatus() { Kodu = "TYCoupon", Adi = "Trendyol Kuponu" });
            sliste.Add(new EntityStatus() { Kodu = "TYCouponCancel", Adi = "Trendyol Kuponu İptali" });

            lookUpİslemTip.Properties.DataSource = sliste;
            lookUpİslemTip.EditValue = "Sale";

            List<EntityStatus> sliste2 = new List<EntityStatus>();
            sliste2.Add(new EntityStatus() { Kodu = " ", Adi = " " });
            sliste2.Add(new EntityStatus() { Kodu = "CashAdvance", Adi = "Nakit Avans" });
            sliste2.Add(new EntityStatus() { Kodu = "WireTransfer", Adi = "Elektronik Transfer" });
            sliste2.Add(new EntityStatus() { Kodu = "IncomingTransfer", Adi = "Gelen Havale" });
            sliste2.Add(new EntityStatus() { Kodu = "ReturnInvoice", Adi = "İade Faturaları" });
            sliste2.Add(new EntityStatus() { Kodu = "CommissionAgreementInvoice", Adi = "Komisyon Sözleşmesi Faturası" });
            sliste2.Add(new EntityStatus() { Kodu = "PaymentOrder", Adi = "Ödeme Talimatı" });
            sliste2.Add(new EntityStatus() { Kodu = "DeductionInvoices", Adi = "Kesinti Faturaları" });

            lookUpDigerFinans.Properties.DataSource = sliste2; 
            lookUpDigerFinans.EditValue = " ";

        }
        private void RefreshData()
        {
            DateTime startDate = dateEditBaslangic.DateTime.Date;
            DateTime endDate = dateEditBitis.DateTime.Date;

            string transactionType = null;
            if (lookUpİslemTip.EditValue != null && !string.IsNullOrWhiteSpace(lookUpİslemTip.EditValue.ToString()))
            {
                transactionType = lookUpİslemTip.EditValue.ToString();
            }

            string transactionType2 = null;
            if (lookUpDigerFinans.EditValue != null && !string.IsNullOrWhiteSpace(lookUpDigerFinans.EditValue.ToString()))
            {
                transactionType2 = lookUpDigerFinans.EditValue.ToString();
            }
            if (string.IsNullOrWhiteSpace(transactionType) && string.IsNullOrWhiteSpace(transactionType2))
            {
                XtraMessageBox.Show("Lütfen işlem tipi seçin.");
                return;
            }
            if (!string.IsNullOrWhiteSpace(transactionType) && !string.IsNullOrWhiteSpace(transactionType2))
            {
                XtraMessageBox.Show("Sadece bir işlem tipi seçilebilir.");
                return;
            }

            using (var context = new TrendyolDenemeEntities())
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(transactionType))
                    {
                        CariSettlements trendyolData = dbCari.GetCariSettlements(startDate, endDate, transactionType);

                        if (trendyolData != null && trendyolData.ContentCari != null)
                        {
                            var matchedData = (from settlement in trendyolData.ContentCari
                                               join kargo in context.TyKargoFatura
                                                   on settlement.OrderNumber equals kargo.OrderNumber.ToString() into kargoJoin
                                               from kargo in kargoJoin.DefaultIfEmpty()
                                               where kargo != null
                                               select new
                                               {
                                                   settlement.Id,
                                                   settlement.Barcode,
                                                   settlement.ReceiptId,
                                                   settlement.OrderNumber,
                                                   settlement.Description,
                                                   settlement.TransactionDate,
                                                   settlement.Debt,
                                                   settlement.Credit,
                                                   settlement.PaymentPeriod,
                                                   settlement.PaymentDate,
                                                   settlement.CommissionRate,
                                                   settlement.CommissionAmount,
                                                   settlement.SellerRevenue,
                                                   ShipmentPackageType = kargo.ShipmentPackageType,
                                                   ParcelUniqueId = kargo.ParcelUniqueId,
                                                   Amount = kargo.Amount,
                                                   Desi = kargo.Desi,
                                                   NetRevenue = settlement.SellerRevenue.HasValue && kargo.Amount.HasValue ?
                                                       Math.Round(settlement.SellerRevenue.Value - kargo.Amount.Value, 2) :
                                                       (double?)null
                                               });

                            if (transactionType == "Sale")
                            {
                                var groupedDataSale = matchedData
                                    .GroupBy(x => x.OrderNumber)
                                    .Select(g => new
                                    {
                                        OrderNumber = g.Key,
                                        Id = g.Key,
                                        Description = g.First().Description,
                                        TransactionDate = g.First().TransactionDate,
                                        Debt = g.First().Debt,
                                        PaymentDate = g.First().PaymentDate,
                                        ShipmentPackageType = g.First().ShipmentPackageType,
                                        Amount = g.Sum(x => x.Amount ?? 0),
                                        TotalKredi = g.Sum(x => x.Credit),
                                        TotalKomisyon = g.Sum(x => x.CommissionAmount),
                                        TotalSatıcıGeliri = g.Sum(x => x.SellerRevenue ?? 0),
                                        NetRevenue = g.Sum(x => (x.SellerRevenue ?? 0) - (x.Amount ?? 0))
                                    })
                                    .ToList();

                                gridControl1.DataSource = groupedDataSale;
                                gridControl1.RefreshDataSource();
                            }
                            else if (transactionType == "Return")
                            {
                                var groupedDataReturn = matchedData
                                    .GroupBy(x => x.OrderNumber)
                                    .Select(g => new
                                    {
                                        OrderNumber = g.Key,
                                        Id = g.Key,
                                        Description = g.First().Description,
                                        TransactionDate = g.First().TransactionDate,
                                        Debt = g.First().Debt,
                                        PaymentDate = g.First().PaymentDate,
                                        ShipmentPackageType = g.First().ShipmentPackageType,
                                        Amount = g.Sum(x => x.Amount ?? 0),
                                        TotalKredi = g.Sum(x => x.Credit),
                                        TotalKomisyon = g.Sum(x => x.CommissionAmount),
                                        TotalSatıcıGeliri = g.Sum(x => x.SellerRevenue ?? 0),
                                         NetRevenue = -(g.Average(x => x.Amount ?? 0))
                                    })
                                    .ToList();

                                gridControl1.DataSource = groupedDataReturn;
                                gridControl1.RefreshDataSource();
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("Trendyol verileri alınamadı.");
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(transactionType2))
                    {
                        CariSettlements trendyolData2 = dbCari.GetCariOtherFinancials(startDate, endDate, transactionType2);

                        if (trendyolData2 != null && trendyolData2.ContentCari != null)
                        {
                            gridControl1.DataSource = trendyolData2.ContentCari;
                            gridControl1.RefreshDataSource();
                        }
                        else
                        {
                            XtraMessageBox.Show("Trendyol verileri alınamadı.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show($"Hata oluştu: {ex.Message}");
                }
            }
        }
            private void gridView1_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "TransactionDate" && !string.IsNullOrEmpty(e.Value?.ToString()))
            {
                if (long.TryParse(e.Value.ToString(), out long unixTimeStampTransaction))
                {
                    DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStampTransaction).LocalDateTime;
                    e.DisplayText = dateTime.ToString("dd.MM.yyyy HH:mm");
                }
            }

            if (e.Column.FieldName == "PaymentDate" && !string.IsNullOrEmpty(e.Value?.ToString()))
            {
                if (long.TryParse(e.Value.ToString(), out long unixTimeStampPayment))
                {
                    DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStampPayment).LocalDateTime;
                    e.DisplayText = dateTime.ToString("dd.MM.yyyy HH:mm");
                }
            }
        }

        private void btnListele_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void btnKargoListe_Click(object sender, EventArgs e)
        {
            KargoFaturaList frm = new KargoFaturaList();
            frm.ShowDialog();
        }

      
        private void btnKapat_Click_1(object sender, EventArgs e)
        {
            Close();
        }
    }
}



