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
using System.Drawing;

namespace DATC
{
   public  class Helper
    {
        public enum Vizualizare { Temperatura, Umiditate, Presiune}
        public static string senzorCurent;
        public static Vizualizare vizualizareaCurenta =Vizualizare.Temperatura;
        public static List<Senzor> listaSenzori = new List<Senzor>();
                                                      
        public static void AdaugareMarker(GoogleMap googleMap, LatLng latLng, string numeSenzor )
        {
            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetPosition(latLng);
            markerOptions.SetTitle(numeSenzor);
            markerOptions.SetAlpha((float)0.5);
            googleMap.AddMarker(markerOptions);
        }

        public static void DesenarePoligon(GoogleMap googleMap,LatLng A,LatLng B,LatLng C,LatLng D,int Culoare)
        {
              PolygonOptions polygonOptions = new PolygonOptions();
              polygonOptions.Add(A);
              polygonOptions.Add(B);
              polygonOptions.Add(C);
              polygonOptions.Add(D);
              polygonOptions.InvokeFillColor(Culoare);
              polygonOptions.InvokeStrokeWidth(0);
              googleMap.AddPolygon(polygonOptions);
        }
    }
}