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
using System.Timers;
using System.Threading;

namespace DATC
{
    [Activity(Label = "Irrigation Master", MainLauncher = true)]
    public class MainActivity : Activity, IOnMapReadyCallback
    {
        static HttpClient client = new HttpClient();
        static GoogleMap mMap;
        Button btnTemp, btnUmid, btnPres;
        public static System.Timers.Timer timpActualizare = new System.Timers.Timer(90000);
        public void OnMapReady(GoogleMap googleMap)
        {
            LatLng cameralatLng = new LatLng(45.740363, 21.244295);
            mMap = googleMap;
            mMap.MapType = GoogleMap.MapTypeSatellite;
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(cameralatLng, 18);
            mMap.MoveCamera(camera);
            mMap.MarkerClick += MMap_MarkerClick;
            mMap.MapLongClick += MMap_MapLongClick;
            mMap.MapClick += MMap_MapClick;
            for (int index = 0; index < Helper.listaHeatMapTemp.Count; index++)
            {
                Helper.DesenarePoligon(mMap, new LatLng(double.Parse(Helper.listaHeatMapTemp[index].LatA), double.Parse(Helper.listaHeatMapTemp[index].LngA)), new LatLng(double.Parse(Helper.listaHeatMapTemp[index].LatB), double.Parse(Helper.listaHeatMapTemp[index].LngB)), new LatLng(double.Parse(Helper.listaHeatMapTemp[index].LatC), double.Parse(Helper.listaHeatMapTemp[index].LngC)), new LatLng(double.Parse(Helper.listaHeatMapTemp[index].LatD), double.Parse(Helper.listaHeatMapTemp[index].LngD)), Helper.listaHeatMapTemp[index].Culoare);
            }
        }

        private void MMap_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            Helper.markersVisible = !Helper.markersVisible;
            if (Helper.markersVisible == true)
            {
                for (int index = 0; index < Helper.listaSenzori.Count; index++)
                    Helper.AdaugareMarker(mMap, Helper.listaSenzori[index].Coordonate, "Senzor " + Helper.listaSenzori[index].Idsenzor);
            }
            else
            {
                mMap.Clear();
                if (Helper.vizualizareaCurenta == Helper.Vizualizare.Temperatura)
                {
                    for (int index = 0; index < Helper.listaHeatMapTemp.Count; index++)
                    {
                        Helper.DesenarePoligon(mMap, new LatLng(double.Parse(Helper.listaHeatMapTemp[index].LatA), double.Parse(Helper.listaHeatMapTemp[index].LngA)), new LatLng(double.Parse(Helper.listaHeatMapTemp[index].LatB), double.Parse(Helper.listaHeatMapTemp[index].LngB)), new LatLng(double.Parse(Helper.listaHeatMapTemp[index].LatC), double.Parse(Helper.listaHeatMapTemp[index].LngC)), new LatLng(double.Parse(Helper.listaHeatMapTemp[index].LatD), double.Parse(Helper.listaHeatMapTemp[index].LngD)), Helper.listaHeatMapTemp[index].Culoare);
                    }
                }
                else if (Helper.vizualizareaCurenta == Helper.Vizualizare.Umiditate)
                {
                    for (int index = 0; index < Helper.listaHeatMapUmid.Count; index++)
                    {
                        Helper.DesenarePoligon(mMap, new LatLng(double.Parse(Helper.listaHeatMapUmid[index].LatA), double.Parse(Helper.listaHeatMapUmid[index].LngA)), new LatLng(double.Parse(Helper.listaHeatMapUmid[index].LatB), double.Parse(Helper.listaHeatMapUmid[index].LngB)), new LatLng(double.Parse(Helper.listaHeatMapUmid[index].LatC), double.Parse(Helper.listaHeatMapUmid[index].LngC)), new LatLng(double.Parse(Helper.listaHeatMapUmid[index].LatD), double.Parse(Helper.listaHeatMapUmid[index].LngD)), Helper.listaHeatMapUmid[index].Culoare);
                    }
                }
                else
                {
                    for (int index = 0; index < Helper.listaHeatMapPres.Count; index++)
                    {
                        Helper.DesenarePoligon(mMap, new LatLng(double.Parse(Helper.listaHeatMapPres[index].LatA), double.Parse(Helper.listaHeatMapPres[index].LngA)), new LatLng(double.Parse(Helper.listaHeatMapPres[index].LatB), double.Parse(Helper.listaHeatMapPres[index].LngB)), new LatLng(double.Parse(Helper.listaHeatMapPres[index].LatC), double.Parse(Helper.listaHeatMapPres[index].LngC)), new LatLng(double.Parse(Helper.listaHeatMapPres[index].LatD), double.Parse(Helper.listaHeatMapPres[index].LngD)), Helper.listaHeatMapPres[index].Culoare);
                    }
                }
            }
        }

