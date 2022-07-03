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
    public partial class fHizliButonUrunEkle : Form
    {
        public fHizliButonUrunEkle()
        {
            InitializeComponent();
        }

        BarkodDbEntities db = new BarkodDbEntities();  //Database tanımlamasını yaptım.

        private void tUrunAra_TextChanged(object sender, EventArgs e)
        {
            if (tUrunAra.Text!="") //Eğer tUrunAra texti boştan farklı ise
            {
                string urunad = tUrunAra.Text; // tUrunAra textbox içeriğini urunad değişkenine atadım.
                var urunler = db.Urun.Where(a => a.UrunAd.Contains(urunad)).ToList(); //Database urunler içerisinde Contains() fonksiyonu kullanılarak harflere göre
                                                                                      //arama yaptıracak ve urunler değişkenine içeriği listeleyerek aktaracak.
                gridUrunler.DataSource = urunler;  // urunler değişkeninden gelen bilgiyi gridUrunler data gridine aktardık.
                Islemler.GridDuzenle(gridUrunler); //Islemler class ı içerisinde GridDuzenle metodunda gridUrunler parametresi ile çalıştırdık.
            }
        }

        private void fHizliButonUrunEkle_Load(object sender, EventArgs e)
        {
            
        }

        private void gridUrunler_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gridUrunler.Rows.Count > 0)  // Eğer gridUrunler in satır sayısı 0 dan fazla ise
            {
                string barkod = gridUrunler.CurrentRow.Cells["Barkod"].Value.ToString(); //gridUrunler seçili sütunundan barkod hücresinin değerini barkod değişkenine atadım.
                string urunad = gridUrunler.CurrentRow.Cells["UrunAd"].Value.ToString();
                double fiyat = Convert.ToDouble(gridUrunler.CurrentRow.Cells["SatisFiyat"].Value.ToString());
                int id = Convert.ToInt16(lButonId.Text); // lbutonId nin textini id ye atadım.
                var guncellenecek = db.HizliUrun.Find(id); //Database de HizliUrun tablosunda Find() fonksiyonu ile birincil anahtarda arama işlemi ilgili id ile aratıyoruz.
                guncellenecek.Barkod = barkod;  //Güncelleme yapılması için değişkenlerin üzerine yeni bilgiyi yazdık.
                guncellenecek.UrunAd = urunad;
                guncellenecek.Fiyat = fiyat;
                db.SaveChanges();   //Database imizi güncelleyip kaydettik.
                MessageBox.Show("Buton tanımlanmıştır"); //Buton tanımlandıktan sonra mesaj verdirdik.
                fSatis f = (fSatis)Application.OpenForms["fSatis"]; //Açılmış bir form olduğu ve fSatis formunda işlem yapmak için tanımladık.
                if (f != null)  //Eğer f nulldan farklı ise dedik.
                {
                    Button b = f.Controls.Find("bH" + id, true).FirstOrDefault() as Button; //b adında buton değişkeni tanımladık. f kontrolleri içerisinde bH ve buton id sini bul ve b butonuna ata dedik.
                    b.Text = urunad + "\n" + fiyat.ToString("C2"); //b butonunun text ine  urun adını ve fiyat TL cinsinde yazdırdık.
                }

            }
        }

        private void chTumu_CheckedChanged(object sender, EventArgs e)
        {
            if(chTumu.Checked)   //Eğer chTumu checked edilmiş ise 
            {
                gridUrunler.DataSource = db.Urun.ToList();   //gridUrunler data source database de Urun tablosunu listeledik.
                gridUrunler.Columns["AlisFiyat"].Visible = false;
                gridUrunler.Columns["SatisFiyat"].Visible = false;
                gridUrunler.Columns["KdvOrani"].Visible = false;
                gridUrunler.Columns["KdvTutari"].Visible = false;
                gridUrunler.Columns["Miktar"].Visible = false;
                Islemler.GridDuzenle(gridUrunler); //Islemler class ı içerisinde GridDuzenle metodunda gridUrunler parametresi ile çalıştırdık.
            }

            else  //checked edilmemiş ise
            {
                gridUrunler.DataSource = null;    //gridUrunlerin eğer data source ünü null yap dedik.
            }
           
        }
    }
}
