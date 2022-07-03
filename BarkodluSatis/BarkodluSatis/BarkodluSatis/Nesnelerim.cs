using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace BarkodluSatis
{
    internal class Nesnelerim
    {
    }

    class lStandart : Label //Label özellikleri kalıtım yoluyla lStandart a geçmesini sağladık.
    {
        public lStandart() //Class a kurucu metod tanımladık.
        {
            this.ForeColor = System.Drawing.Color.DarkCyan; //Label rengini tanımladık.
            this.Text = "lStandart"; //Label textine yazılacak yazıyı belirttik.
            this.Font = new System.Drawing.Font("Microsoft Sans Serif",12F,System.Drawing.FontStyle.Regular,System.Drawing.GraphicsUnit.Point); //Fontunu ayarladık.
            this.Name = "lStandart"; //Name ini ayarladık.                      
        }    
    }

    class bStandart : Button // Buton özelliği türünden class tanımladık.
        {
        public bStandart()  //Kurucu Metodu tanımladık.
        {
            //Buton özelliklerini ayarladık.
            this.BackColor = System.Drawing.Color.Tomato;
            this.FlatAppearance.BorderColor = System.Drawing.Color.Tomato;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ForeColor = System.Drawing.Color.White;
            this.Image = global::BarkodluSatis.Properties.Resources.turkish_lira;
            this.Location = new System.Drawing.Point(1, 1);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "bNakit";
            this.Size = new System.Drawing.Size(134, 113);
            this.TabIndex = 0;
            this.Text = "NAKİT \r\n(F1)";
            this.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.UseVisualStyleBackColor = false;
          
        }           
    }

    class tStandart : TextBox
    {
        public tStandart()
        {
            this.Size = new System.Drawing.Size(250, 26); //Yazı boyutu ayarını yaptık.
            this.BackColor = System.Drawing.Color.White; //Arkaplan rengini ayarladık.
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F); //Yazı tipini ayarladık.
        }
    }

    class tNumeric : TextBox
    {
        public tNumeric()
        {
            this.Size = new System.Drawing.Size(115, 26); //Yazu boyutunu ayarladık.
            this.BackColor = System.Drawing.Color.White; //Yazı rengini ayarladık.
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F); //Yazı tipini ayarladık.
            this.Name = "tNumeric";  // textbox name ini ayarladık.
            this.TextAlign = System.Windows.Forms.HorizontalAlignment.Right; //Textbox sağa yatay hizalı olarak görünmesini ayarladık.
            this.Click += TNumeric_Click;   //Click olayı olduğu zaman metoda aktardık.
            this.KeyPress += TNumeric_KeyPress; //Sadece rakamları girebilmek için metoda aktardık.

        }

        private void TNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar)==false && e.KeyChar!=(char)08 && e.KeyChar!=(char)44)  //Sadece rakamları girmek için tanımladık.
            {
                e.Handled = true;
            }
        }

        private void TNumeric_Click(object sender, EventArgs e)
        {
            this.SelectAll(); //Üzerine tıklandığı zaman içerisindeki karakterleri seçiyor. 
        }
    }

    class gridOzel : DataGridView
    {
        public gridOzel()
        {
            this.AllowUserToAddRows = false;
            this.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            this.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            this.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnHeadersDefaultCellStyle = this.DefaultCellStyle;
            this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EnableHeadersVisualStyles = false;
            this.Location = new System.Drawing.Point(3, 125);
            this.Name = "gridSatisListesi";
            this.RowHeadersVisible = false;
            this.RowHeadersWidth = 51;
            this.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            this.RowsDefaultCellStyle = this.DefaultCellStyle;
            this.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            this.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Silver;
            this.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.RowTemplate.Height = 34;
            this.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Size = new System.Drawing.Size(677, 451);
        }
    }
}
