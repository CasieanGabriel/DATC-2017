using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OxyPlot.Xamarin.Android;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Net.Http;
using Newtonsoft.Json;
using System.Globalization;

namespace DATC
{
    [Activity(Label = "Date senzor")]
    public class SenzorActivity : Activity
    {
        static HttpClient client = new HttpClient();
        Button btnTemp, btnUmid, btnPres;
        PlotView pltSenzor;
        string axisTitle;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle("Conectare esuata");
            alertDialog.SetNeutralButton("OK", delegate { alertDialog.Dispose(); });
            alertDialog.SetMessage("Verificati conexiunea la internet");
            SetContentView(Resource.Layout.DateSenzor);
            btnTemp = FindViewById<Button>(Resource.Id.btnTemp);
            btnUmid = FindViewById<Button>(Resource.Id.btnUmid);
            btnPres = FindViewById<Button>(Resource.Id.btnPres);
            btnTemp.Click += BtnTemp_Click;
            btnUmid.Click += BtnUmid_Click;
            btnPres.Click += BtnPres_Click;

            Helper.listaDateSenzor.Clear();
            if (Helper.vizualizareaCurenta == Helper.Vizualizare.Temperatura)
            {
                axisTitle = "Temperatura (C)";
            }
            else if (Helper.vizualizareaCurenta == Helper.Vizualizare.Umiditate)
            {
                axisTitle = "Umiditatea (%)";
            }
            else
            {
                axisTitle = "Presiune (mmHG)";
            }
            pltSenzor = FindViewById<PlotView>(Resource.Id.pltSenzor);

            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/hal+json");
                var Home = "http://datcapitmv.azurewebsites.net/api/values/" + Helper.senzorCurent.Substring(7);
                var response = client.GetAsync(Home).Result;
                string data = response.Content.ReadAsStringAsync().Result;
                Helper.listaDateSenzor = JsonConvert.DeserializeObject<List<DateSenzor>>(data);
            }
            catch (Exception e)
            {
                alertDialog.Show();
            }
            ActualizareGrafic();
        }

        private void BtnPres_Click(object sender, EventArgs e)
        {
            Helper.vizualizareaCurenta = Helper.Vizualizare.Presiune;
            axisTitle = "Presiune (mmHG)";
            ActualizareGrafic();
        }

        private void BtnUmid_Click(object sender, EventArgs e)
        {
            Helper.vizualizareaCurenta = Helper.Vizualizare.Umiditate;
            axisTitle = "Umiditate (%)";
            ActualizareGrafic();
        }

        private void BtnTemp_Click(object sender, EventArgs e)
        {
            Helper.vizualizareaCurenta = Helper.Vizualizare.Temperatura;
            axisTitle = "Temperatura (C)";
            ActualizareGrafic();
        }

        void ActualizareGrafic()
        {
            pltSenzor.Model = CreatePlotModel();
        }

        private PlotModel CreatePlotModel()
        {
            string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                         "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                         "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                         "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                         "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm"};
            DateTime timp, firstTime = DateTime.Now;
            TimeSpan minute;
            LineSeries series1 = new LineSeries();
            PlotModel plotModel = new PlotModel { Title = "Date " + Helper.senzorCurent, TitleColor = OxyColors.White };
            if (Helper.vizualizareaCurenta == Helper.Vizualizare.Temperatura)
            {
                plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Maximum = 50, Minimum = -20, TextColor = OxyColors.White, AxislineColor = OxyColors.White, MajorGridlineColor = OxyColors.White, Title = axisTitle, TitleColor = OxyColor.FromRgb(255, 255, 255) });
                plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, TextColor = OxyColors.White, MajorGridlineColor = OxyColors.White, AxislineColor = OxyColors.White, Title = "Timp (min)", TitleColor = OxyColor.FromRgb(255, 255, 255) });
                for (int index = 0; index < Helper.listaDateSenzor.Count; index++)
                {
                    if (index == 0)
                    {
                        firstTime = DateTime.ParseExact(Helper.listaDateSenzor[index].Data, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                        series1.Points.Add(new DataPoint(index, double.Parse(Helper.listaDateSenzor[index].Temperatura)));
                    }
                    else
                    {
                        timp = DateTime.ParseExact(Helper.listaDateSenzor[index].Data, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                        minute = timp.Subtract(firstTime);
                        series1.Points.Add(new DataPoint(minute.Minutes, double.Parse(Helper.listaDateSenzor[index].Temperatura)));
                    }
                }
            }
            else if (Helper.vizualizareaCurenta == Helper.Vizualizare.Umiditate)
            {
                plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Maximum = 100, Minimum = 0, TextColor = OxyColors.White, AxislineColor = OxyColors.White, MajorGridlineColor = OxyColors.White, Title = axisTitle, TitleColor = OxyColor.FromRgb(255, 255, 255) });
                plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, TextColor = OxyColors.White, MajorGridlineColor = OxyColors.White, AxislineColor = OxyColors.White, Title = "Timp (min)", TitleColor = OxyColor.FromRgb(255, 255, 255) });
                for (int index = 0; index < Helper.listaDateSenzor.Count; index++)
                    if (index == 0)
                    {
                        firstTime = DateTime.ParseExact(Helper.listaDateSenzor[index].Data, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                        series1.Points.Add(new DataPoint(index, double.Parse(Helper.listaDateSenzor[index].Umiditate)));
                    }
                    else
                    {
                        timp = DateTime.ParseExact(Helper.listaDateSenzor[index].Data, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                        minute = timp.Subtract(firstTime);
                        series1.Points.Add(new DataPoint(minute.Minutes, double.Parse(Helper.listaDateSenzor[index].Umiditate)));
                    }
            }
            else
            {
                plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Maximum = 790, Minimum = 720, TextColor = OxyColors.White, AxislineColor = OxyColors.White, MajorGridlineColor = OxyColors.White, Title = axisTitle, TitleColor = OxyColor.FromRgb(255, 255, 255) });
                plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, TextColor = OxyColors.White, MajorGridlineColor = OxyColors.White, AxislineColor = OxyColors.White, Title = "Timp (min)", TitleColor = OxyColor.FromRgb(255, 255, 255) });
                for (int index = 0; index < Helper.listaDateSenzor.Count; index++)
                    if (index == 0)
                    {
                        firstTime = DateTime.ParseExact(Helper.listaDateSenzor[index].Data, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                        series1.Points.Add(new DataPoint(index, double.Parse(Helper.listaDateSenzor[index].Presiune)));
                    }
                    else
                    {
                        timp = DateTime.ParseExact(Helper.listaDateSenzor[index].Data, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                        minute = timp.Subtract(firstTime);
                        series1.Points.Add(new DataPoint(minute.Minutes, double.Parse(Helper.listaDateSenzor[index].Presiune)));
                    }
            }
            plotModel.DefaultColors = new List<OxyColor> { OxyColors.White };
            plotModel.Series.Add(series1);
            return plotModel;
        }
    }
}