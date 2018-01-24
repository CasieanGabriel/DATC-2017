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

namespace DATC
{
    class ListViewAdapter : BaseAdapter<Log>
    {
        public List<Log> listLogs;
        private Context mContext;

        public ListViewAdapter(List<Log> listLogs, Context mContext)
        {
            this.listLogs = listLogs;
            this.mContext = mContext;
        }

        public override Log this[int position]
        {
            get { return listLogs[position]; }
        }

        public override int Count
        {
            get { return listLogs.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if(row==null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.Logs,null,false);
            }
            TextView txtId = row.FindViewById<TextView>(Resource.Id.txtId);
            txtId.Text = listLogs[position].IdSenzor.ToString();

            TextView txtEronat = row.FindViewById<TextView>(Resource.Id.txtEronat);
            txtEronat.Text = listLogs[position].ValoareEronata.ToString();

            TextView txtCorectat = row.FindViewById<TextView>(Resource.Id.txtCorectat);
            txtCorectat.Text = listLogs[position].ValoareCorecta.ToString();

            TextView txtTip = row.FindViewById<TextView>(Resource.Id.txtTip);
            txtTip.Text = listLogs[position].TipData;

            TextView txtData = row.FindViewById<TextView>(Resource.Id.txtData);
            txtData.Text = listLogs[position].Data ;
            return row;
        }
    }
}