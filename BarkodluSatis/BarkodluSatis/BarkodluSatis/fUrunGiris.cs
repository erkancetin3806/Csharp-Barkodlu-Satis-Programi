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
    public partial class fUrunGiris : Form
    {
        public fUrunGiris()
        {
            InitializeComponent();
        }

        BarkodDbEntities db = new BarkodDbEntities(); //Database imizi çağırdık bir değişken ile
        private void tBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Eğer anahtar kodu enter sa dedik.
            {
                string barkod = tBarkod.Text.Trim(); //tBarkod un textini başında ve sonundaki boşlukları aldıktan sonra barkod değişkenine temiz bir barkod değerini gönderdik.
                if (db.Urun.Any(a => a.Barkod == barkod)) //Eğer database içerisinde barkod değerini gönderdik varmı diye sorguladık.
                {
                   var urun = db.Urun.Where(a=> a.Barkod == barkod).SingleOrDefault(); //DatabaseUrun tablosunda barkodu buluyoruz.   
                    // İlgili Barkodun özelliklerini veritabanından arayacak ve textlerine yazacak.
                    tUrunAdi.Text = urun.UrunAd;
                    tAciklama.Text = urun.Aciklama;
                    cmbUrunGrubu.Text = urun.UrunGrup;
                    tAlisFiyati.Text = urun.AlisFiyat.ToString();
                    tSatisFiyati.Text = urun.SatisFiyat.ToString();
                    tMiktar.Text = urun.Miktar.ToString();
                    tKdvOrani.Text = urun.KdvOrani.ToString();
                }
                else
                {
                    MessageBox.Show("Ürün kayıtlı değil, kaydedebilirsiniz...");
                }
            }
        }

        private void bKaydet_Click(object sender, EventArgs e)
        {
            if (tBarkod.Text != "" && tUrunAdi.Text != "" && cmbUrunGrubu.Text != "" && tAlisFiyati.Text != "" && tSatisFiyati.Text != "" && tKdvOrani.Text != "" && tMiktar.Text != "") //Eğer değişkenlerdeki değerler boş değilse
            {
                if (db.Urun.Any(a => a.Barkod == tBarkod.Text)) //Eğer database de tBarkod textinden gelen bilgi varsa
                {
                    var guncelle = db.Urun.Where(a => a.Barkod == tBarkod.Text).SingleOrDefault(); //Database de Urun tablosunda tBarkod textini aradık ve guncelleye atadık.
                    guncelle.UrunAd = tUrunAdi.Text;     //tUrunAdi textini databaseye aktardık.
                    guncelle.Aciklama = tAciklama.Text;  //tAciklama textini databaseye aktardık.
                    guncelle.UrunGrup = cmbUrunGrubu.Text;   //cmbUrunGrubu textini databaseye aktardık.
                    guncelle.AlisFiyat = Convert.ToDouble(tAlisFiyati.Text);    //tAlisFiyati textini databaseye aktardık.
                    guncelle.SatisFiyat = Convert.ToDouble(tSatisFiyati.Text);  //tSatisFiyati textini databaseye aktardık.
                    guncelle.KdvOrani = Convert.ToInt32(tKdvOrani.Text);    //tKdvOrani textini databaseye aktardık.
                    guncelle.KdvTutari = Math.Round(Islemler.DoubleYap(tSatisFiyati.Text) * Convert.ToInt32(tKdvOrani.Text) / 100, 2); //tSatisFiyati double ve tKdvOrani int türünden çarparak çıkan değerleri virgülden sonra 2 hane olacak şekilde databaseye aktardık.
                    guncelle.Miktar += Convert.ToDouble(tMiktar.Text);   //tMiktar textini database üzerinde eklemelerini yaptık ve aktardık.
                    guncelle.Birim = "Adet";    //Adet ifadesini databaseye aktardık.
                    guncelle.Tarih = DateTime.Now; //Tarih ve saat o andaki bilgisini textini databaseye aktardık.
                    guncelle.Kullanici = lKullanici.Text;  //lKullanici textini databaseye aktardık.
                    db.SaveChanges();  // Database imizi güncelledik.
                    gridUrunler.DataSource = db.Urun.OrderByDescending(a => a.UrunId).Take(10).ToList(); // gridUrunlerin data source ünde veritabanında Urunler tablosunu tersten ilk 10 satırını listele dedik.            
                }
                else
                {
                    Urun urun = new Urun();         // Urun modelimizden bir örnek değişken tanımladık.
                    urun.Barkod = tBarkod.Text;     //tBarkod textini modelimize aktardık.
                    urun.UrunAd = tUrunAdi.Text;     //tUrunAdi textini modelimize aktardık.
                    urun.Aciklama = tAciklama.Text;  //tAciklama textini modelimize aktardık.
                    urun.UrunGrup = cmbUrunGrubu.Text;   //cmbUrunGrubu textini modelimize aktardık.
                    urun.AlisFiyat = Convert.ToDouble(tAlisFiyati.Text);    //tAlisFiyati textini modelimize aktardık.
                    urun.SatisFiyat = Convert.ToDouble(tSatisFiyati.Text);  //tSatisFiyati textini modelimize aktardık.
                    urun.KdvOrani = Convert.ToInt32(tKdvOrani.Text);    //tKdvOrani textini modelimize aktardık.
                    urun.KdvTutari = Math.Round(Islemler.DoubleYap(tSatisFiyati.Text) * Convert.ToInt32(tKdvOrani.Text) / 100, 2); //tSatisFiyati double ve tKdvOrani int türünden çarparak çıkan değerleri virgülden sonra 2 hane olacak şekilde modelimize aktardık.
                    urun.Miktar = Convert.ToDouble(tMiktar.Text);   //tMiktar textini modelimize aktardık.
                    urun.Birim = "Adet";    //Adet ifadesini modelimize aktardık.
                    urun.Tarih = DateTime.Now; //Tarih ve saat o andaki bilgisini textini modelimize aktardık.
                    urun.Kullanici = lKullanici.Text;  //lKullanici textini modelimize aktardık.
                    db.Urun.Add(urun); // Database Urun tablosuna urun modelinden gelen değişikleri ekle dedik.
                    db.SaveChanges(); // Database kaydedip güncelledik.
                    if(tBarkod.Text.Length == 8)
                    {
                        var ozelbarkod = db.Barkod.First(); //Databasede Barkod tablosunun ilkini aldık ve değişkene atadık.
                        ozelbarkod.BarkodNo += 1;   //ozelbarkod databesinde BarkodNo yu 1 artırdık.
                        db.SaveChanges();   //Databasede kaydedip güncelledik.
                    }
                    
                    gridUrunler.DataSource = db.Urun.OrderByDescending(a => a.UrunId).Take(20).ToList(); // gridUrunlerin data source ünde veritabanında Urunler tablosunu tersten ilk 10 satırını listele dedik.                             
                    Islemler.GridDuzenle(gridUrunler); //Islemler class ı içerisinde GridDuzenle metodunda gridUrunler parametresi ile çalıştırdık.
                }
                Islemler.StokHareket(tBarkod.Text, tUrunAdi.Text, "Adet", Convert.ToDouble(tMiktar.Text), cmbUrunGrubu.Text, lKullanici.Text);
                Temizle(); // Metodumuzu Çağırdık.
            }
            else
            {
                MessageBox.Show("Bilgi girişlerini kontrol ediniz...");
                tBarkod.Focus(); //tBarkod textine focusladık.
            }
        }

        private void tUrunAra_TextChanged(object sender, EventArgs e)
        {
            if (tUrunAra.Text.Length>=2)  //Eğer tUrunAra textini ilk 2 karakterden sonra veya büyükse ara dedik.
            {
                string urunad = tUrunAra.Text; //tUrunAra nın textini string türündeki urunad değişkenine atadık.
                gridUrunler.DataSource = db.Urun.Where(a => a.UrunAd.Contains(urunad)).ToList(); // gridUrunlerin data source üne database Urun tablosunda UrunAd a urunad ın textini bulunduruyorsa listele dedik.
                Islemler.GridDuzenle(gridUrunler);  //Islemler class ı içerisinde GridDuzenle metodunda gridUrunler parametresi ile çalıştırdık.
            }
        }

        private void bIptal_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void Temizle()
        {
            tBarkod.Clear();   //tBarkod textbox ını temizledik.
            tUrunAdi.Clear();   //tUrunAdi textbox ını temizledik.
            tAciklama.Clear();  //tAciklama textbox ını temizledik.
            tAlisFiyati.Text = "0"; //tAlisFiyati textbox ını 0 yaptık.
            tSatisFiyati.Text = "0";    //tSatisFiyati textbox ını 0 yaptık.
            tMiktar.Text = "0";   //tMiktar textbox ını 0 yaptık.
            tKdvOrani.Text = "8";   //tKdvOrani textbox ını 8 yaptık.
            tBarkod.Focus();    //tBarkod textine focusladık.
        }

        private void fUrunGiris_Load(object sender, EventArgs e)
        {
            tUrunSayisi.Text = db.Urun.Count().ToString(); //Form yüklendiğinde database Urun tablosunun sayısını tUrunSayisi nin textine yazdırdık.
            gridUrunler.DataSource = db.Urun.OrderByDescending(a => a.UrunId).Take(20).ToList(); // gridUrunlerin data source ünde veritabanında Urunler tablosunu tersten ilk 10 satırını listele dedik.
            Islemler.GridDuzenle(gridUrunler); //Islemler Class ın da GridDuzenle metodunu gridUrunler parametresi ile çalıştırdık.
            GrupDoldur(); //Metodumuzu çağırdık.
        }

        public void GrupDoldur()
        {
            cmbUrunGrubu.DisplayMember = "UrunGrupAd"; //listUrunGrup listBox ında görünen değeri UrunGrupAd olarak ayarladık.
            cmbUrunGrubu.ValueMember = "Id";
            cmbUrunGrubu.DataSource = db.UrunGrup.OrderBy(a => a.UrunGrupAd).ToList(); //listUrunGrup listBox ının data source database UrunGrup elemanlarını a-z sıralaması ile listele dedik.
        }
        
        private void bUrunGrubuEkle_Click(object sender, EventArgs e)
        {
            fUrunGrubuEkle f = new fUrunGrubuEkle(); // fUrunGrubuEkle formunu değişkene atadık.
            f.ShowDialog();  // f değişkeninden gelen çağırdığımız formumuzu göster dedik.
        }

        private void bBarkodOlustur_Click(object sender, EventArgs e)
        {
            var barkodno = db.Barkod.First(); // Database Barkod tablosunun birincisini barkodno değişkenine ata dedik.
            int karakter = barkodno.BarkodNo.ToString().Length; // barkodno tablosundaki karakter sayısını bulabilmek için karakter değişkenine atadık.
            string sifirlar = string.Empty; // Kaç tane sıfır olacaksa sifirlar değişkenine atadık.
            for (int i=0; i < 8-karakter; i++)  // for döngüsü oluşturuyoruz 0 dan başlayacak ve karakter sayısından 8 çıkaracak i bir bir artacak.
            {
                sifirlar = sifirlar + "0";  //sifirlarin sayisinin yanina bir sifir daha ekledik. 
            }
            string olusanbarkod = sifirlar + barkodno.BarkodNo.ToString(); // sifirlar değişkenine barkodno database inde BarkodNo tablosunun ilkini ekledik. O nun değerini 1 di zaten.
            tBarkod.Text = olusanbarkod;  // olusanbarkod text bilgisini tBarkod.textine at dedik.,
            tUrunAdi.Focus(); 
        }

        private void tSatisFiyati_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(char.IsDigit(e.KeyChar) == false && e.KeyChar!=(char)08 && e.KeyChar!=(char)44 && e.KeyChar!=(char)45)
            {
                e.Handled = true;
            }
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (gridUrunler.Rows.Count > 0)
            {
                int urunid = Convert.ToInt32(gridUrunler.CurrentRow.Cells["UrunId"].Value.ToString()); //gridUrunler in geçerli satırındaki UrunId string değerini int türünden urunid değişkenimize atadık.
                string urunad = gridUrunler.CurrentRow.Cells["UrunAd"].Value.ToString();    //gridUrunler in geçerli satırındaki UrunAd string değerini urunad değişkenimize atadık.
                string barkod = gridUrunler.CurrentRow.Cells["Barkod"].Value.ToString();
                DialogResult onay = MessageBox.Show(urunad + " ürününü silmek istiyor musunuz?", "Ürün Silme İşlemi", MessageBoxButtons.YesNo);
                if (onay == DialogResult.Yes)
                {
                    var urun = db.Urun.Find(urunid);
                    db.Urun.Remove(urun);
                    db.SaveChanges();
                    var hizliurun = db.HizliUrun.Where(x => x.Barkod == barkod).SingleOrDefault();
                    hizliurun.Barkod = "-";
                    hizliurun.UrunAd = "-";
                    hizliurun.Fiyat = 0;
                    db.SaveChanges();
                    MessageBox.Show("Ürün silinmiştir");
                    gridUrunler.DataSource = db.Urun.OrderByDescending(a => a.UrunId).Take(20).ToList(); // gridUrunlerin data source ünde veritabanında Urunler tablosunu tersten ilk 10 satırını listele dedik.
                    Islemler.GridDuzenle(gridUrunler); //Islemler Class ın da GridDuzenle metodunu gridUrunler parametresi ile çalıştırdık.
                    tBarkod.Focus();
                }
            }
            
        }
    }
}
