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
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            InitializeControlls();           
        }

        public void InitializeControlls()
        {
            Temperature_min_bar.Minimum = 0;
            Temperature_min_bar.Maximum = 50;
            Temperature_max_bar.Minimum = 0;
            Temperature_max_bar.Maximum = 50;
            Humidity_min_bar.Minimum = 0;
            Humidity_min_bar.Maximum = 100;
            Humidity_max_bar.Minimum = 0;
            Humidity_max_bar.Maximum = 100;
        }

        #region Scroll_Bars_Controlls
        private void Temperature_min_bar_Scroll(object sender, EventArgs e)
        {
            Temperature_min_txt.Text = Temperature_min_bar.Value.ToString();
        }

        private void Temperature_max_bar_Scroll(object sender, EventArgs e)
        {
            Temperature_max_txt.Text = Temperature_max_bar.Value.ToString();
        }

        private void Humidity_min_bar_Scroll(object sender, EventArgs e)
        {
            Humidity_min_txt.Text = Humidity_min_bar.Value.ToString();
        }

        private void Humidity_max_bar_Scroll(object sender, EventArgs e)
        {
            Humidity_max_txt.Text = Humidity_max_bar.Value.ToString();
        }
        #endregion

        #region Text_Boxes_Controlls

        private void Temperature_min_txt_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Temperature_min_txt.Text))
            {
                try
                {
                    Temperature_min_bar.Value = Convert.ToInt16(Temperature_min_txt.Text);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    Temperature_min_bar.Value = Temperature_min_bar.Maximum;
                    Temperature_min_txt.Text = Temperature_min_bar.Maximum.ToString();
                }
            }
        }

        private void Temperature_max_txt_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Temperature_max_txt.Text))
            {
                try
                {
                    Temperature_max_bar.Value = Convert.ToInt16(Temperature_max_txt.Text);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    Temperature_max_bar.Value = Temperature_max_bar.Maximum;
                    Temperature_max_txt.Text = Temperature_max_bar.Maximum.ToString();
                }
            }
        }

        private void Humidity_min_txt_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Humidity_min_txt.Text))
            {
                try
                {
                    Humidity_min_bar.Value = Convert.ToInt16(Humidity_min_txt.Text);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    Humidity_min_bar.Value = Humidity_min_bar.Maximum;
                    Humidity_min_txt.Text = Humidity_min_bar.Maximum.ToString();
                }
            }
        }

        private void Humidity_max_txt_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Humidity_max_txt.Text))
            {
                try
                {
                    Humidity_max_bar.Value = Convert.ToInt16(Humidity_max_txt.Text);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    Humidity_max_bar.Value = Humidity_max_bar.Maximum;
                    Humidity_max_txt.Text = Humidity_max_bar.Maximum.ToString();
                }
            }
        }

        #endregion

        private void Save_Settings_btn_Click(object sender, EventArgs e)
        {
            Sensors_Control New_sensor_control = new Sensors_Control();
            Sensors_Settings New_sensor_settings = new Sensors_Settings(Temperature_min_bar.Value, Temperature_max_bar.Value, Humidity_min_bar.Value, Humidity_max_bar.Value);
            New_sensor_control.My_Sensors_Settings.Add(New_sensor_settings);
            this.Close();
        }
    }
}
