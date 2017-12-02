using Android.App;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using System;
using Android.Gms.Maps.Model;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DATC
{
    [Activity(Label = "Irrigation Master", MainLauncher = true)]
    public class MainActivity : Activity,IOnMapReadyCallback
    {
        static HttpClient client = new HttpClient();
        GoogleMap mMap;
        Button btnTemp,btnUmid,btnPres;
        public void OnMapReady(GoogleMap googleMap)
        {
            LatLng cameralatLng = new LatLng(45.740363, 21.244295);
            mMap = googleMap;
            mMap.MapType = GoogleMap.MapTypeSatellite;
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(cameralatLng,18);
            mMap.MoveCamera(camera);
            mMap.MarkerClick += MMap_MarkerClick;

            /*Helper.DesenarePoligon(mMap, new LatLng(45.740684, 21.244014), new LatLng(45.740687, 21.244037), new LatLng(45.740667, 21.244037), new LatLng(45.740663, 21.244003), Color.Red.ToArgb());
             Helper.DesenarePoligon(mMap, latlnh[0], latlnh[5], latlnh[11], latlnh[6], Color.Red.ToArgb());*/
            for (int index = 0; index < Helper.listaSenzori.Count; index++)
                Helper.AdaugareMarker(mMap,Helper.listaSenzori[index].Coordonate, "Senzor " + Helper.listaSenzori[index].Idsenzor);

        }

        private void MMap_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            Helper.senzorCurent = e.Marker.Title;
            StartActivity(typeof(SenzorActivity));
        }

        protected override void OnResume()
        {
            base.OnResume();
            if(Helper.vizualizareaCurenta==Helper.Vizualizare.Temperatura)
            {
               
            }
            else if(Helper.vizualizareaCurenta == Helper.Vizualizare.Umiditate)
            {

            }
            else
            {

            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            btnTemp = FindViewById<Button>(Resource.Id.btnTemp);
            btnUmid = FindViewById<Button>(Resource.Id.btnUmid);
            btnPres = FindViewById<Button>(Resource.Id.btnPres);
            SetupMap();
            btnTemp.Click += BtnTemp_Click;
            btnUmid.Click += BtnUmid_Click;
            btnPres.Click += BtnPres_Click;
            //Preluare lista senzori
            client.DefaultRequestHeaders.Add("Accept", "application/hal+json");
            var Home = "http://datcapitmv.azurewebsites.net/api/values";
            var response = client.GetAsync(Home).Result;
            string data = response.Content.ReadAsStringAsync().Result;
            Helper.listaSenzori = JsonConvert.DeserializeObject<List<Senzor>>(data);
            for(int index=0;index<Helper.listaSenzori.Count;index++)
            {
                Helper.listaSenzori[index].Coordonate = new LatLng(double.Parse(Helper.listaSenzori[index].Latitudine), double.Parse(Helper.listaSenzori[index].Longitudine));
            }

            if (Helper.vizualizareaCurenta == Helper.Vizualizare.Temperatura)
            {
                //get heatmap
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
        }

        private void BtnUmid_Click(object sender, EventArgs e)
        {
            Helper.vizualizareaCurenta = Helper.Vizualizare.Umiditate;
        }

        private void BtnTemp_Click(object sender, EventArgs e)
        {
            Helper.vizualizareaCurenta = Helper.Vizualizare.Temperatura;
        }

        private void BtnTempOrUmid_Click(object sender, EventArgs e)
        {

                Helper.vizualizareaCurenta = Helper.Vizualizare.Umiditate;
        }

        private void SetupMap()
        {
            if (mMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
            }
        }
    }
}

