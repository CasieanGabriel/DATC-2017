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

namespace Datc_Map2.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string accses_token { get; set; } 
        public string error_description { get; set; }
        public DateTime expire_date { get; set; }

        public Token()
        {

        }
    }
}