        private void MMap_MapLongClick(object sender, GoogleMap.MapLongClickEventArgs e)
        {
            StartActivity(typeof(LogsActivity));
        }

        private void MMap_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            Helper.senzorCurent = e.Marker.Title;
            StartActivity(typeof(SenzorActivity));
        }

        protected override void OnResume()
        {
            base.OnResume();
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
            btnTemp.LongClick += BtnTemp_LongClick;
            btnUmid.LongClick += BtnUmid_LongClick;
            btnPres.LongClick += BtnPres_LongClick;
            timpActualizare.Elapsed += TimpActualizare_Elapsed;
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle("Conectare esuata");
            alertDialog.SetNeutralButton("OK", delegate { alertDialog.Dispose(); });
            alertDialog.SetMessage("Verificati conexiunea la internet");
            //Preluare lista senzori
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/hal+json");
                var Home = "http://datcapitmv.azurewebsites.net/api/values";
                var response = client.GetAsync(Home).Result;
                string data = response.Content.ReadAsStringAsync().Result;
                Helper.listaSenzori = JsonConvert.DeserializeObject<List<Senzor>>(data);
            }
            catch (Exception e)
            {
                alertDialog.Show();
            }

            for (int index = 0; index < Helper.listaSenzori.Count; index++)
            {
                Helper.listaSenzori[index].Coordonate = new LatLng(double.Parse(Helper.listaSenzori[index].Latitudine), double.Parse(Helper.listaSenzori[index].Longitudine));
            }
            //Preluare HeatMap temperatura
            try
            {
                var Home = "http://datcapitmv.azurewebsites.net/api/Temperatura";
                var response = client.GetAsync(Home).Result;
                string data = response.Content.ReadAsStringAsync().Result;
                Helper.listaHeatMapTemp = JsonConvert.DeserializeObject<List<HeatMap>>(data);
            }
            catch (Exception e)
            {
                alertDialog.Show();
            }
            try
            {
                Helper.listaHeatMapUmid.Clear();
                var Home = "http://datcapitmv.azurewebsites.net/api/Umiditate";
                var response = client.GetAsync(Home).Result;
                string data = response.Content.ReadAsStringAsync().Result;
                Helper.listaHeatMapUmid = JsonConvert.DeserializeObject<List<HeatMap>>(data);
            }
            catch (Exception e)
            {
                alertDialog.Show();
            }
            try
            {
                Helper.listaHeatMapPres.Clear();
                var Home = "http://datcapitmv.azurewebsites.net/api/Presiune";
                var response = client.GetAsync(Home).Result;
                string data = response.Content.ReadAsStringAsync().Result;
                Helper.listaHeatMapPres = JsonConvert.DeserializeObject<List<HeatMap>>(data);
            }
            catch (Exception e)
            {
                alertDialog.Show();
            }
            timpActualizare.Start();
        }

        private void BtnPres_LongClick(object sender, Android.Views.View.LongClickEventArgs e)
        {
            //Helper.vizualizareaCurenta = Helper.Vizualizare.Presiune;
            StartActivity(typeof(LegendaActivity));
        }

        private void BtnUmid_LongClick(object sender, Android.Views.View.LongClickEventArgs e)
        {
            //Helper.vizualizareaCurenta = Helper.Vizualizare.Umiditate;
            StartActivity(typeof(LegendaActivity));
        }

        private void BtnTemp_LongClick(object sender, Android.Views.View.LongClickEventArgs e)
        {
            //Helper.vizualizareaCurenta = Helper.Vizualizare.Temperatura;
            StartActivity(typeof(LegendaActivity));
        }

        private void TimpActualizare_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool error = false;
            try
            {
                Helper.listaHeatMapTemp.Clear();
                var Home = "http://datcapitmv.azurewebsites.net/api/Temperatura";
                var response = client.GetAsync(Home).Result;
                string data = response.Content.ReadAsStringAsync().Result;
                Helper.listaHeatMapTemp = JsonConvert.DeserializeObject<List<HeatMap>>(data);
            }
            catch (Exception ex)
            {
                error = true;
            }
            try
            {
                Helper.listaHeatMapUmid.Clear();
                var Home = "http://datcapitmv.azurewebsites.net/api/Umiditate";
                var response = client.GetAsync(Home).Result;
                string data = response.Content.ReadAsStringAsync().Result;
                Helper.listaHeatMapUmid = JsonConvert.DeserializeObject<List<HeatMap>>(data);
            }
            catch (Exception ex)
            {
                error = true;
            }
            try
            {
                Helper.listaHeatMapPres.Clear();
                var Home = "http://datcapitmv.azurewebsites.net/api/Presiune";
                var response = client.GetAsync(Home).Result;
                string data = response.Content.ReadAsStringAsync().Result;
                Helper.listaHeatMapPres = JsonConvert.DeserializeObject<List<HeatMap>>(data);
            }
            catch (Exception ex)
            {
                error = true;
            }
            RunOnUiThread(() =>
            {
                if (error == true)
                {
                    AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
                    alertDialog.SetTitle("Conectare esuata");
                    alertDialog.SetNeutralButton("OK", delegate { alertDialog.Dispose(); });
                    alertDialog.SetMessage("Verificati conexiunea la internet");
                    alertDialog.Show();
                }
                else
                {
                    mMap.Clear();
                    if (Helper.vizualizareaCurenta == Helper.Vizualizare.Temperatura)
                    {
                        for (int index = 0; index < Helper.listaHeatMapTemp.Count; index++)
                        {
                            Helper.DesenarePoligon(mMap, new LatLng(double.Parse(Helper.listaHeatMapTemp[index].LatA), double.Parse(Helper.listaHeatMapTemp[index].LngA)), new LatLng(double.Parse(Helper.listaHeatMapTemp[index].LatB), double.Parse(Helper.listaHeatMapTemp[index].LngB)), new LatLng(double.Parse(Helper.listaHeatMapTemp[index].LatC), double.Parse(Helper.listaHeatMapTemp[index].LngC)), new LatLng(double.Parse(Helper.listaHeatMapTemp[index].LatD), double.Parse(Helper.listaHeatMapTemp[index].LngD)), Helper.listaHeatMapTemp[index].Culoare);
                        }
                    }
                    else if (Helper.vizualizareaCurenta == Helper.Vizualizare.Umiditate)
                    {
                        for (int index = 0; index < Helper.listaHeatMapUmid.Count; index++)
                        {
                            Helper.DesenarePoligon(mMap, new LatLng(double.Parse(Helper.listaHeatMapUmid[index].LatA), double.Parse(Helper.listaHeatMapUmid[index].LngA)), new LatLng(double.Parse(Helper.listaHeatMapUmid[index].LatB), double.Parse(Helper.listaHeatMapUmid[index].LngB)), new LatLng(double.Parse(Helper.listaHeatMapUmid[index].LatC), double.Parse(Helper.listaHeatMapUmid[index].LngC)), new LatLng(double.Parse(Helper.listaHeatMapUmid[index].LatD), double.Parse(Helper.listaHeatMapUmid[index].LngD)), Helper.listaHeatMapUmid[index].Culoare);
                        }
                    }
                    else
                    {
                        for (int index = 0; index < Helper.listaHeatMapPres.Count; index++)
                        {
                            Helper.DesenarePoligon(mMap, new LatLng(double.Parse(Helper.listaHeatMapPres[index].LatA), double.Parse(Helper.listaHeatMapPres[index].LngA)), new LatLng(double.Parse(Helper.listaHeatMapPres[index].LatB), double.Parse(Helper.listaHeatMapPres[index].LngB)), new LatLng(double.Parse(Helper.listaHeatMapPres[index].LatC), double.Parse(Helper.listaHeatMapPres[index].LngC)), new LatLng(double.Parse(Helper.listaHeatMapPres[index].LatD), double.Parse(Helper.listaHeatMapPres[index].LngD)), Helper.listaHeatMapPres[index].Culoare);
                        }
                    }
                }
            });
        }

        private void BtnPres_Click(object sender, EventArgs e)
        {
            Helper.markersVisible = false;
            Helper.vizualizareaCurenta = Helper.Vizualizare.Presiune;
            mMap.Clear();
            for (int index = 0; index < Helper.listaHeatMapPres.Count; index++)
            {
                Helper.DesenarePoligon(mMap, new LatLng(double.Parse(Helper.listaHeatMapPres[index].LatA), double.Parse(Helper.listaHeatMapPres[index].LngA)), new LatLng(double.Parse(Helper.listaHeatMapPres[index].LatB), double.Parse(Helper.listaHeatMapPres[index].LngB)), new LatLng(double.Parse(Helper.listaHeatMapPres[index].LatC), double.Parse(Helper.listaHeatMapPres[index].LngC)), new LatLng(double.Parse(Helper.listaHeatMapPres[index].LatD), double.Parse(Helper.listaHeatMapPres[index].LngD)), Helper.listaHeatMapPres[index].Culoare);
            }
        }

        private void BtnUmid_Click(object sender, EventArgs e)
        {
            Helper.markersVisible = false;
            Helper.vizualizareaCurenta = Helper.Vizualizare.Umiditate;
            mMap.Clear();
            for (int index = 0; index < Helper.listaHeatMapUmid.Count; index++)
            {
                Helper.DesenarePoligon(mMap, new LatLng(double.Parse(Helper.listaHeatMapUmid[index].LatA), double.Parse(Helper.listaHeatMapUmid[index].LngA)), new LatLng(double.Parse(Helper.listaHeatMapUmid[index].LatB), double.Parse(Helper.listaHeatMapUmid[index].LngB)), new LatLng(double.Parse(Helper.listaHeatMapUmid[index].LatC), double.Parse(Helper.listaHeatMapUmid[index].LngC)), new LatLng(double.Parse(Helper.listaHeatMapUmid[index].LatD), double.Parse(Helper.listaHeatMapUmid[index].LngD)), Helper.listaHeatMapUmid[index].Culoare);
            }
        }

        private void BtnTemp_Click(object sender, EventArgs e)
        {
            Helper.markersVisible = false;
            Helper.vizualizareaCurenta = Helper.Vizualizare.Temperatura;
            mMap.Clear();
            for (int index = 0; index < Helper.listaHeatMapTemp.Count; index++)
            {
                Helper.DesenarePoligon(mMap, new LatLng(double.Parse(Helper.listaHeatMapTemp[index].LatA), double.Parse(Helper.listaHeatMapTemp[index].LngA)), new LatLng(double.Parse(Helper.listaHeatMapTemp[index].LatB), double.Parse(Helper.listaHeatMapTemp[index].LngB)), new LatLng(double.Parse(Helper.listaHeatMapTemp[index].LatC), double.Parse(Helper.listaHeatMapTemp[index].LngC)), new LatLng(double.Parse(Helper.listaHeatMapTemp[index].LatD), double.Parse(Helper.listaHeatMapTemp[index].LngD)), Helper.listaHeatMapTemp[index].Culoare);
            }
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

