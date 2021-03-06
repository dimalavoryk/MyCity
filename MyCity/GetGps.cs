﻿using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

using Parse;

namespace MyNewProject
{
	[Activity (Label = "Отримання GPS-координат", ScreenOrientation =  Android.Content.PM.ScreenOrientation.Portrait/*, MainLauncher = true*/)  ]			
	public class GetGps : Activity
	{
		GPSServiceBinder _binder;  
		GPSServiceConnection _gpsServiceConnection;  
		Intent _gpsServiceIntent;
		private GPSServiceReciever _receiver;  
		public static GetGps Instanse;
       


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
            SetContentView(Resource.Layout.StartScreen);

            Instanse = this;
			RegisterService ();

			// Create your application here
		}

		private void RegisterService()  
		{  
			_gpsServiceConnection = new GPSServiceConnection(_binder);  
			_gpsServiceIntent = new Intent(Android.App.Application.Context, typeof(GPSService));  
			BindService(_gpsServiceIntent, _gpsServiceConnection, Bind.AutoCreate);  
		}  
		private void RegisterBroadcastReceiver()  
		{  
			IntentFilter filter = new IntentFilter(GPSServiceReciever.LOCATION_UPDATED);  
			filter.AddCategory(Intent.CategoryDefault);  
			_receiver = new GPSServiceReciever();  
			RegisterReceiver(_receiver, filter);  
		}  

		private void UnRegisterBroadcastReceiver()  
		{  
			UnregisterReceiver(_receiver);  
		}   

		protected override void OnResume()  
		{  
			base.OnResume();  
			RegisterBroadcastReceiver();  
		}  

		protected override void OnPause()  
		{  
			base.OnPause();  
			UnRegisterBroadcastReceiver();  
		}

     //   public void StartMainActivity(Intent intent)
		public void StartNextSession(Intent intent)
		{
			intent = new Intent(this, typeof(ListOfProblemsActivity));
			Finish ();

			StartActivity(intent);
        }

		public override void OnBackPressed()
		{
		//	System.Environment.Exit(0);
			//Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
		}

		[BroadcastReceiver]  
		internal class GPSServiceReciever : BroadcastReceiver  
		{  
			public static readonly string LOCATION_UPDATED = "LOCATION_UPDATED";  
			public override void OnReceive(Context context, Intent intent)  
			{  
				if (intent.Action.Equals(LOCATION_UPDATED))  
				{
					GetGps.Instanse.StartNextSession(intent);
                }  

			}  
		} 
	}
}

