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
    public partial class fNakitKart : Form
    {
        public fNakitKart()
        {
            InitializeComponent();
        }

        private void fNakitKart_Load(object sender, EventArgs e)
        {

        }

        private void tNakit_KeyDown(object sender, KeyEventArgs e)
        {
            if (tNakit.Text != "") //Şart olarak eğer tNakit text i boş değilse dedik
            {
                if (e.KeyCode == Keys.Enter) // Enter e basıldığında
                {
                    Hesapla(); //Hesapla() metodumuzu çağırdık.
                }
            }
        }

        private void Hesapla()
        {
            fSatis f = (fSatis)Application.OpenForms["fSatis"]; //Açılmış fSatis formuna f değişkeni ile erişmek için atadık.
            double nakit = Islemler.DoubleYap(tNakit.Text); //Islemler Class ında DoubleYap() metodumuza tNakit textbox dan gelen bilgiyi gönderdik ve nakit değişkenine atadık.
            double geneltoplam = Islemler.DoubleYap(f.tGenelToplam.Text); //Islemler Class ında DoubleYap() metodumuza f formu ile erişime açtığımız tGenelToplam textbox dan gelen bilgiyi gönderdik ve geneltoplam değişkenine atadık.
            double kart = geneltoplam - nakit; // geneltoplam ve nakit değişkenlerinin farkını alarak yeni mevcut değerimizi kart değişkenine atadık.
            f.lNakit.Text = nakit.ToString("C2"); //nakit bilgisini para formatında f ile eriştiğimiz form içerisinde lNakit in textine gönderdik.
            f.lKart.Text = kart.ToString("C2"); //kart bilgisini para formatında f ile eriştiğimiz form içerisinde lKart ın textine gönderdik.
            f.SatisYap("Kart-Nakit"); //f formunda SatisYap() metodunda Kart-Nakit e gönderdik.
            this.Hide(); //Pencereyi entere bastıktan sonra kapatma işlemini yaptık.
        }

        private void bNx_Click(object sender, EventArgs e) //private türünde bir metod tanımladık.Bütün butonlar bu metod ile çalışacak.
        {
            Button b = (Button)sender; //Gelecek obje yani nesnemiz butondur dedik ve atadık.

            if (b.Text == ",")  //Eğer b texti "," ise dedik.
            {
                int virgul = tNakit.Text.Count(x => x == ','); //tNakit de girilen virgül sayısını say ve virgul değişkenine ata dedik.
                if (virgul < 1)   //Eğer virgül sayısı 1 den küçükse
                {
                    tNakit.Text += b.Text;   // b objesinden gelen içeriği numaratörde güncelledik.
                }
            }

            else if (b.Text == "<")   // Eğer b texti "<" işaretine eşit ise
            {
                if (tNakit.Text.Length > 0)   //Eğer tNakit ün texti 0 dan büyükse
                {
                    tNakit.Text = tNakit.Text.Substring(0, tNakit.Text.Length - 1); //tNakit in textini al 0 dan başla ve tNakit in uzunluğundan 2 çıkart bunuda tNumarator ün textinde güncelle dedik.

                }
            }

            else
            {
                tNakit.Text += b.Text; // b objesinden gelen içeriği numaratörde güncelledik.
            }

        }

        private void bEnter_Click(object sender, EventArgs e)
        {
            if(tNakit.Text != "") //Eğer tNakit in texti boş değilse
            {
                Hesapla(); //Hesapla() metodumuzu çağırdık.
            }
        }

        private void tNakit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar !=(char)08) //Burada klavyeden karakter almayı kapattık ve sadece rakama izin veriyoruz.
            {
                e.Handled = true;               
            }
        }
    }
}
