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

namespace DATC
{
    [Activity(Label = "Date senzor")]
    public class SenzorActivity : Activity
    {
        Button btnTemp, btnUmid, btnPres;
        PlotView pltSenzor;
        public static List<int> dateSenzor = new List<int>();
        string axisTitle;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DateSenzor);
            btnTemp = FindViewById<Button>(Resource.Id.btnTemp);
            btnUmid = FindViewById<Button>(Resource.Id.btnUmid);
            btnPres = FindViewById<Button>(Resource.Id.btnPres);
            btnTemp.Click += BtnTemp_Click;
            btnUmid.Click += BtnUmid_Click;
            btnPres.Click += BtnPres_Click;

            if (Helper.vizualizareaCurenta == Helper.Vizualizare.Temperatura)
            {
                axisTitle = "Temperatura (C)";
            }
            else if(Helper.vizualizareaCurenta == Helper.Vizualizare.Umiditate)
            {
                axisTitle = "Umiditatea (%)";
            }
            else
            {
                axisTitle = "Presiune (Pa)";
            }
            pltSenzor = FindViewById<PlotView>(Resource.Id.pltSenzor);
            dateSenzor.Add(34);
            dateSenzor.Add(32);
            dateSenzor.Add(36);
            ActualizareGrafic();
        }

        private void BtnPres_Click(object sender, EventArgs e)
        {
            Helper.vizualizareaCurenta = Helper.Vizualizare.Presiune;
            axisTitle = "Presiune (Pa)";
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
            LineSeries series1 = new LineSeries();
            PlotModel plotModel = new PlotModel { Title = "Date " + Helper.senzorCurent, TitleColor= OxyColors.White };
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Maximum = 50, Minimum = -20, TextColor = OxyColors.White, AxislineColor = OxyColors.White, MajorGridlineColor = OxyColors.White,Title= axisTitle, TitleColor = OxyColor.FromRgb(255, 255, 255) });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom,TextColor= OxyColors.White, MajorGridlineColor = OxyColors.White,AxislineColor= OxyColors.White, Title ="Timp (s)", TitleColor= OxyColor.FromRgb(255,255,255)});
            for (int i = 0; i < dateSenzor.Count; i++)
                series1.Points.Add(new DataPoint(0.1 * i, dateSenzor[i]));
            plotModel.DefaultColors = new List<OxyColor> { OxyColors.White };
            plotModel.Series.Add(series1);
            return plotModel;
        }
    }
}