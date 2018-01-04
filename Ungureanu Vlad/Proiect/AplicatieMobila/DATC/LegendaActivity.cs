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
        //ImageView imgLegenda;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Legenda);
            //imgLegenda=FindViewById<ImageView>(Resource.Id.imgLegenda);
            //imgLegenda.SetImageResource(Resource.Drawable.download);
        }
    }
}