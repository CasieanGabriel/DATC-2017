using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DATC_GenerareDate
{
    public partial class Form1 : Form
    {
        static string[] gv_ParkingLots = { "A", "B", "C", "D" };
        static string[] gv_ParkingSpace = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"};
        static uint[] gv_Malfunction = new uint[13];
        static uint[] gv_FreeOrNot = new uint[13];
        static uint[] gv_ResOrOcc = new uint[13];
        
        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
            Timer_Parking.Start();
            Timer_Malfunction.Start();
            Timer_SendDataToAPI.Start();
            Timer_GetReservationRequest.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
                
        private void Timer_Malfunction_Tick(object sender, EventArgs e)
        {
            Random Rnd = new Random();
            int PLot = Rnd.Next(0, gv_ParkingLots.Length);
            int PSpace = Rnd.Next(0, gv_ParkingSpace.Length);
            foreach (Control GBox in this.Controls)
            {
                if (GBox.GetType() == typeof(GroupBox))
                {
                    foreach(Control Button in GBox.Controls)
                    {
                        if(Button.GetType() == typeof(Button))
                        {
                            if(Button.Name == gv_ParkingLots[PLot] + gv_ParkingSpace[PSpace])
                            {
                                int Poz = PLot * gv_ParkingSpace.Length + PSpace;
                                if((1 & (gv_Malfunction[Poz / 8] >> Poz % 8)) == 0)
                                {
                                    gv_Malfunction[Poz / 8] |= (uint)(1 << Poz % 8);
                                    Button.BackColor = Color.Black;
                                    Timer MalfunctionSolved = new Timer();
                                    MalfunctionSolved.Interval = Rnd.Next(10,26) * 1000;
                                    MalfunctionSolved.Tick += (object s, EventArgs a) => Timer_MalfunctionSolved_Tick(s, a, Poz, Button);
                                    MalfunctionSolved.Start();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Timer_MalfunctionSolved_Tick(object sender, EventArgs e, int Poz, Control Button)
        {
            Timer Malfunction = (Timer)sender;
            Malfunction.Stop();

            gv_Malfunction[Poz / 8] = gv_Malfunction[Poz / 8] & ~(uint)(1 << Poz % 8);
            if((1 & (gv_FreeOrNot[Poz / 8] >> Poz % 8)) == 0)
            {
                Button.BackColor = Color.LimeGreen;
            }
            else if((1 & (gv_ResOrOcc[Poz / 8] >> Poz % 8)) == 0)
            {
                Button.BackColor = Color.Yellow;
            }
            else if((1 & (gv_ResOrOcc[Poz / 8] >> Poz % 8)) == 1)
            {
                Button.BackColor = Color.Red;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Random Rnd = new Random();
            int PLot = Rnd.Next(0, gv_ParkingLots.Length);
            int PSpace = Rnd.Next(0, gv_ParkingSpace.Length);
            foreach(Control GBox in this.Controls)
            {
                if(GBox.GetType() == typeof(GroupBox))
                {
                    foreach (Control Button in GBox.Controls)
                    {
                        if (Button.GetType() == typeof(Button))
                        {
                            if (Button.Name == gv_ParkingLots[PLot] + gv_ParkingSpace[PSpace])
                            {
                                int Poz = PLot * gv_ParkingSpace.Length + PSpace;
                                if ((1 & (gv_Malfunction[Poz / 8] >> Poz % 8)) == 0)
                                {
                                    if ((1 & (gv_FreeOrNot[Poz / 8] >> Poz % 8)) == 0)
                                    {
                                        // Free -> Reserved or Occupied
                                        gv_FreeOrNot[Poz / 8] |= (uint)(1 << Poz % 8);
                                        uint ResOrOcc = (uint)Rnd.Next(0, 2);

                                        Label_Free_Counter.Text = (Convert.ToInt32(Label_Free_Counter.Text) - 1).ToString();

                                        if (ResOrOcc == 0)
                                        {
                                            // Reserved
                                            gv_ResOrOcc[Poz / 8] = gv_ResOrOcc[Poz / 8] & ~(uint)(1 << Poz % 8);

                                            Button.BackColor = Color.Yellow;

                                            Timer ResToOcc = new Timer();
                                            ResToOcc.Interval = 20000;
                                            ResToOcc.Tick += (object s, EventArgs a) => Timer_ResToOcc_Tick(s, a, Poz, Button);
                                            ResToOcc.Start();

                                            Label_Reserved_Counter.Text = (Convert.ToInt32(Label_Reserved_Counter.Text) + 1).ToString();
                                        }
                                        else
                                        {
                                            // Occupied
                                            gv_ResOrOcc[Poz / 8] |= (uint)(1 << Poz % 8);

                                            Button.BackColor = Color.Red;

                                            Label_Occupied_Counter.Text = (Convert.ToInt32(Label_Occupied_Counter.Text) + 1).ToString();
                                        }
                                    }
                                    else
                                    {
                                        // Reserved or Occupied -> Free
                                        Label_Free_Counter.Text = (Convert.ToInt32(Label_Free_Counter.Text) + 1).ToString();
                                        if ((1 & (gv_ResOrOcc[Poz / 8] >> Poz % 8)) == 0)
                                            Label_Reserved_Counter.Text = (Convert.ToInt32(Label_Reserved_Counter.Text) - 1).ToString();
                                        else
                                            Label_Occupied_Counter.Text = (Convert.ToInt32(Label_Occupied_Counter.Text) - 1).ToString();

                                        gv_FreeOrNot[Poz / 8] = gv_FreeOrNot[Poz / 8] & ~(uint)(1 << Poz % 8);
                                        gv_ResOrOcc[Poz / 8] = gv_ResOrOcc[Poz / 8] & ~(uint)(1 << Poz % 8);

                                        Button.BackColor = Color.LimeGreen;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Timer_ResToOcc_Tick(object sender, EventArgs e, int Poz, Control Btn)
        {
            Timer OccToRes = (Timer)sender;
            OccToRes.Stop();

            if((1 & (gv_ResOrOcc[Poz / 8] >> Poz % 8)) == 0)
            {
                gv_ResOrOcc[Poz / 8] |= (uint)(1 << Poz % 8);
                Btn.BackColor = Color.Red;

                Label_Reserved_Counter.Text = (Convert.ToInt32(Label_Reserved_Counter.Text) - 1).ToString();
                Label_Occupied_Counter.Text = (Convert.ToInt32(Label_Occupied_Counter.Text) + 1).ToString();
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button Btn = (Button)sender;
            for(int PL_Counter = 0; PL_Counter < gv_ParkingLots.Length; PL_Counter++)
            {
                for(int PS_Counter = 0; PS_Counter < gv_ParkingSpace.Length; PS_Counter++)
                {
                    if (Btn.Name == gv_ParkingLots[PL_Counter] + gv_ParkingSpace[PS_Counter])
                    {
                        int Poz = PL_Counter * gv_ParkingSpace.Length + PS_Counter;
                        if ((1 & (gv_Malfunction[Poz / 8] >> Poz % 8)) == 0)
                        {
                            if ((1 & (gv_FreeOrNot[Poz / 8] >> Poz % 8)) == 0)
                            {
                                // Free -> Reserved
                                gv_FreeOrNot[Poz / 8] |= (uint)(1 << Poz % 8);
                                gv_ResOrOcc[Poz / 8] = gv_ResOrOcc[Poz / 8] & ~(uint)(1 << Poz % 8);
                                Btn.BackColor = Color.Yellow;
                                Timer OccToRes = new Timer();
                                OccToRes.Interval = 20000;
                                OccToRes.Tick += (object s, EventArgs a) => Timer_ResToOcc_Tick(s, a, Poz, Btn);
                                OccToRes.Start();

                                Label_Free_Counter.Text = (Convert.ToInt32(Label_Free_Counter.Text) - 1).ToString();
                                Label_Reserved_Counter.Text = (Convert.ToInt32(Label_Reserved_Counter.Text) + 1).ToString();
                            }
                            else if (((1 & (gv_FreeOrNot[Poz / 8] >> Poz % 8)) == 1) && ((1 & (gv_ResOrOcc[Poz / 8] >> Poz % 8)) == 1))
                            {
                                // Occupied -> Free
                                gv_FreeOrNot[Poz / 8] = gv_FreeOrNot[Poz / 8] & ~(uint)(1 << Poz % 8);
                                //gv_ResOrOcc[Poz / 8] = gv_ResOrOcc[Poz / 8] & ~(uint)(1 << Poz % 8);
                                Btn.BackColor = Color.LimeGreen;

                                Label_Occupied_Counter.Text = (Convert.ToInt32(Label_Occupied_Counter.Text) - 1).ToString();
                                Label_Free_Counter.Text = (Convert.ToInt32(Label_Free_Counter.Text) + 1).ToString();
                            }
                            else
                            {
                                // Reserved -> Occupied
                                gv_ResOrOcc[Poz / 8] |= (uint)(1 << Poz % 8);
                                Btn.BackColor = Color.Red;

                                Label_Reserved_Counter.Text = (Convert.ToInt32(Label_Reserved_Counter.Text) - 1).ToString();
                                Label_Occupied_Counter.Text = (Convert.ToInt32(Label_Occupied_Counter.Text) + 1).ToString();
                            }
                        }
                    }
                }
            }
        }

        private void Text_Changed(object sender, EventArgs e)
        {
            Label Lbl = (Label)sender;
            if (Convert.ToInt32(Lbl.Text) < 0)
            {
                int i_Free = 0;
                int i_Occ = 0;
                int i_Res = 0;

                for(int i_Counter = 0; i_Counter < gv_ParkingLots.Length * gv_ParkingSpace.Length; i_Counter++)
                {
                    if((1 & (gv_FreeOrNot[i_Counter / 8] >> i_Counter % 8)) == 0)
                    {
                        i_Free++;
                    }
                    else
                    {
                        if((1 & (gv_ResOrOcc[i_Counter / 8] >> i_Counter % 8)) == 0)
                        {
                            i_Res++;
                        }
                        else
                        {
                            i_Occ++;
                        }
                    }
                }

                Label_Free_Counter.Text = i_Free.ToString();
                Label_Occupied_Counter.Text = i_Occ.ToString();
                Label_Reserved_Counter.Text = i_Res.ToString();
            }
        }

        private async void Timer_SendDataToAPI_TickAsync(object sender, EventArgs e)
        {
            string s_FreeOrNot = string.Empty;
            string s_ResOrOcc = string.Empty;
            string s_Malfunction = string.Empty;

            for(int i_Counter = 0; i_Counter < gv_ParkingLots.Length * gv_ParkingSpace.Length; i_Counter++)
            {
                s_FreeOrNot += (1 & (gv_FreeOrNot[i_Counter / 8] >> i_Counter % 8)).ToString();
                s_ResOrOcc += (1 & (gv_ResOrOcc[i_Counter / 8] >> i_Counter % 8)).ToString();
                s_Malfunction += (1 & (gv_Malfunction[i_Counter / 8] >> i_Counter % 8)).ToString();
            }

            HttpClient client = new HttpClient();

            Dictionary<string, string> d = new Dictionary<string, string>
            {
                { "FoN", s_FreeOrNot },
                { "RoO", s_ResOrOcc },
                { "Mal", s_Malfunction }
            };

            var content = new FormUrlEncodedContent(d);

            var response = await client.PostAsync("http://localhost:60454/Home/PostRawData", content);

            var responseString = await response.Content.ReadAsStringAsync();
        }

        private void Timer_GetReservationRequest_Tick(object sender, EventArgs e)
        {
            bool b_PLSwitch = false;
            bool b_PSSwitch = false;
            int i_PL = -1, i_PS = -1;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetAsync("http://localhost:60454/Home/GetJsonReservationRequests").Result;

            var responseString = response.Content.ReadAsStringAsync().Result;
            var obj = JsonConvert.DeserializeObject(responseString);

            JsonTextReader reader = new JsonTextReader(new System.IO.StringReader(obj.ToString()));

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (b_PLSwitch == true)
                    {
                        for (int i_Counter = 0; i_Counter < gv_ParkingLots.Length; i_Counter++)
                        {
                            if (gv_ParkingLots[i_Counter] == reader.Value.ToString())
                            {
                                i_PL = i_Counter;
                                b_PLSwitch = false;
                            }
                        }
                    }
                    if (reader.Value.ToString() == "ParkingLot")
                    {
                        b_PLSwitch = true;
                    }

                    if (b_PSSwitch == true)
                    {
                        for (int i_Counter = 0; i_Counter < gv_ParkingSpace.Length; i_Counter++)
                        {
                            if (gv_ParkingSpace[i_Counter] == reader.Value.ToString())
                            {
                                i_PS = i_Counter;
                                b_PSSwitch = false;
                            }
                        }
                    }
                    if (reader.Value.ToString() == "ParkingSpace")
                    {
                        b_PSSwitch = true;
                    }

                    if (i_PL != -1 && i_PS != -1)
                    {
                        foreach (Control GBox in this.Controls)
                        {
                            if (GBox.GetType() == typeof(GroupBox))
                            {
                                foreach (Control Button in GBox.Controls)
                                {
                                    if (Button.GetType() == typeof(Button))
                                    {
                                        if (Button.Name == gv_ParkingLots[i_PL] + gv_ParkingSpace[i_PS])
                                        {
                                            int i_Poz = i_PL * gv_ParkingSpace.Length + i_PS;
                                            if ((1 & (gv_Malfunction[i_Poz / 8] >> i_Poz % 8)) == 0)
                                            {
                                                if ((1 & (gv_FreeOrNot[i_Poz / 8] >> i_Poz % 8)) == 0)
                                                {
                                                    // Free -> Reserved
                                                    gv_FreeOrNot[i_Poz / 8] |= (uint)(1 << i_Poz % 8);
                                                    gv_ResOrOcc[i_Poz / 8] = gv_ResOrOcc[i_Poz / 8] & ~(uint)(1 << i_Poz % 8);

                                                    Button.BackColor = Color.Yellow;

                                                    Timer ResToOcc = new Timer();
                                                    ResToOcc.Interval = 20000;
                                                    ResToOcc.Tick += (object s, EventArgs a) => Timer_ResToOcc_Tick(s, a, i_Poz, Button);
                                                    ResToOcc.Start();

                                                    Label_Free_Counter.Text = (Convert.ToInt32(Label_Free_Counter.Text) - 1).ToString();
                                                    Label_Reserved_Counter.Text = (Convert.ToInt32(Label_Reserved_Counter.Text) + 1).ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        i_PL = i_PS = -1;
                    }
                }
            }
            b_PLSwitch = b_PSSwitch = false;
        }
    }
}
