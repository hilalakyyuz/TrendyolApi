using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrendyolDeneme
{
    public partial class CariDiger : Form
    {
        public CariDiger()
        {
            InitializeComponent();
        }

        private void CariDiger_Load(object sender, EventArgs e)
        {

            dateEdit2.DateTime = DateTime.Now;
            dateEdit1.DateTime = dateEdit2.DateTime.AddDays(-14);

            List<EntityStatus> sliste = new List<EntityStatus>();
            sliste.Add(new EntityStatus() { Kodu = "CashAdvance", Adi = "Nakit Avans" });
            sliste.Add(new EntityStatus() { Kodu = "WireTransfer", Adi = "Elektronik Transfer" });
            sliste.Add(new EntityStatus() { Kodu = "IncomingTransfer", Adi = "Gelen Havale" });
            sliste.Add(new EntityStatus() { Kodu = "ReturnInvoice", Adi = "İade Faturaları" });
            sliste.Add(new EntityStatus() { Kodu = "CommissionAgreementInvoice", Adi = "Komisyon Sözleşmesi Faturası" });
            sliste.Add(new EntityStatus() { Kodu = "PaymentOrder", Adi = "Ödeme Talimatı" });
            sliste.Add(new EntityStatus() { Kodu = "DeductionInvoices", Adi = "Kesinti Faturaları" });

            lookUpEdit1.Properties.DataSource = sliste;
            lookUpEdit1.EditValue = "DeductionInvoices";
            lookUpEdit1.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Kodu", ""));
            lookUpEdit1.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Adi", ""));


            lookUpEdit1.Properties.Columns["Kodu"].Visible = false;
            lookUpEdit1.Properties.Columns["Adi"].Visible = true;

        }
        private void RefreshData()
        {
            DateTime startDate = dateEdit1.DateTime.Date;
            DateTime endDate = dateEdit2.DateTime.Date;


            string transactionType = lookUpEdit1.EditValue.ToString();

            CariSettlements trendyolData = dbCari.GetCariOtherFinancials(startDate, endDate, transactionType);

            if (trendyolData != null && trendyolData.ContentCari != null)
            {
                gridControl1.DataSource = trendyolData.ContentCari;
                gridControl1.Refresh();
            }
            else
            {
                MessageBox.Show("Trendyol verileri alınamadı.");
            }
        }

        private void btnListele_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
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

        private void btnKargoListe_Click_1(object sender, EventArgs e)
        {
            KargoFaturaList frm = new KargoFaturaList();
            frm.ShowDialog();
        }
    }
}

 
