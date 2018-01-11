using Android.App;
using Android.Widget;
using Android.OS;
using Android.Runtime;
using Android.Content;
using Android.Views;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using System;
using System.Threading;
using System.Drawing;
using Android.Locations;
using Xamarin.Android;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections.Generic;
using Datc_Map2.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace Datc_Map2
{
    [Activity(Label = "Activity2")]
    public class Activity2 : Activity, IOnMapReadyCallback, ILocationListener
    {

        private Button btnNomral;
        private Button btnHybid;
        private Button btnSatellite;
        private Button btnTerrain;
        int a = 2;

        public static System.Timers.Timer timpActualizare = new System.Timers.Timer(100000);  //o data la 5s se apeleaza functia asta 

        private GoogleMap mMap;
        LocationManager locationManager;

        MapFragment mapFragment;
        MarkerOptions markerOptions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main2);

            // Create your application here
            timpActualizare.Enabled = true;
            timpActualizare.Elapsed += TimpActualizare_Elapsed;

            btnNomral = FindViewById<Button>(Resource.Id.btnNomral);
            btnHybid = FindViewById<Button>(Resource.Id.btnHybid);
            btnSatellite = FindViewById<Button>(Resource.Id.btnSatellite);
            btnTerrain = FindViewById<Button>(Resource.Id.btnTerrain);


            btnNomral.Click += btnNomral_Click;
            btnHybid.Click += btnHybid_Click;
            btnSatellite.Click += btnSatellite_Click;
            btnTerrain.Click += btnTerrain_Click;

            FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
        }


        private void btnTerrain_Click(object sender, EventArgs e)
        {
            mMap.MapType = GoogleMap.MapTypeTerrain;
        }

        private void btnSatellite_Click(object sender, EventArgs e)
        {
            mMap.MapType = GoogleMap.MapTypeSatellite;
        }

        private void btnHybid_Click(object sender, EventArgs e)
        {
            mMap.MapType = GoogleMap.MapTypeHybrid;
        }

        private void btnNomral_Click(object sender, EventArgs e)
        {
            mMap.MapType = GoogleMap.MapTypeNormal;
        }

        /*private void SetUpMap()
        {
            if (mMap == null)
            {
                mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
                mapFragment.GetMapAsync(this);
            }
        }*/

        public void OnMapReady(GoogleMap googleMap)
        {

            markerOptions = new MarkerOptions();
            markerOptions.SetPosition(new LatLng(16.03, 108));
            markerOptions.SetTitle("MyPosition");
            googleMap.AddMarker(markerOptions);

            googleMap.UiSettings.ZoomControlsEnabled = true;
            googleMap.UiSettings.CompassEnabled = true;
            googleMap.MoveCamera(CameraUpdateFactory.ZoomIn());

            mMap = googleMap;

            mMap.MapType = GoogleMap.MapTypeHybrid;
            /* PolylineOptions rectangle = new PolylineOptions()
                 .Add(new LatLng(45.444073, 21.130679))
                 .Add(new LatLng(45.444070, 21.130679))
                 .Add(new LatLng(45.444070, 21.130676))
                 .Add(new LatLng(45.444073, 21.130676))
                 .Add(new LatLng(45.444073, 21.130679));

             Polyline polyline = mMap.AddPolyline(rectangle);
             */


            // DataBase date = new DataBase();
            // date.GetInfoDate();

            /*  RunOnUiThread(() =>    ///aici desenez datele 

              List<CircleOptions> circles = new List<CircleOptions>();
              double lat = 45.744594, longit = 21.218611,la=0 ,lo =0;
              for (int i = 0; i < 10; i++)
              {
                  CircleOptions circle = new CircleOptions();
                  circle.InvokeRadius(0.7);
                  circle.InvokeCenter(new LatLng(lat + la, longit + lo));
                  circle.InvokeFillColor(Android.Graphics.Color.DarkCyan);
                  circle.InvokeStrokeColor(Android.Graphics.Color.DarkCyan);
                  la =  i*0.000019;
                  lo =  -i*0.000019;

                  mMap.AddCircle(circle);
                  circles.Add(circle);
                  circle.Clickable(true);

                  // mMap.CircleClick += MMap_CircleClick;

                  /*   mBtnSignUp.Click += (object sender, EventArgs args) =>
                     {
                         //Pull up dialog
                         FragmentTransaction transaction = FragmentManager.BeginTransaction();
                         DialogSignUp signUp = new DialogSignUp();
                         signUp.Show(transaction, "dialog_fragment");

                         signUp.mOnSignUpInComplete += SignUp_mOnSignUpInComplete;

                     };   */
            /*   mMap.CircleClick += (object sender,EventArgs args) =>
               {
                   FragmentTransaction transaction = FragmentManager.BeginTransaction();


               }
             }*/
            InitCameraPosition();
        }

        private void InitCameraPosition()
        {
            LatLng location = new LatLng(45.74481, 21.21836);
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(18);
            // builder.Bearing(155);
            // builder.Tilt(65);

            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            mMap.MoveCamera(cameraUpdate);
        }

        public void OnLocationChanged(Location location)
        {
            throw new NotImplementedException();
        }

        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }



        private void TimpActualizare_Elapsed(object sender, System.Timers.ElapsedEventArgs e) // 
        {
            //citesc din baza e date prin API
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/hal+json");
            var Home = "https://datcproiectginbell.azurewebsites.net/api/Mobile";  //   /api/Mobile ( controler ) 
            var response = client.GetAsync(Home).Result;
            string data = response.Content.ReadAsStringAsync().Result;
            List<Senzor> listaSenzori = JsonConvert.DeserializeObject<List<Senzor>>(data);
            List<Senzor> listFinal = listaSenzori.GetRange(listaSenzori.Count - 10, 10);

            //in functie de data.Value( 0 ,1 ,2 ) imi pun culorile.

            //  RunOnUiThread(() =>    ///aici desenez datele 
            //{
            List<CircleOptions> circles = new List<CircleOptions>();
            double lat = 45.744594, longit = 21.218611, la = 0, lo = 0;
            for (int i = 0; i < 10; i++)
            {
                var color = Android.Graphics.Color.White;
                if (a == 3)
                {
                    try
                    {

                        foreach (var s in listFinal)
                        {
                            switch (s.Valoare)
                            {
                                case 0:
                                    color = Android.Graphics.Color.Red;
                                    break;
                                case 1:
                                    color = Android.Graphics.Color.Gold;
                                    break;
                                case 2:
                                    color = Android.Graphics.Color.Green;
                                    break;
                                default:
                                    color = Android.Graphics.Color.White;
                                    break;
                            }

                        }
                    }
                    catch (Exception exp)
                    {
                        //RIP
                    }
                    //  color = Android.Graphics.Color.Black;
                }



                CircleOptions circle = new CircleOptions();
                circle.InvokeRadius(0.7);
                circle.InvokeCenter(new LatLng(lat + la, longit + lo));

                circle.InvokeFillColor(color);
                circle.InvokeStrokeColor(color);
                la = i * 0.000019;
                lo = -i * 0.000019;



                mMap.AddCircle(circle);
                circles.Add(circle);
                circle.Clickable(true);



                // mMap.CircleClick += MMap_CircleClick;

                /*   mBtnSignUp.Click += (object sender, EventArgs args) =>
                   {
                       //Pull up dialog
                       FragmentTransaction transaction = FragmentManager.BeginTransaction();
                       DialogSignUp signUp = new DialogSignUp();
                       signUp.Show(transaction, "dialog_fragment");

                       signUp.mOnSignUpInComplete += SignUp_mOnSignUpInComplete;

                   };   */
                /*   mMap.CircleClick += (object sender,EventArgs args) =>
                   {
                       FragmentTransaction transaction = FragmentManager.BeginTransaction();

                   }*/
            }
            a = 3;
        //}//);

        }

    }



}