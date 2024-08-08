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
    public partial class apiDeneme : Form
    {
        public apiDeneme()
        {
            InitializeComponent();
        }

        private void apiDeneme_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
           KargoFatura trendyolDataList = dbCari.GetKargoFatura("DDF2024007899010");

                if (trendyolDataList != null && trendyolDataList.KargoFaturaDetay != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in trendyolDataList.KargoFaturaDetay)
                {
                    sb.AppendLine($"Gönderi Paket Türü: {item.ShipmentPackageType}, Parça Benzersiz Kimlik: {item.ParcelUniqueId}, Sipariş Numarası: {item.OrderNumber}, Tutar: {item.Amount}, Desi: {item.Desi}");
                }

                richTextBox1.Text = sb.ToString();
            }
            else
            {
                richTextBox1.Text = "Veri bulunamadı.";
            }
        }
    }
}
