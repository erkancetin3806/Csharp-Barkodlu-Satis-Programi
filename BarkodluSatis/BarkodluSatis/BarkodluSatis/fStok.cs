using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarkodluSatis
{
    public partial class fStok : Form
    {
        public fStok()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void bAra_Click(object sender, EventArgs e)
        {
            gridListe.DataSource = null;   //gridListe data gridimizin ilk olarak data source ünü null yapıyoruz.
            using (var db = new BarkodDbEntities())   //BarkodbEntities database ini db değişkeninde kullanarak diyoruz. 
            {
                if(cmbIslemTuru.Text != "")  //Eğer cmbIslemTuru comboBox ı boş değilse dedik.
                {
                    string urungrubu = cmbUrunGrubu.Text; //cmbUrunGrubu textini string türünde urungrubu değişkenine atadık.
                    if (cmbIslemTuru.SelectedIndex == 0) //Eğer cmbIslemTuru comboBoxının 1. objesi yani 0. indexi seçili durumdaysa işlem yaptıracağız.
                    {
                        if(rdTumu.Checked)   //Eğer rdTumu radioButton seçili ise
                        {
                            db.Urun.OrderBy(x => x.Miktar).Load(); //Database içerisinde Urun tablosunda Miktar sütununu sıralamasını istedik.Load() başlığa tıkladığımızda sıralama yaptırsın diye metodu tanımladık.
                            gridListe.DataSource = db.Urun.Local.ToBindingList(); //gridListe nin data source üne database içerisindeki Urun tablosunu listeleme işlemini yaptık.
                        }
                        else if (rdUrunGrubunaGore.Checked)   //Eğer rdUrunGrubunaGore radioButton seçili ise
                        {
                            db.Urun.Where(x => x.UrunGrup == urungrubu).OrderBy(x => x.Miktar).Load();  // Database de Urun tablosunda urungrubu textinden gelen bilgi UrunGrup sütununda eşitliği sağlıyorsa Miktar da sıralama yap ve yükle dedik.
                            gridListe.DataSource = db.Urun.Local.ToBindingList(); //gridListe nin data source üne database içerisindeki Urun tablosunu listeleme işlemini yaptık.   
                        }
                    }

                    else if (cmbIslemTuru.SelectedIndex == 1)   //Eğer cmbIslemTuru comboBoxının 2. objesi yani 1. indexi seçili durumdaysa işlem yaptıracağız.
                    {
                        DateTime baslangic = DateTime.Parse(dateBaslangic.Value.ToShortDateString());
                        DateTime bitis = DateTime.Parse(dateBaslangic.Value.ToShortDateString());
                        if (rdTumu.Checked)
                        {
                           
                        }

                    }
                }
            }
        }


        BarkodDbEntities dbx = new BarkodDbEntities();  //Veritabanı modelimizi dbx de tanımladık.
        private void fStok_Load(object sender, EventArgs e)
        {
            cmbUrunGrubu.DisplayMember = "UrunGrupAd";  // cmbUrunGrubu görünen değerine UrunGrupAd olsun dedik.
            cmbUrunGrubu.ValueMember = "Id";  //cmbUrunGrubu değeri Id olsun dedik.
            cmbUrunGrubu.DataSource = dbx.UrunGrup.ToList();   //cmbUrunGrubu data source ünde dbx database içerisinde UrunGrup tablosunu listeleyeceğiz.
        }
    }
}
