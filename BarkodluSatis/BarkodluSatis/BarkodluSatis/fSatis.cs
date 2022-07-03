using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarkodluSatis
{
    public partial class fSatis : Form
    {
        public fSatis()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fSatis_Load(object sender, EventArgs e)
        {
            HizliButonDoldur();
            b5.Text = 5.ToString("C2");     //Form yüklenirken b5 in textini para formatında 5 yap dedik.
            b10.Text = 10.ToString("C2");   //Form yüklenirken b10 in textini para formatında 10 yap dedik.
            b20.Text = 20.ToString("C2");   //Form yüklenirken b20 in textini para formatında 20 yap dedik.
            b50.Text = 50.ToString("C2");   //Form yüklenirken b50 in textini para formatında 50 yap dedik.
            b100.Text = 100.ToString("C2"); //Form yüklenirken b100 in textini para formatında 100 yap dedik.
            b200.Text = 200.ToString("C2"); //Form yüklenirken b200 in textini para formatında 200 yap dedik.


        }
        private void HizliButonDoldur()     //Hızlı Butonları doldurmak için parametre tanımladık.
        {
            var hizliurun = db.HizliUrun.ToList();  //ToList ile Hizli Urun tablosunu listeledik. Gelen bilgiliyi Hizli urun değişkenine atadık. 
            foreach (var item in hizliurun)    //Foreach ile bütün listeyi gez liste adı=hizliurun listede arama işlemi yapıyoruz.
            {
                Button bH = this.Controls.Find("bH" + item.Id, true).FirstOrDefault() as Button; //Form içerisindeki kontrollerden bH ile başlayıp id sini de ekleyerek bulacak ve FirstOrDefault öncelik belirtmek için kullanılır.Ve buton olacağını belirttik.
                if (bH != null)   //Eğer bH null dan farklı ise
                {
                    double fiyat = Islemler.DoubleYap(item.Fiyat.ToString()); //İşlemler sınıfı içerisinde DoubleYap() parametresi içerisine item den gelen string fiyat bilgisini fiyat değişkenine double türünde atama işlemini yaptık.
                    bH.Text = item.UrunAd + "\n" + fiyat.ToString("C2"); //bH nin textine urun ad gelecek ve alta geçmek için n ve itemin fiyat bilgisi TL formatında gelecek.
                }
            }
        }
        private void HizliButonClick(object sender, EventArgs e)   //Objenin gönderdiğini tanımlamak için sender kullandık unboxing için gerekli, EventArgs e ise form ekranında action click te metodu görünür hale getirmek için kullandık.
        {
            Button b = (Button)sender; //Gelen değişkeni buton olarak b değişkenine at. Unboxing işlemi sender ile yapmıs oluyoruz.
            int butonid = Convert.ToInt16(b.Name.ToString().Substring(2, b.Name.Length - 2)); //b butonunun name ini 2 den başla ve name uzunluğundan ilk 2 yi çıkart. Yani bH1 iken bH yi cıkartacak geriye 1,2,11,21 gibi karakter kalacak.Bize verilen değer string olduğu için çıkan değer int olduğundan Convert.ToInt16 yapıyoruz.

            if (b.Text.ToString().StartsWith("-")) //Eğer butonun texti başlangıçta - ile başlıyor ise ürün ekleme sayfasını aç değilse işlem yap.
            {
                fHizliButonUrunEkle f = new fHizliButonUrunEkle(); //fHizliButonUrunEkle form sayfasını tanımladım.
                f.lButonId.Text = butonid.ToString(); //lButonId nin textine butonid yi yazdırdım.
                f.ShowDialog(); //Diğer formu açmak için yazdık.
            }
            else
            {
                // => İşlem Yap
                var urunbarkod = db.HizliUrun.Where(a => a.Id == butonid).Select(a => a.Barkod).FirstOrDefault(); //Veritabanı içerisinde HizliUrun tablosu içerisinde id si buttonid yi ve barkod sütunundan al ve urunbarkod değişkenine atama işlemini yaptık. 
                var urun = db.Urun.Where(a => a.Barkod == urunbarkod).FirstOrDefault(); //Ürün tablosunda barkod sütununda ürün barkodu eşit olanını getir urun degiskenine ata.
                UrunGetirListeye(urun, urunbarkod, Convert.ToDouble(tMiktar.Text)); //UrunGetirListeye() metoduna parametre gönderdik ve çalıştırdık.
                GenelToplam(); //GenelToplam() parametresini çalıştırdık.
            }
        }
        BarkodDbEntities db = new BarkodDbEntities();

        private void tBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)    //Barkod Kutusunda Enter e basıldığında
            {
                string barkod = tBarkod.Text.Trim();
                if (barkod.Length <= 2)    // 2 karakterden küçük veya eşitse
                {
                    tMiktar.Text = barkod;  // Adet olarak algılatmak için.
                    tBarkod.Clear();
                    tBarkod.Focus();
                }

                else    // Değilse yani 2 den fazla ise
                {
                    if (db.Urun.Any(a => a.Barkod == barkod))   // Ürünü  'Ürün Tablosunda 'arama işlemi.
                    {
                        var urun = db.Urun.Where(a => a.Barkod == barkod).FirstOrDefault();
                        UrunGetirListeye(urun, barkod, Convert.ToDouble(tMiktar.Text));

                    }
                    else
                    {
                        int onek = Convert.ToInt32(barkod.Substring(0, 2));  //Barkod un ilk 2 karakterini al ve int türünde aldık.
                        if (db.Terazi.Any(a => a.TeraziOnEk == onek))  //Eğer database içerisinde terazi tablosunda onek den bilgiyi karşılaştırarak olup olmadığını sorguladık.Var ise if bloğuna girer.
                        {
                            string teraziurunno = barkod.Substring(2, 5);

                            if (db.Urun.Any(a => a.Barkod == teraziurunno))
                            {
                                var urunterazi = db.Urun.Where(a => a.Barkod == teraziurunno).FirstOrDefault();
                                double miktarkg = Convert.ToDouble(barkod.Substring(7, 5)) / 1000;
                                UrunGetirListeye(urunterazi, teraziurunno, miktarkg);

                            }

                            else
                            {
                                Console.Beep(900, 2000);
                                MessageBox.Show("Kg Ürün Ekleme Sayfası");
                            }

                        }

                        else
                        {
                            Console.Beep(900, 2000); //Beep Sesini ayarladık.
                            fUrunGiris f = new fUrunGiris(); // fUrunGiris Formumuzu tanımladık.
                            f.tBarkod.Text = barkod;
                            f.ShowDialog(); //Formu göster dedik.
                        }
                    }

                }
                gridSatisListesi.ClearSelection();
                GenelToplam();

            }
        }

        private void UrunGetirListeye(Urun urun, string barkod, double miktar)
        {
            int satirsayisi = gridSatisListesi.RowCount;
            //double miktar = Convert.ToDouble(tMiktar.Text);
            bool eklenmismi = false;

            if (satirsayisi > 0)   // Eğer ürün tablosunda ürünü bulduğumuz zaman aynı ürünler listeye eklenmiş ise listeyi satır sayısı kadar döngüye sokuyoruz. Döngü sonucunda o barkodu bulup güncellemesini yapıyoruz. Yani miktar ve toplam değişmiş oluyor.
            {
                for (int i = 0; i < satirsayisi; i++)
                {
                    if (gridSatisListesi.Rows[i].Cells["Barkod"].Value.ToString() == barkod)
                    {
                        gridSatisListesi.Rows[i].Cells["Miktar"].Value = miktar + Convert.ToDouble(gridSatisListesi.Rows[i].Cells["Miktar"].Value);
                        gridSatisListesi.Rows[i].Cells["Toplam"].Value = Math.Round(Convert.ToDouble(gridSatisListesi.Rows[i].Cells["Miktar"].Value) * Convert.ToDouble(gridSatisListesi.Rows[i].Cells["Fiyat"].Value), 2);
                        eklenmismi = true;
                    }
                }

            }
            if (!eklenmismi)   // Eğer ürün tablosuna ürün eklenmemiş ise yeni bir satır ekleyip içerisine ürün tablosundan aldığımız değişkenlerin üzerine bilgileri aktarıyoruz. 
            {
                gridSatisListesi.Rows.Add();
                gridSatisListesi.Rows[satirsayisi].Cells["Barkod"].Value = barkod;
                gridSatisListesi.Rows[satirsayisi].Cells["UrunAdi"].Value = urun.UrunAd;
                gridSatisListesi.Rows[satirsayisi].Cells["UrunGrup"].Value = urun.UrunGrup;
                gridSatisListesi.Rows[satirsayisi].Cells["Birim"].Value = urun.Birim;
                gridSatisListesi.Rows[satirsayisi].Cells["Fiyat"].Value = urun.SatisFiyat;
                gridSatisListesi.Rows[satirsayisi].Cells["Miktar"].Value = miktar;
                gridSatisListesi.Rows[satirsayisi].Cells["Toplam"].Value = Math.Round(miktar * (double)urun.SatisFiyat, 2);
                gridSatisListesi.Rows[satirsayisi].Cells["AlisFiyati"].Value = urun.AlisFiyat;
                gridSatisListesi.Rows[satirsayisi].Cells["KdvTutari"].Value = urun.KdvTutari;
            }
        }
        private void GenelToplam()
        {
            double toplam = 0;
            for (int i = 0; i < gridSatisListesi.Rows.Count; i++)
            {
                toplam += Convert.ToDouble(gridSatisListesi.Rows[i].Cells["Toplam"].Value);
            }

            tGenelToplam.Text = toplam.ToString("C2"); // toplam değişkeninden gelen bilgiyi C2 - TL türü formatında geneltoplamın text ine yaz.
            tMiktar.Text = "1"; //tmiktar ın textini 1 yap dedik.
            tBarkod.Clear(); //Barkod Textbox içeriğini temizledik.
            tBarkod.Focus(); //Barkod Textbox ögesine tekrar odaklanmasını sağladık.  
        }

        private void gridSatisListesi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 9)   // Eğer tıklanan sütun 9 değerine eşit ise
            {
                gridSatisListesi.Rows.Remove(gridSatisListesi.CurrentRow); // Listedeki geçerli satırı kaldır.
                gridSatisListesi.ClearSelection();  //Listenin diğer elemanını seçmesini istemediğimiz için seçili olanı iptal ettik.
                GenelToplam();  //Toplam sonucunu tekrar güncellemesini sağladık.
                tBarkod.Focus(); //Barkod textbox kutusu içerisine odaklanmasını sağladık.
            }
        }

        private void bh_MouseDown(Object Sender, MouseEventArgs e) //Mouse down için eventi için bir metod tanımladık.
        {
            if (e.Button == MouseButtons.Right) // Eğer e buton mouse butonları içerisinde sağ tıklanmış ise
            {
                Button b = (Button)Sender;  // Yeni bir buton tanımladık ve tıklama ile gelen nesnenin içeriğini aktardık.
                if (!b.Text.StartsWith("-")) // Eğer butonun texti boş değil ise (- boş anlamına geliyor) - ile başlıyorsa.
                {
                    int butonid = Convert.ToInt16(b.Name.ToString().Substring(2, b.Name.Length - 2)); //Buton name ini aldık ve name 2 karakterden başla ve name uzunluğundan 2 karakter çıkartınca buton id yi bulmuş oluyoruz ve butonid değişkenimize atadık. 
                    ContextMenuStrip s = new ContextMenuStrip(); //Sağ tıklamak için gerekli olan ContextMenuStrip metodunu tanımladık.
                    ToolStripMenuItem sil = new ToolStripMenuItem(); //Sağ tıklama için gerekli olan silme işlemi için ToolStripMenuStrıpItem pencere metodunu tanımladık.
                    sil.Text = "Temizle - Buton No:" + butonid.ToString(); //Sil textine yazı ve buton id sini yazdırdık.
                    sil.Click += Sil_Click; // Silme işlemini yapabilmek için sil değişkenine bir click metodu özelliği ekledik.
                    s.Items.Add(sil); // s itemi içerisine sil den gelen tool umuzu ekledik.
                    this.ContextMenuStrip = s; // s den gelen içeriği ContextMenuStrip de uygula dedik.
                }


            }
        }

        private void Sil_Click(object sender, EventArgs e)
        {
            int butonid = Convert.ToUInt16(sender.ToString().Substring(19, sender.ToString().Length - 19)); //Sender buton nesnesiyle gelen bilgiyi 19.karakterden başlayarak ve bilginin uzunluğundan 19 karakter çıkartırsak buton id ye ulaşmış olduk.
            var guncelle = db.HizliUrun.Find(butonid); // Database içerisinde HizliUrun tablosunda butonid yi bul ve guncelleye atama işlemini yaptık.
            guncelle.Barkod = "-"; //Guncellenin Barkodunu - yap dedik.
            guncelle.UrunAd = "-";  //Guncellenin Ürün Adını - yap dedik.
            guncelle.Fiyat = 0; //Guncellenin Fiyatını 0 yap dedik.
            db.SaveChanges();  //Database imizde yapılan değişikleri kaydet ve güncelleme işlemini yaptık.
            double fiyat = 0; // double türünde ve içeriği 0 olan bir fiyat değişkeni tanımladık.
            Button b = this.Controls.Find("bH" + butonid, true).FirstOrDefault() as Button; // b adında bir buton tanımladık ve bH ile başlayan ve butonid bulma işlemini yaptık ve gelen bilgiyi b değişkenine atadık.
            b.Text = "-" + "\n" + fiyat.ToString("C2"); // b textine - ve fiyatı tl cinsinden yazdırdık.
        }

        private void bNx_Click(object sender, EventArgs e) //private türünde bir metod tanımladık.Bütün butonlar bu metod ile çalışacak.
        {
            Button b = (Button)sender; //Gelecek obje yani nesnemiz butondur dedik ve atadık.

            if (b.Text == ",")  //Eğer b texti "," ise dedik.
            {
                int virgul = tNumarator.Text.Count(x => x == ','); //tNumarator de girilen virgül sayısını say ve virgul değişkenine ata dedik.
                if (virgul < 1)   //Eğer virgül sayısı 1 den küçükse
                {
                    tNumarator.Text += b.Text;   // b objesinden gelen içeriği numaratörde güncelledik.
                }
            }

            else if (b.Text == "<")   // Eğer b texti "<" işaretine eşit ise
            {
                if (tNumarator.Text.Length > 0)   //Eğer tNumarator ün texti 0 dan büyükse
                {
                    tNumarator.Text = tNumarator.Text.Substring(0, tNumarator.Text.Length - 1); //tNumarator ün textini al 0 dan başla ve tNumarator ün uzunluğundan 2 çıkart bunuda tNumarator ün textinde güncelle dedik.

                }
            }

            else
            {
                tNumarator.Text += b.Text; // b objesinden gelen içeriği numaratörde güncelledik.
            }

        }

        private void bAdet_Click(object sender, EventArgs e)
        {
            if (tNumarator.Text != "")   //Eğer tNumarator boşdan farklıysa
            {
                tMiktar.Text = tNumarator.Text;   //Önce tNumarator ün textini tMiktar ın textine yazdırdık.
                tNumarator.Clear();  //tNumarator ün textini temizle.
                tBarkod.Clear();    //tBarkod un textini temizle.
                tBarkod.Focus();    //En son tBarkod a focuslansın dedik.

            }
        }

        private void bOdenen_Click(object sender, EventArgs e)
        {
            if (tNumarator.Text != "")   //Eğer tNumarator texti boş değil ise
            {
                double sonuc = Islemler.DoubleYap(tNumarator.Text) - Islemler.DoubleYap(tGenelToplam.Text); //Islemler Classı içerisinde tanımladığımız metodumuz içerine tNumarator ün textini ve tGenelToplam text ini gönderdik ve ikisinin farkını sonuc değişkenimize atadık.
                tParaUstu.Text = sonuc.ToString("C2"); // sonuc değişkenimizi para formatında tParaUstu textine yazdırdık.
                tOdenen.Text = Islemler.DoubleYap(tNumarator.Text).ToString("C2");
                tNumarator.Clear();  //tNumarator ü temizledik.
                tBarkod.Focus();    //tBarkoda tekrar focusladık.

            }
        }

        private void bBarkod_Click(object sender, EventArgs e)
        {
            if (tNumarator.Text != "")  //Eğer tNumarator ün texti boş değil ise dedik.
            {
                if (db.Urun.Any(a => a.Barkod == tNumarator.Text)) //Eğer database içerisinde Urun tablosunda Barkod değişkeninde tNumarator ün textinden gelen bilgi var mı dedik. Eğer varsa if bloğunu çalıştıracaktır.
                {
                    var urun = db.Urun.Where(a => a.Barkod == tNumarator.Text).FirstOrDefault(); //Database Urun tablosunda tNumarator den gelen texti urun değişkenine atadık.
                    UrunGetirListeye(urun, tNumarator.Text, Convert.ToDouble(tMiktar.Text));    //UrunGetirListeye() metodu içerisine parametreleri gönderdik.
                    tNumarator.Clear(); //tNumarator ün textini temizledik.
                    tBarkod.Focus(); //tBarkoda tekrar focusladık.
                }

                else
                {
                    MessageBox.Show("Urun Ekleme Sayfasını Aç");
                }
            }
        }

        private void ParaUstuHesapla_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender; //Butonu tanımladık 
            double sonuc = Islemler.DoubleYap(b.Text) - Islemler.DoubleYap(tGenelToplam.Text);  //Islemler Classı içerisinde tanımladığımız metodumuz içerine butonun textini ve tGenelToplam text ini gönderdik ve ikisinin farkını sonuc değişkenimize atadık.
            tOdenen.Text = Islemler.DoubleYap(b.Text).ToString("C2"); // b butonundan gelen bilgiyi Islemler Classı içerisindeki DoubleYap() metoduna gönderdik ve para türünden tOdenen textine yazdırdık.
            tParaUstu.Text = sonuc.ToString("C2"); // sonuc değişkenimizi para cinsinden olacak şekilde tParaUstu textine yazdırdık.
        }

        private void bDigerUrun_Click(object sender, EventArgs e)
        {
            if (tNumarator.Text != "") //Eğer numaratörün texti boş değil ise dedik.
            {
                int satirsayisi = gridSatisListesi.Rows.Count; // gridSatisListesi data gridinin satırlarını say ve satirsayisi değişkenine atama işlemini yap dedik.
                gridSatisListesi.Rows.Add(); //gridSatisListesi data gridine satır ekledik.
                gridSatisListesi.Rows[satirsayisi].Cells["Barkod"].Value = "1111111111116"; //satirsayisi satırında Barkod hücresinde değerini barkod tanımladık.
                gridSatisListesi.Rows[satirsayisi].Cells["UrunAdi"].Value = "Barkodsuz Ürün"; //satirsayisi satırında UrunAdi hücresinde adini tanımladık.
                gridSatisListesi.Rows[satirsayisi].Cells["UrunGrup"].Value = "Barkodsuz Ürün";  //satirsayisi satırında UrunGrup hücresinde adini tanımladık.
                gridSatisListesi.Rows[satirsayisi].Cells["Birim"].Value = "Adet";   //satirsayisi satırında Birim hücresinde birimini tanımladık.
                gridSatisListesi.Rows[satirsayisi].Cells["Miktar"].Value = 1; //satirsayisi satırında Miktar hücresinin değerini 1 yaptık.
                gridSatisListesi.Rows[satirsayisi].Cells["AlisFiyati"].Value = 0; // satirsayisi satırında AlisFiyati hücresinin değerini 0 yap dedik.
                gridSatisListesi.Rows[satirsayisi].Cells["Fiyat"].Value = Convert.ToDouble(tNumarator.Text); //satirsayisi satırında Fiyat hücresinde, tNumaratör gelen texti double türünde convert ettikten sonra tanımladık.
                gridSatisListesi.Rows[satirsayisi].Cells["KdvTutari"].Value = "0"; //satirsayisi satırında KdvTutari hücresinde 0 tanımladık.
                gridSatisListesi.Rows[satirsayisi].Cells["Toplam"].Value = Convert.ToDouble(tNumarator.Text);   //satirsayisi satırında Toplam hücresinde, tNumaratör gelen texti double türünde convert ettikten sonra tanımladık.
                tNumarator.Text = ""; //Satır işlemi bittiğinden tNumaratör textini boş olsun dedik.
                tBarkod.Focus(); //tBarkoda focusladık.
                GenelToplam();      //GenelToplam() metodunu çalıştırdık.       
            }
        }

        private void bIade_Click(object sender, EventArgs e)
        {
            if (chSatisIadeIslemi.Checked) //Eğer chSatisIadeIslemi seçiliyse
            {
                chSatisIadeIslemi.Checked = false; //chSatisIadeIslemi seçili olma işlemini false yap.
                chSatisIadeIslemi.Text = "Satis Yapılıyor"; //chSatisIadeIslemi textine yazı yazdık.
            }

            else   //Değilse
            {
                chSatisIadeIslemi.Checked = true; //chSatisIadeIslemi seçili olma işlemini true yap.
                chSatisIadeIslemi.Text = "İade İşlemi"; //chSatisIadeIslemi textine yazı yazdık.
            }

        }

        private void bTemizle_Click(object sender, EventArgs e)
        {

            Temizle();
        }

        private void Temizle()
        {
            tMiktar.Text = "1"; // tmiktarın textini 1 yaptık.
            tBarkod.Clear(); // tBarkodun textini temizle.
            tOdenen.Clear(); // tOdenen textini temizle.
            tParaUstu.Clear(); // tParaUstu textini temizle.
            tGenelToplam.Text = 0.ToString("C2"); //tGenelToplamın textini para cinsinden 0 yap dedik.
            chSatisIadeIslemi.Checked = false; // chSatisIadeIslemi checkboxunu tekrar false haline getir dedik.
            tNumarator.Clear(); // tNumarator textini temizle.
            gridSatisListesi.Rows.Clear(); //gridSatisListesinin satırlarını temizle dedik.
            tBarkod.Clear(); // tBarkod textini temizle.
            tBarkod.Focus();  //tBarkod a focusla dedik.

        }

        public void SatisYap(string odemesekli)
        {
            int satirsayisi = gridSatisListesi.Rows.Count;   //gridSatisListesi satırlarını say ve satirsayisi değişkenine atama işlemini yaptık. 
            bool satisiade = chSatisIadeIslemi.Checked; // chSatisIadeIslemi checkboxunun seçili durumdayken al ve satisiade değişkenine aktar dedik. 
            double alisfiyattoplam = 0; // alisfiyattoplam değişkenine 0 tanımladık;

            if (satirsayisi > 0) //satirsayisi 0 dan büyükse
            {
                int? islemno = db.Islem.First().IslemNo; // Database Islem tablosunda önce IslemNo değişkenini al ve islemno ya atama işlemini yaptık. Türünü int? ile 0 olabilir şeklinde tanımladık.
                Satis satis = new Satis(); //Satis modelimizi yeni bir değişken ile oluşturduk.

                for (int i = 0; i < satirsayisi; i++) //burada satirsayisi kadar işlem yaptıracağımız için döngü kullandık.
                {
                    satis.IslemNo = islemno; //islemno değişkenin içeriğini oluşturduğumuz veritabanı modelinden gelen satis tablosundaki IslemNo ya ekle dedik.
                    satis.UrunAd = gridSatisListesi.Rows[i].Cells["UrunAdi"].Value.ToString(); //gridSatisListesindeki i ninci satırındaki UrunAdi hücresinin değerini satis modelimizin UrunAd kısmına ekle dedik.
                    satis.UrunGrup = gridSatisListesi.Rows[i].Cells["UrunGrup"].Value.ToString();   //gridSatisListesindeki i ninci satırındaki UrunAdi hücresinin değerini satis modelimizin UrunGrup kısmına ekle dedik.
                    satis.Barkod = gridSatisListesi.Rows[i].Cells["Barkod"].Value.ToString();   //gridSatisListesindeki i ninci satırındaki Barkod hücresinin değerini satis modelimizin Barkod kısmına ekle dedik.
                    satis.Birim = gridSatisListesi.Rows[i].Cells["Birim"].Value.ToString(); //gridSatisListesindeki i ninci satırındaki Birim hücresinin değerini satis modelimizin Birim kısmına ekle dedik.
                    satis.AlisFiyat = Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["AlisFiyati"].Value.ToString()); // Islemler sınıfı içerisinde DoubleYap metodumuza parametre olarak gridSatisListesindeki i ninci satırındaki AlisFiyati hücresinin değerini satis modelimizin AlisFiyati kısmına ekle dedik.
                    satis.SatisFiyat = Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Fiyat"].Value.ToString());    // Islemler sınıfı içerisinde DoubleYap metodumuza parametre olarak gridSatisListesindeki i ninci satırındaki Fiyat hücresinin değerini satis modelimizin SatisFiyat kısmına ekle dedik.
                    satis.Miktar = Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString());    // Islemler sınıfı içerisinde DoubleYap metodumuza parametre olarak gridSatisListesindeki i ninci satırındaki Miktar hücresinin değerini satis modelimizin Miktar kısmına ekle dedik.
                    satis.Toplam = Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Toplam"].Value.ToString()); // Islemler sınıfı içerisinde DoubleYap metodumuza parametre olarak gridSatisListesindeki i ninci satırındaki Toplam hücresinin değerini satis modelimizin Toplam kısmına ekle dedik.
                    satis.KdvTutari = Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["KdvTutari"].Value.ToString()) * Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString()); //Islemler sınıfı içerisine KdvTutari ve Miktar değerlerini gönderdik ve çarptık. Daha sonra dönen değeri satis modelimizin KdvTutari kısmına ekledik.
                    satis.OdemeSekli = odemesekli;  // Metottan gelen odemesekli parametremizi satis modelimizin OdemeSekli kısmına gönderdik.
                    satis.Iade = satisiade; //Satis iadeden gelen bilgiyi satis modelimizin Iade kısmına aktardık.
                    satis.Tarih = DateTime.Now; //O an ki tarih ve saat bilgisini satis modelimizin Tarih kısmına gönderdik.
                    satis.Kullanici = lKullanici.Text; //lkullanicinin textini satis modelimizin Kullanici kısmına gönderdik.
                    db.Satis.Add(satis); //Database içerisindeki satis tablosuna satis modelini ekledik.
                    db.SaveChanges(); //Database üzerinde yapılan değişiklikleri kaydedip güncelledik.
                    if (!satisiade) //Eğer satisiade değilse dedik
                    {
                        Islemler.StokAzalt(gridSatisListesi.Rows[i].Cells["Barkod"].Value.ToString(), Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString())); //Islemler Classı içerisindeki StokAzalt() metodumuzu çağırdık.

                    }
                    else
                    {
                        Islemler.StokArtir(gridSatisListesi.Rows[i].Cells["Barkod"].Value.ToString(), Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString()));  //Islemler Classı içerisindeki StokArtir() metodumuzu çağırdık.
                    }

                    alisfiyattoplam += Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["AlisFiyati"].Value.ToString()); //İslemler Classı içerisindeki DoubleYap() metodu içerisinde gridSatislistesi data gridinin i ninci satırındaki AlisFiyati hücresinin değerini string yap ve alisfiyattoplam değişkenine ata dedik.

                }

                IslemOzet io = new IslemOzet(); //IslemOzet modelimizi bir değişkene çağırdık;
                io.IslemNo = islemno; // islemno değişkeninden gelen bilgiyi modelimizin IslemNo değişkenine aktardık.
                io.Iade = satisiade; //satisiade değişkeninden gelen bilgiyi modelimizin Iade değişkenine aktardık.
                io.AlisFiyatToplam = alisfiyattoplam; //alisfiyattoplam dan gelen bilgiyi modelimizdeki değişkenimize aktardık.
                io.Gelir = false;
                io.Gider = false;

                if (!satisiade)
                {
                    io.Aciklama = odemesekli + " Satış"; // odemesekli paramatresindeki bilgiyi modelimizin Aciklama kısmına aktardık.
                }
                else
                {
                    io.Aciklama = "İade işlemi (" + odemesekli + ")";
                }
                io.OdemeSekli = odemesekli; //odemesekli parametresinden gelen bilgiyi modelimiz ile ilişkilendirdik.
                io.Kullanici = lKullanici.Text; //lkullanici label textini modelin Kullanici bölümüne gönderdik.
                io.Tarih = DateTime.Now; //O anda gerçek tarih ve zamanı al ve modelin Tarih kısmına gönderdik.
                switch (odemesekli)    //switch bloğuna şart olarak odemesekli ibaresi bilgisiyse dedik.
                {
                    case "Nakit":   //Gelen bilgi Nakit se
                        io.Nakit = Islemler.DoubleYap(tGenelToplam.Text); // Islemler Classı içerisinde DoubleYap() metodunda tGenelToplam ın textini çalıştır ve modelin Nakit kısmına aktar dedik.
                        io.Kart = 0; break;  //Durum 0 sa Kart kısmına gönderip işlemden çık dedik.
                    case "Kart":   //Gelen bilgi Kart sa
                        io.Nakit = 0;   // 0 değerini modelimizin Nakit kısmında güncelledik.
                        io.Kart = Islemler.DoubleYap(tGenelToplam.Text); break; // Islemler Classı içerisinde DoubleYap() metodunda tGenelToplam ın textini çalıştır ve modelimizin Kart kısmına aktardık.
                    case "Kart-Nakit":
                        io.Nakit = Islemler.DoubleYap(lNakit.Text);
                        io.Kart = Islemler.DoubleYap(lKart.Text); break;

                }
                db.IslemOzet.Add(io); // Database IslemOzet içerisine io dan gelen bilgiyi ekledik.
                db.SaveChanges(); //Database de yapılan değişiklikleri kaydedip güncelledik.

                var islemnoartir = db.Islem.First(); //Database Islem deki ilk satırı al ve islemnoartir değişkenine ata dedik.
                islemnoartir.IslemNo += 1; // islemnoartir içerisinde IslemNo yu 1 artır dedik.
                db.SaveChanges();   //Database de yapılan değişiklikleri kaydedip güncelledik.
                MessageBox.Show("Yazdırma İşlemini Yap");
                Temizle();  //Temizle metodumuzu çağırdık.

            }
        }

        private void bNakit_Click(object sender, EventArgs e)
        {
            SatisYap("Nakit"); //bNakit butonu click edildiğinde SatisYap metodu içerisine Nakit parametresini gönderdik.
        }

        private void bKart_Click(object sender, EventArgs e)
        {
            SatisYap("Kart");
        }

        private void bKartNakit_Click(object sender, EventArgs e)
        {
            fNakitKart f = new fNakitKart(); //fNakitKart form sayfamızı oluşturduk f değişkeni ile çağırdık.
            f.ShowDialog(); //Formu gösterdik.
        }

        private void tBarkod_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != (char)08) //Burada klavyeden karakter almayı kapattık ve sadece rakama izin veriyoruz.
            {
                e.Handled = true;
            }
        }

        private void tMiktar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != (char)08) //Burada klavyeden karakter almayı kapattık ve sadece rakama izin veriyoruz.
            {
                e.Handled = true;
            }
        }

        private void fSatis_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)   // F1 tuşuna basıldığında işlem yaptırmak için tanımladık.
                      SatisYap("Nakit");
            if (e.KeyCode == Keys.F2)   // F2 tuşuna basıldığında işlem yaptırmak için tanımladık.
                      SatisYap("Kart");
            if (e.KeyCode == Keys.F3)   // F3 tuşuna basıldığında işlem yaptırmak için tanımladık.
            {
                fNakitKart f = new fNakitKart(); //fNakitKart form sayfamızı oluşturduk f değişkeni ile çağırdık.
                f.ShowDialog(); //Formu gösterdik.
            }
        }

        private void bIslemBeklet_Click(object sender, EventArgs e)
        {
            if(bIslemBeklet.Text== "İşlem Beklet")  //Eğer bIslemBeklet texti eşit İşlem Beklet ise dedik.
            {
                Bekle();
                bIslemBeklet.BackColor = System.Drawing.Color.OrangeRed;  //bIslemBeklet in backcolorunu Orange Red rengi yap dedik.
                bIslemBeklet.Text = "İşlem Bekliyor"; // bIslemBeklet textine İşlem Bekliyor yazdırdık.
                gridSatisListesi.Rows.Clear();     // gridSatisListesi nin satırlarını temizledik.       
            }
            else
            {
                BeklemedenCik(); //BeklemedenCik() metodumuzu çalıştırdık.
                bIslemBeklet.BackColor = System.Drawing.Color.DimGray;  //bIslemBeklet in backcolorunu DimGray rengi yap dedik.
                bIslemBeklet.Text = "İşlem Beklet";   // bIslemBeklet textine İşlem Beklet yazdırdık.
                gridBekle.Rows.Clear();  // gridBekle data gridin satirlarini temizle dedik.
            }
            
        }

        private void Bekle()
        {
            int satir = gridSatisListesi.Rows.Count;  //gridSatisListesi data gridinin satırlarını say satir değişkenine ata dedik.
            int sutun = gridSatisListesi.Columns.Count;   //gridSatisListesi data gridinin sutunlarını say sutun değişkenine ata dedik.  
            if (satir > 0) //satir büyükse 0 dan dedik.
            {
                for(int i=0; i<satir; i++)  // i değeri 0 dan başlayarak satir değerine kadar 1 artacaktır.
                {
                    gridBekle.Rows.Add();
                    for (int j=0; j<sutun-1; j++)  // j değeri 0 dan başlayarak sutun değerine kadar 1 artacaktır. En sondaki sutunu istemediğimiz için sutundan 1 çıkartarak işleme başlıyoruz.
                    {
                        gridBekle.Rows[i].Cells[j].Value = gridSatisListesi.Rows[i].Cells[j].Value;     // gridSatisListesi nin ilgili satır ve sütunlarını gridBekle nin satır ve sütunlarına ekledik.
                    }
                }
            }
        }
    
        private void BeklemedenCik()
        {
            int satir = gridBekle.Rows.Count;  //gridBekle data gridinin satırlarını say satir değişkenine ata dedik.
            int sutun = gridBekle.Columns.Count;   //gridBekle data gridinin sutunlarını say sutun değişkenine ata dedik.  
            if (satir > 0) //satir büyükse 0 dan dedik.
            {
                for (int i = 0; i < satir; i++)  // i değeri 0 dan başlayarak satir değerine kadar 1 artacaktır.
                {
                    gridSatisListesi.Rows.Add();   //gridSatisListesi ne satir ekle dedik.
                    for (int j = 0; j < sutun - 1; j++)  // j değeri 0 dan başlayarak sutun değerine kadar 1 artacaktır. En sondaki sutunu istemediğimiz için sutundan 1 çıkartarak işleme başlıyoruz.
                    {
                        gridSatisListesi.Rows[i].Cells[j].Value = gridBekle.Rows[i].Cells[j].Value; // gridBekle nin ilgili satır ve sütunlarını gridSatisListesi nin satır ve sütunlarına ekledik.
                    }
                }
            }
        }
    }
}
