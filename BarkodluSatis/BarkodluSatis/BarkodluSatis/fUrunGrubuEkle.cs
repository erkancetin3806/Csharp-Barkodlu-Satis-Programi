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
    public partial class fUrunGrubuEkle : Form
    {
        public fUrunGrubuEkle()
        {
            InitializeComponent();
        }

        BarkodDbEntities db = new BarkodDbEntities();  //Database değişkenimizi oluşturduk. 

        private void fUrunGrubuEkle_Load(object sender, EventArgs e)
        {
            GrupDoldur();
        }

        private void bEkle_Click(object sender, EventArgs e) //bEkle butonu click
        {
            if (tUrunGrupAd.Text != "")  //Eğer tUrunGrupAd boş değil ise dedik.
            {
                UrunGrup ug = new UrunGrup();  //UrunGrup modelimizi çağırdık.
                ug.UrunGrupAd = tUrunGrupAd.Text;  // tUrunGrupAd textini modelimizin UrunGrupAd değişkenine aktardık.
                db.UrunGrup.Add(ug);    // Yaptığımız değişiklikleri database UrunGrup tablomuza ug modeli üstünden ekledik.
                db.SaveChanges();   //Database üzerinde yapılan değişiklikleri kaydedip güncelledik.
                GrupDoldur();   //Metodumuzu çağırdık.
                tUrunGrupAd.Clear(); // tUrunGrupAd textini temizle dedik.
                MessageBox.Show("Ürün grubu eklenmiştir");
                fUrunGiris f = (fUrunGiris)Application.OpenForms["fUrunGiris"]; //Açılmış bir forma tekrar ulaşabilmemiz için ilgili kodlarımızı yazdık.
                if(f != null)    // Eğer f formu boş değil ise dedik.
                {
                    f.GrupDoldur(); // f formunda GrupDoldur() metodunu çağırdık.
                }                
            }
            else
            {
                MessageBox.Show("Ürün bilgisi ekleyiniz");
            }
        }

        private void GrupDoldur()
        {
            listUrunGrup.DisplayMember = "UrunGrupAd"; //listUrunGrup listBox ında görünen değeri UrunGrupAd olarak ayarladık.
            listUrunGrup.ValueMember = "Id";
            listUrunGrup.DataSource = db.UrunGrup.OrderBy(a => a.UrunGrupAd).ToList(); //listUrunGrup listBox ının data source database UrunGrup elemanlarını a-z sıralaması ile listele dedik.
        }

        private void bSil_Click(object sender, EventArgs e)
        {
            int grupid = Convert.ToInt32(listUrunGrup.SelectedValue.ToString()); // listUrunGrup listBox ında seçili değerin string değerini al ve grupid ata dedik.
            string grupad = listUrunGrup.Text;   //listUrunGrup textini grupad string değişkenine atadık.
            DialogResult onay = MessageBox.Show(grupad + " grubunu silmek istiyor musunuz?","Silme İşlemi",MessageBoxButtons.YesNo); // onay değişkenine bilgi gelecek şekilde Evet ve Hayır butonlarından oluşan DialogResult oluşturduk. 
            if (onay == DialogResult.Yes)  //Eğer DialogResult Evet seçiliyse 
            {
                var grup = db.UrunGrup.FirstOrDefault(x => x.Id == grupid);  //Database UrunGrup tablosunda birincinin içerisinde grupid bul dedik ve var tipinde grup değişkenine atadık.
                db.UrunGrup.Remove(grup);  //Database UrunGrup tablosunda grup değişkeninden gelen bilgiyi kaldır dedik.
                db.SaveChanges();  //Database yapılan değişikleri kaydet ve güncelle dedik.
                GrupDoldur();    //Grup doldur metodumuzu çağırdık.
                tUrunGrupAd.Focus();   //tUrunGrupAd textbox ına focusladık.
                MessageBox.Show(grupad + " ürün grubu silindi");   //İşlem sonucunda bize bir bilgi vermesi için mesaj yazdırdık.
                fUrunGiris f = (fUrunGiris)Application.OpenForms["fUrunGiris"]; //Açılmış bir forma tekrar erişmek için ilgili kodun yazımını kullandık ve f değişkenimize atadık.
                f.GrupDoldur(); // f formunda GrupDoldur() metodunu çalıştırdık.
            }
        }
    }
}
