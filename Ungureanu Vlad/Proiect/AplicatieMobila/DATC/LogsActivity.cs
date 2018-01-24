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
using Newtonsoft.Json;
using System.Net.Http;

namespace DATC
{
    [Activity(Label = "Logs")]
    public class LogsActivity : Activity
    {
        static HttpClient client = new HttpClient();
        private List<Log> listaLoguri;
        private ListView lstViewLogs;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LogsView);
            lstViewLogs = FindViewById<ListView>(Resource.Id.listViewLogs);
            listaLoguri = new List<Log>();
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle("Conectare esuata");
            alertDialog.SetNeutralButton("OK", delegate { alertDialog.Dispose(); });
            alertDialog.SetMessage("Verificati conexiunea la internet");
            try
            {
                listaLoguri.Clear();
                var Home = "https://xdoit.azurewebsites.net/api/Logs";
                var response = client.GetAsync(Home).Result;
                string data = response.Content.ReadAsStringAsync().Result;
                listaLoguri = JsonConvert.DeserializeObject<List<Log>>(data);
            }
            catch (Exception e)
            {
                alertDialog.Show();
            }
            ListViewAdapter adapter = new ListViewAdapter(listaLoguri, this);
            lstViewLogs.Adapter = adapter;
        }
    }
}