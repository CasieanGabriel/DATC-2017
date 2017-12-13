using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Senzori
{
    public partial class Sensors_Control : Form
    {
        public List<Sensors_Settings> My_Sensors_Settings = new List<Sensors_Settings>();

        public Sensors_Control()
        {
            InitializeComponent();
        }

        private void Generate_btn_Click(object sender, EventArgs e)
        {
            Sensors_Data_dgv.Rows.Clear();
            if(!string.IsNullOrEmpty(Sensors_no_txt.Text)&&!string.IsNullOrEmpty(Sensors_Values_txt.Text))
            {
                for(int dgv_index=0;dgv_index<Convert.ToInt16(Sensors_no_txt.Text)*Convert.ToInt16(Sensors_Values_txt.Text);dgv_index++)
                {
                    Sensors_Data_dgv.Rows.Add();
                    Sensors_Data_dgv.Rows[dgv_index].Cells[0].Value = Convert.ToInt16(dgv_index)/ Convert.ToInt16(Sensors_Values_txt.Text) + 1;
                }
            }
        }

        private void Settings_btn_Click(object sender, EventArgs e)
        {
            Settings New_settings = new Settings();
            New_settings.ShowDialog();
        }
    }
}
