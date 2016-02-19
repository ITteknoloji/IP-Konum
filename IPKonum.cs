using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using DevExpress;
using System.Net;
using System.Text.RegularExpressions;
using DevExpress.XtraMap;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using RestSharp;


namespace IP_konum_bul
{
    public partial class IPKonum : DevExpress.XtraEditors.XtraForm
    {
        public IPKonum()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textEdit5.Text != null)
            {
                this.Query(textEdit5.Text);
            }
            else MessageBox.Show("Lütfen IP adresini giriniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        int s1, s2, snc;

        private void Query(string strIPAddress)
        {
            string ulke;
            IP2Location.IPResult oIPResult = new IP2Location.IPResult();
            IP2Location.Component oIP2Location = new IP2Location.Component();
            try
            {

                if (strIPAddress != "")
                {
                    oIP2Location.IPDatabasePath = Application.StartupPath + @"\IPVeritabani.BIN"; // only IPv4
                    oIP2Location.IPLicensePath = Application.StartupPath + @"\Lisans.key";

                    oIPResult = oIP2Location.IPQuery(strIPAddress);
                    switch (oIPResult.Status.ToString())
                    {
                        case "OK":
                            textEdit1.Text = (oIPResult.IPAddress);
                            textEdit2.Text = (oIPResult.City);
                            textEdit3.Text = ("" + oIPResult.Latitude + "");
                            textEdit4.Text = ("" + oIPResult.Longitude + "");
                            ulke = oIPResult.CountryShort;
                            textEdit9.Text = ulke;
                            ulke = ulke.ToLower();
                            pictureBox1.ImageLocation = Application.StartupPath + @"\16\" + ulke + "_16.png";

                            break;
                        case "EMPTY_IP_ADDRESS":
                            MessageBox.Show("IP adresini giriniz..!");
                            break;
                        case "INVALID_IP_ADDRESS":
                            MessageBox.Show("IP Adresi Bulunamadı.");
                            break;
                        case "MISSING_FILE":
                            MessageBox.Show("Veritabanı bulunamadı.");
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("IP adresini giriniz..!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                oIPResult = null;
                oIP2Location = null;
            }
        }
        public string DisIpGetir()
        {

            try
            {
                string externalIP;
                externalIP = (new System.Net.WebClient()).DownloadString("http://checkip.dyndns.org/");
                externalIP = (new System.Text.RegularExpressions.Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")).Matches(externalIP)[0].ToString();
                return externalIP;
            }
            catch (Exception ex)
            {
                string hata = "Ulaşılamadı!";
                MessageBox.Show(ex.Message);
                return hata;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //textEdit5.Text= DisIpGetir().ToString();
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
            else { MessageBox.Show("İşlem Devam Etmektedir. Lütfen Bekleyiniz.."); }

        }

        private void windowsUIButtonPanel1_Click(object sender, EventArgs e)
        {

            try
            {
                mapControl1.CenterPoint = new GeoPoint(Convert.ToDouble(textEdit3.Text), Convert.ToDouble(textEdit4.Text));
                MapPushpin pp2 = new MapPushpin();
                pp2.Location = new GeoPoint(Convert.ToDouble(textEdit3.Text), Convert.ToDouble(textEdit4.Text));
                MapBubble ba = new MapBubble();
                ba.MarkerType = MarkerType.Cross;
                ba.Location = new GeoPoint(Convert.ToDouble(textEdit3.Text), Convert.ToDouble(textEdit4.Text));
                ba.SelectedFill = Color.White;
                ba.SelectedStroke = Color.Red;
                ba.Fill = System.Drawing.Color.Red;
                ba.Stroke = System.Drawing.Color.White;
                InformationLayer ly2 = new InformationLayer();
                ly2.Data.Items.Add(ba);
                mapControl1.Layers.Add(ly2);
                dockPanel1.Show();
            }
            catch
            {
                MessageBox.Show("Konum bilgisi hatalıdır.Lütfen Konum Hesapla butonuna bastığınızdan emin olunuz!.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dockPanel1.Close();
            dockPanel2.Close();
            dockPanel3.Close();
            dockPanel4.Close();
            backgroundWorker2.WorkerReportsProgress = true;
            backgroundWorker2.WorkerSupportsCancellation = true;
        }

        private void ip()
        {
            TcpClient asd = new TcpClient();
            // MessageBox.Show(ipString);     
            IPAddress address = IPAddress.Parse(textEdit6.Text);
            for (int i = Convert.ToInt32(textEdit7.Text); i <= Convert.ToInt32(textEdit8.Text); i++)
            {
                try
                {
                    asd.SendTimeout = 500;
                    asd.ReceiveTimeout = 500;
                    asd.Connect(address, i);

                    if (asd.Connected)
                    {
                        listBox1.Items.Add("Port " + i + " is open");
                    }
                }
                catch
                {
                    listBox1.Items.Add("Port " + i + " is closed");
                }
                a = a + deger;
                backgroundWorker2.ReportProgress(Convert.ToInt32(a));
            }
        }

        private void textEdit5_Enter(object sender, EventArgs e)
        {
            textEdit6.Text = textEdit5.Text;
        }

        private void textEdit5_TextChanged(object sender, EventArgs e)
        {
            textEdit6.Text = textEdit5.Text;
        }

        private void IPKonum_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    radialMenu1.ShowPopup(Cursor.Position);
                    break;
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockPanel1.Show();
            radialMenu1.HidePopup();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockPanel2.Show();
            radialMenu1.HidePopup();
        }
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockPanel3.Show();
            radialMenu1.HidePopup();
        }
        double a;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            textEdit5.Text = DisIpGetir();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            ip();
        }
        double deger;
        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            progressBarControl1.EditValue = e.ProgressPercentage;
            label19.Text = "%" + progressBarControl1.EditValue + " tamamlandı lütfen bekleyiniz..";
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            progressBarControl1.EditValue = 100;
            label19.Text = "%100 İşlem tamamlandı.";
        }
        private void windowsUIButtonPanel3_Click(object sender, EventArgs e)
        {
            backgroundWorker2.CancelAsync();
            MessageBox.Show("İşlem Durduruldu..");
            label19.Text = "İşlem Durduruldu..";


        }

        private void windowsUIButtonPanel2_Click_1(object sender, EventArgs e)
        {
            if (!backgroundWorker2.IsBusy)
            {
                if (textEdit6.Text == "")
                {
                    MessageBox.Show("Lütfen IP giriniz!!!");
                }
                else if (textEdit7.Text == "" || textEdit8.Text == "")
                {

                    MessageBox.Show("Port aralığı giriniz");
                }
                else
                {
                    label19.Text = "Lütfen bekleyin..";
                    progressBarControl1.EditValue = 0;
                    s1 = Convert.ToInt32(textEdit7.Text);
                    s2 = Convert.ToInt32(textEdit8.Text);
                    snc = s2 - s1;
                    snc = snc + 1;
                    deger = 100 / snc;
                    progressBarControl1.Properties.Minimum = 0;
                    progressBarControl1.Properties.Maximum = 100;
                    backgroundWorker2.RunWorkerAsync();

                }
            }
            else { MessageBox.Show("İşlem devam ediyor.. Lütfen Bekleyiniz"); }
        }
        //******************************* 11/02/2016 TELNET**
        private void button3_Click(object sender, EventArgs e)
        {
            if (textEdit5.Text != "")
            {
                if (!backgroundWorker3.IsBusy)
                {
                    backgroundWorker3.RunWorkerAsync(); //telnet başlat
                }

                else MessageBox.Show("İşlem devam ediyor. Lütfen Bekleyiniz..");
            }
            else MessageBox.Show("Lütfen IP giriniz!!..");
        }
        //******************************


        private void basla()
        {
            AllocConsole();
            Console.WriteLine("IP adresi: {0}", textEdit5.Text);
            Console.Write("Port Numarası giriniz : ");
            string girdi = Console.ReadLine();
            MessageBox.Show("bekleyin..");
            ipislem(girdi);





        }
        [DllImport("kernel32.dll", SetLastError = true)]    //Console uygulama açmak için kernel32.dll kütüphanesini çağırıyoruz
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();


        private void ipislem(string Girdi)
        {
            //**Burda kaldım. 

        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            basla();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockPanel4.Show();
            radialMenu1.HidePopup();

        }

        private void windowsUIButtonPanel4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Lütfen Değerleri Giriniz!");
            }
            else
            {
                listBox1.Items.Add("lütfen bekleyin..");
                dockPanel1.Show();
                backgroundWorker4.RunWorkerAsync();
            }
        }
        bool durum=false;
        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
                try
                {
                    string MCC = textBox3.Text, MNC = textBox4.Text, LAC = textBox2.Text, CellId = textBox1.Text;
                    var client = new RestClient("https://us1.unwiredlabs.com/v2/process.php");
                    var request = new RestRequest(Method.POST);
                    request.AddParameter("undefined", "{\"token\": \"922fdf96f5807d\",\"radio\": \"gsm\",\"mcc\": " + MCC + ",\"mnc\": " + MNC + ",\"cells\": [{\"lac\": " + LAC + ",\"cid\": " + CellId + "}],\"address\":  1}", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string deger = response.Content;
                    string[] dizi = deger.Split('"');
                    string enlem, boylam, adres, kesinlik;
                    enlem = dizi[8];
                    enlem = enlem.Replace(":", string.Empty);
                    enlem = enlem.Replace(",", string.Empty);
                    enlem = enlem.Replace(".", ",");
                    boylam = dizi[10];
                    boylam = boylam.Replace(":", string.Empty);
                    boylam = boylam.Replace(",", string.Empty);
                    boylam = boylam.Replace(".", ",");
                    kesinlik = dizi[12];
                    kesinlik = kesinlik.Replace(":", string.Empty);
                    kesinlik = kesinlik.Replace(",", string.Empty);
                    adres = dizi[15];
                    adres = adres.Replace(",", string.Empty);
                    richTextBox1.Text = "Enlem" + Convert.ToDouble(enlem) + "\nBoylam:" + boylam + "\nKesinlik:" + kesinlik;

                    try
                    {
                        mapControl1.CenterPoint = new GeoPoint(Convert.ToDouble(enlem), Convert.ToDouble(boylam));
                        MapPushpin pp1 = new MapPushpin();
                        pp1.Location = mapControl1.CenterPoint;
                        MapBubble b = new MapBubble();
                        b.Location = new GeoPoint(Convert.ToDouble(enlem), Convert.ToDouble(boylam));
                        b.MarkerType = MarkerType.Cross;
                        b.SelectedFill = Color.White;
                        b.SelectedStroke = Color.Red;
                        b.Fill = System.Drawing.Color.Red;
                        b.Stroke = System.Drawing.Color.White;
                        InformationLayer ly = new InformationLayer();
                        ly.Data.Items.Add(b);
                        mapControl1.Layers.Add(ly);
                        durum = true;


                    }
                    catch
                    {
                        MessageBox.Show("Konum bilgisi hatalıdır.Lütfen Konum Hesapla butonuna bastığınızdan emin olunuz!.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch
                {
                    MessageBox.Show("Girilen değerler yanlış yada bulunamadı..");
                }
            
            // listBox2.Items.Add(a.konum);

        }

        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (durum != false)
            {
                mapControl1.Zoom(15);
                durum = false;
            }

        }
        //public BazIstasyonBilgi getir(string CellId, string LAC, string MCC, string MNC)
        //{

        //    BazIstasyonBilgi baz = response.Request;
        //    return;
        //}

        //public class BazIstasyonBilgi
        //{
        //    public BazIstasyonBilgi() { }
        //    public BazIstasyonBilgi(string mnc, string mcc, string lac)
        //    {
        //        this.Mnc = mnc;
        //        this.Mcc = mcc;
        //        this.Lac = lac;
        //    }
        //    public string Mnc { get; set; }
        //    public string Mcc { get; set; }
        //    public string Lac { get; set; }
        //    public string CellID { get; set; }
        //    public Konum konum { get; set; }


        //    public class Konum
        //    {
        //        public Konum() { }
        //        public Konum(string Enlem, string Boylam)
        //        {
        //            this.enlem = Enlem;
        //            this.boylam = Boylam;
        //        }
        //        public string enlem { get; set; }
        //        public string boylam { get; set; }
        //        public Adres adres { get; set; }

        //        public class Adres
        //        {
        //            public Adres() { }
        //            public string ulke { get; set; }
        //            public string ulke_code { get; set; }
        //            public string sehir { get; set; }
        //            public string bolge { get; set; }
        //            public string sokak { get; set; }
        //            public string sokak_number { get; set; }
        //            public string posta_kodu { get; set; }
        //        }
        //    }

        //}

    }
}