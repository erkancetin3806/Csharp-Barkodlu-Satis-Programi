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
    public partial class deneme : Form
    {
        public deneme()
        {
            InitializeComponent();
        }

        BarkodDbEntities db = new BarkodDbEntities();
        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int satirsayisi = gridSatisListesiD.Rows.Count;
            gridSatisListesiD.Rows.Add();
            
            if(satirsayisi > 1)
            {
                for (int i=0; i<satirsayisi; i++) {
                   
                }
            }

        }
    }
}
