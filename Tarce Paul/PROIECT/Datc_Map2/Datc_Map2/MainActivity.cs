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

namespace Datc_Map2
{
    [Activity(Label = "Datc_Map2", MainLauncher = true)]
    public class MainActivity : Activity
    {
        

        private Button mBtnSignUp;
        private Button mBtnSignIn;
        private ProgressBar mProgressBar;
        private int checkProgressBarr = 0;
       

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Login);

         

            mBtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mBtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            



            mBtnSignUp.Click += (object sender, EventArgs args) =>
                 {
                    //Pull up dialog
                    FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    DialogSignUp signUp = new DialogSignUp();
                    signUp.Show(transaction, "dialog_fragment");

                     signUp.mOnSignUpInComplete += SignUp_mOnSignUpInComplete;
 
                 };
            mBtnSignIn.Click += (object sender, EventArgs args) =>
                {
                    FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    DialogSignUp signIn = new DialogSignUp();
                    signIn.Show(transaction, "dialog_fragment");

                    signIn.mOnSignUpInComplete += SignIn_mOnSignUpInComplete;

                    
                };

            //mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            // mapFragment.GetMapAsync(this);
          //  FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);

            // SetUpMap();


        }

        private void SignUpInThreads()
        {
            mProgressBar.Visibility = ViewStates.Visible;
            Thread thread = new Thread(ActLikeARrequest);
            thread.Start();

            

            // dupa 3 secunde
            //check response ...=> send to next activitu
        }
        private void SignUp_mOnSignUpInComplete(object sender, OnSignUpInEventArgs e)
        {
            checkProgressBarr = 0;
            SignUpInThreads();
        }

        private void SignIn_mOnSignUpInComplete(object sender, OnSignUpInEventArgs e)
        {
            checkProgressBarr = 0;
            SignUpInThreads();
        }
        
        private void ActLikeARrequest()
        {
            Thread.Sleep(3000);
            checkProgressBarr = 1;
            //
            RunOnUiThread(() => { mProgressBar.Visibility = ViewStates.Invisible; });

            if (checkProgressBarr == 1)
            {
                Intent intent = new Intent(this, typeof(Activity2));
                this.StartActivity(intent);
            }

        }




    }

}

