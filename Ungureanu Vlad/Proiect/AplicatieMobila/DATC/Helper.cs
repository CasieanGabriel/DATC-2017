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
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace DATC
{
   public  class Helper
    {
        public enum Vizualizare { Temperatura, Umiditate, Presiune}
        public static string senzorCurent;
        public static Vizualizare vizualizareaCurenta =Vizualizare.Temperatura;
                                                      
        public static void AdaugareMarker(GoogleMap googleMap, LatLng latLng, string numeSenzor )
        {
            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetPosition(latLng);
            markerOptions.SetTitle(numeSenzor);
            googleMap.AddMarker(markerOptions);
        }
    }
}