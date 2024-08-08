using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrendyolDeneme
{
    public partial class TyMusteriCevap : Form
    {
        private Content selectedContent;


        public TyMusteriCevap(Content content)
        {
            InitializeComponent();
            selectedContent = content;
        }

        private void TyMusteriCevap2_Load(object sender, EventArgs e)
        {
            if (selectedContent != null)
            {
                textUrun.Text = selectedContent.ProductName;
                textTarih.Text = selectedContent.CreationDate.ToString();
                textMusIsım.Text = selectedContent.UserName;
                memoEditSoru.Text = selectedContent.Text;
                urunUrl.Text = selectedContent.WebUrl.ToString();
                urunUrl.ForeColor = Color.Blue;
                urunUrl.Cursor = Cursors.Hand;
               

                if (selectedContent != null && selectedContent.Answer != null)
                {
                    memoEditCvp.Text = selectedContent.Answer.Text;
                    memoEditCvp.ReadOnly = true;
                }
                else
                {
                    
                    memoEditCvp.Text = "";
                    memoEditCvp.ReadOnly = false;
                }
                LoadImage(selectedContent.ImageUrl);
            }

        }
        private void urunUrl_Click_1(object sender, EventArgs e)
        {
            string url = urunUrl.Text;

            
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                try
                {
                   
                    System.Diagnostics.Process.Start(url);
                }
                catch (Exception ex)
                {
                   
                    MessageBox.Show("URL açılamadı: " + ex.Message);
                }
            }
            else
            {
                
                MessageBox.Show("Geçersiz URL");
            }
        }
        private void LoadImage(Uri imageUrl)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    byte[] imageData = webClient.DownloadData(imageUrl);
                    using (var stream = new System.IO.MemoryStream(imageData))
                    {
                        Image originalImage = Image.FromStream(stream);

                        int newWidth = 200;
                        int newHeight = 200;
                        Image resizedImage = ResizeImage(originalImage, newWidth, newHeight);

                        pictureImage.Image = resizedImage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Resim yüklenirken bir hata oluştu: {ex.Message}");
            }
        }
        //private void urunUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Link.LinkData.ToString()) { UseShellExecute = true });
        //}

        private Image ResizeImage(Image image, int newWidth, int newHeight)
        {
            Bitmap newImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }
        private void btnCvp_Click(object sender, EventArgs e)
        {
            try
            {
                string answerText = memoEditCvp.Text;

          
                if (answerText.Length < 10 || answerText.Length > 200)
                {
                    MessageBox.Show("Cevap 10 ile 200 karakter arasında olmalıdır.");
                    return;
                }

                long questionId = selectedContent.Id;

                EntityContent response = dbMusteriSoruCvp.PostCreateAnswer(questionId, answerText);

                if (response != null)
                {
                    MessageBox.Show("Cevap başarıyla gönderildi.");
                    Close();
                }
                else
                {
                    MessageBox.Show("Cevap gönderilirken bir hata oluştu.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cevap gönderilirken bir hata oluştu: {ex.Message}");
            }
        }
        private void textTarih_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value?.ToString()) && long.TryParse(e.Value.ToString(), out long unixTimeStamp))
            {
               
                DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStamp).LocalDateTime;
                e.DisplayText = dateTime.ToString("dd.MM.yyyy HH:mm");
            }
        }
        private void btnIptal_Click(object sender, EventArgs e)
        {
            Close();
        }

       
    }
}
