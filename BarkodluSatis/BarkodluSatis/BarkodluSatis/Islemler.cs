using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarkodluSatis
{
    static class Islemler
    {
        public static double DoubleYap(string deger)
        {
            double sonuc;  //double türünde ve sonuc adında bir değişken tanımladık.
            double.TryParse(deger,NumberStyles.Currency, CultureInfo.CurrentUICulture.NumberFormat, out sonuc); //Globalization işlemi diğer bilgisayarlarda varsayılan değerlerini dil-bölge ile ilgili sorun yaşanmaması için.
            return Math.Round(sonuc,2); //return ile sonucu gönderdik.Double türünde bir deger döndürme işlemini yaptık.Virgülden sonra 2 basamak yuvarladık.

        }

        public static void StokAzalt(string barkod, double miktar)
        {
           if (barkod != "1111111111116")  //Barkod numarası bu değer den farklıysa işlem yapacaktır.
            {
            using (var db = new BarkodDbEntities()) // Veritabanını modelledik.
            {
                var urunbilgi = db.Urun.SingleOrDefault(x => x.Barkod == barkod); //Database de Urun tablosu içerisinde barkod olan satırı al dedik.
                urunbilgi.Miktar = urunbilgi.Miktar - miktar; //miktar değişkenininin azaltılması işlemini yaptık.
                db.SaveChanges(); //Database de yapılan değişikleri kaydedip güncelledik.
            }
        }
            
        }

        public static void StokArtir(string barkod, double miktar)
        {
            if (barkod != "1111111111116")  //Barkod numarası bu değer den farklıysa işlem yapacaktır.
            {
              using (var db = new BarkodDbEntities()) // Veritabanını modelledik.
            {
                var urunbilgi = db.Urun.SingleOrDefault(x => x.Barkod == barkod); //Database de Urun tablosu içerisinde barkod olan satırı al dedik.
                urunbilgi.Miktar = urunbilgi.Miktar + miktar; //miktar değişkenininin artırılması işlemini yaptık.
                db.SaveChanges();
            }
            }
               
        }
    
        public static void GridDuzenle(DataGridView dgv) //GridDuzenle adında bir metod tanımladım. DataGridView den gelecek bilgiyi dgv ile parametre aldık.
        {
            if(dgv.Columns.Count > 0) //Eğer dgv nin sütunları 0 dan büyükse
            {
                for(int i=0; i<dgv.Columns.Count; i++)  //dgv nin sütunlarını sayma işlemini yaptık.
                {
                    switch (dgv.Columns[i].HeaderText)  // dgv sütunlarındaki başlığındaki i. ninci sütunu aldık.
                    {
                        case "Id":   //Şart koşulumuz Id dedik.
                            dgv.Columns[i].HeaderText = "Numara"; break;   // dgv nin i. ninci header textine Numara yaz dedik.
                        case "UrunId":
                            dgv.Columns[i].HeaderText = "Urun Numarası"; break;
                        case "UrunAd":
                            dgv.Columns[i].HeaderText = "Ürün Adı"; break;
                        case "Aciklama":
                            dgv.Columns[i].HeaderText = "Açıklama"; break;
                        case "UrunGrup":
                            dgv.Columns[i].HeaderText = "Ürün Grubu"; break;
                        case "AlisFiyat":
                            dgv.Columns[i].HeaderText = "Alış Fiyatı"; 
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgv.Columns[i].DefaultCellStyle.Format = "C2"; 
                            break;
                        case "SatisFiyat":
                            dgv.Columns[i].HeaderText = "Satış Fiyatı";
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgv.Columns[i].DefaultCellStyle.Format = "C2";
                            break;
                        case "KdvOrani":
                            dgv.Columns[i].HeaderText = "Kdv Oranı";
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case "Birim":
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case "Miktar":
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case "OdemeSekli":
                            dgv.Columns[i].HeaderText = "Ödeme Şekli";
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case "Kart":
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgv.Columns[i].DefaultCellStyle.Format = "C2";
                            break;
                        case "Nakit":
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgv.Columns[i].DefaultCellStyle.Format = "C2";
                            break;
                        case "Gelir":
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgv.Columns[i].DefaultCellStyle.Format = "C2";
                            break;
                        case "Gider":
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgv.Columns[i].DefaultCellStyle.Format = "C2";
                            break;
                        case "Kullanici":
                            dgv.Columns[i].HeaderText = "Kullanıcı";
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case "KdvTutari":
                            dgv.Columns[i].HeaderText = "Kdv Tutarı";
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgv.Columns[i].DefaultCellStyle.Format = "C2";
                            break;

                    }
                }
            }
        }
    
        public static void StokHareket(string barkod, string urunad, string birim, double miktar, string urungrup, string kullanici)
        {
            using (var db = new BarkodDbEntities())   // db yeni bir
            {
                StokHareket sh = new StokHareket();  //database StokHareket() modelimizi sh değişkeninde tanımladık.
                //Modelimiz üzerine gelen bilgileri gönderdik.
                sh.Barkod = barkod;
                sh.UrunAd = urunad;
                sh.Birim = birim;
                sh.Miktar = miktar;
                sh.UrunGrup = urungrup;
                sh.Kullanici = kullanici;
                sh.Tarih = DateTime.Now;
                db.StokHareket.Add(sh);  //Database StokHareket Tablosuna oluşturduğumuz sh modelindeki değişiklikleri ekledik.
                db.SaveChanges();   //Database de kaydedip güncelledik.
            }
        }
    }
}
