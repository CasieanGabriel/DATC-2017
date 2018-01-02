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
using Android.Graphics;

namespace DATC
{
    [Activity(Label = "Legenda")]
    public class LegendaActivity : Activity
    {
        Button btnTemp, btnUmid, btnPres;
        //ImageView imgLegenda;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Legenda);
            btnTemp = FindViewById<Button>(Resource.Id.btnTemp);
            btnUmid = FindViewById<Button>(Resource.Id.btnUmid);
            btnPres = FindViewById<Button>(Resource.Id.btnPres);
            //imgLegenda=FindViewById<ImageView>(Resource.Id.imgLegenda);
            btnTemp.Click += BtnTemp_Click;
            btnUmid.Click += BtnUmid_Click;
            btnPres.Click += BtnPres_Click;
            //btnTemp.SetBackgroundColor(new Color());
            if (Helper.vizualizareaCurenta == Helper.Vizualizare.Temperatura)
            {

            }
            else if (Helper.vizualizareaCurenta == Helper.Vizualizare.Umiditate)
            {

                
            }
            else
            {
                
            }
        }

        private void BtnPres_Click(object sender, EventArgs e)
        {
            Helper.vizualizareaCurenta = Helper.Vizualizare.Presiune;
            //imgLegenda.SetImageResource(Resource.Drawable.download);
        }

        private void BtnUmid_Click(object sender, EventArgs e)
        {
            Helper.vizualizareaCurenta = Helper.Vizualizare.Umiditate;
            //imgLegenda.SetImageResource(Resource.Drawable.download1);
        }

        private void BtnTemp_Click(object sender, EventArgs e)
        {
            Helper.vizualizareaCurenta = Helper.Vizualizare.Temperatura;
            //imgLegenda.SetImageResource(Resource.Drawable.download);
        }
    }
}