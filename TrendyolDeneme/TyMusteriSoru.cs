using DevExpress.DocumentView.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace TrendyolDeneme
{
    public partial class TyMusteriSoru : Form
    {
        public TyMusteriSoru()
        {
            InitializeComponent();

        }
        private void TyMusteriSoru_Load_1(object sender, EventArgs e)
        {

            dateEdit2.DateTime = DateTime.Today.AddDays(1);
            dateEdit1.DateTime = dateEdit2.DateTime.AddDays(-3);

            List<EntityStatus> sliste = new List<EntityStatus>();
            sliste.Add(new EntityStatus() { Kodu = "", Adi = "Tüm Sorular" });
            sliste.Add(new EntityStatus() { Kodu = "WAITING_FOR_ANSWER", Adi = "Cevap Bekleyenler" });
            sliste.Add(new EntityStatus() { Kodu = "WAITING_FOR_APPROVE", Adi = "Onay Bekleyenler" });
            sliste.Add(new EntityStatus() { Kodu = "ANSWERED", Adi = "Cevaplananlar" });
            sliste.Add(new EntityStatus() { Kodu = "UNANSWERED", Adi = "Cevaplanmayanlar" });
            sliste.Add(new EntityStatus() { Kodu = "REPORTED", Adi = "Sorun Bildirenler" });
            sliste.Add(new EntityStatus() { Kodu = "REJECTED", Adi = "Reddedilenler" });


            sliste.Add(new EntityStatus() { Kodu = "WAITING_FOR_ANSWER", Adi = "Cevap Bekliyor" });

            lookUpEdit1.Properties.DataSource = sliste;
            repositoryItemGridLookUpEdit1.DataSource = sliste; //**** yeni eklenecek
            lookUpEdit1.EditValue = "";
        }
        private void RefreshData()
        {
            DateTime startDate = dateEdit1.DateTime.Date;
            DateTime endDate = dateEdit2.DateTime.Date;
            string status = lookUpEdit1.EditValue.ToString();

            EntityContent trendyolData = dbMusteriSoruCvp.GetTrendyolDataAnswers(startDate, endDate, status);

            if (trendyolData != null)
            {
                splashScreenManager1.ShowWaitForm();
                gridControl1.DataSource = trendyolData.Content;
                splashScreenManager1.CloseWaitForm();
            }
            else
            {
                MessageBox.Show("Trendyol verileri alınamadı.");
            }
        }
        private void gridView1_MasterRowGetChildList(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetChildListEventArgs e)
        {
            GridView view = sender as GridView;
            Content row = view.GetRow(e.RowHandle) as Content;
            e.ChildList = new BindingList<Answer> { row.Answer };
        }
        private void gridView1_MasterRowEmpty(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowEmptyEventArgs e)
        {
            GridView view = sender as GridView;
            Content row = view.GetRow(e.RowHandle) as Content;
            e.IsEmpty = row.Answer == null;
        }
        private void gridView1_MasterRowGetRelationCount(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationCountEventArgs e)
        {
            e.RelationCount = 1;
        }
        private void gridView1_MasterRowGetRelationName(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationNameEventArgs e)
        {
            e.RelationName = "Cevap";
        }
        private void btnListele_Click_1(object sender, EventArgs e)
        {
            RefreshData();
        }
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            int selectedRowHandle = view.FocusedRowHandle;
            if (selectedRowHandle >= 0)
            {
                Content selectedContent = view.GetRow(selectedRowHandle) as Content;


                TyMusteriCevap frm = new TyMusteriCevap(selectedContent);
                frm.ShowDialog();
            }
        }
        private void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "CreationDate" && !string.IsNullOrEmpty(e.Value?.ToString()) && long.TryParse(e.Value.ToString(), out long unixTimeStamp))
            {
                DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStamp).LocalDateTime;
                e.DisplayText = dateTime.ToString("dd.MM.yyyy HH:mm");
            }

        }

     
    }
}


