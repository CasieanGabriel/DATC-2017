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

namespace Datc_Map2
{

    public class OnSignUpInEventArgs : EventArgs //custom sign up/in event args
    {
        private string mFirstName;
        private string mPassword;

        public string FirstName
        {
            get { return mFirstName; }
            set { mFirstName = value; }
        }
        public string Password
        {
            get { return mPassword; }
            set { mPassword = value; }
        }

        public OnSignUpInEventArgs(string firstName,string password) :base()
        {
            FirstName = firstName; // propietatile
            Password = password;
        }
    }

    class DialogSignUp:DialogFragment
    {

        private EditText mTxtFirstName;
        private EditText mTxtPassword;
        private Button mBtnSignUpIn;

        public event EventHandler<OnSignUpInEventArgs> mOnSignUpInComplete;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            //create the view the dialog is going to hold
            var view = inflater.Inflate(Resource.Layout.dialog_sign_up, container, false);
            mTxtFirstName = view.FindViewById<EditText>(Resource.Id.txtFirstName);
            mTxtPassword = view.FindViewById<EditText>(Resource.Id.txtPassword);
            mBtnSignUpIn = view.FindViewById<Button>(Resource.Id.btnDialogEmail);

            mBtnSignUpIn.Click += MBtnSignUpIn_Click;
            return view;



        }

        void MBtnSignUpIn_Click(object sender, EventArgs e)
        {         
            //User has clicked the sign up button
            //Custom events arguments - subclass EventArgs to fire an event
            mOnSignUpInComplete.Invoke(this, new OnSignUpInEventArgs(mTxtFirstName.Text, mTxtPassword.Text));
            this.Dismiss();

        }
    }
}