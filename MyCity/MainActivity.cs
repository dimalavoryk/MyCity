using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

using Java.IO;

using Parse;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyNewProject
{
	[Activity (Label = "My City", Icon = "@drawable/icon", MainLauncher = true, ScreenOrientation =  Android.Content.PM.ScreenOrientation.Portrait)]
	public class MainActivity : Activity
	{
		static public string type;
		static public double latitude;
		static public double longitude;
		static public string address;
		static public string comment;

		static public bool isStarted = false;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			Button exit = FindViewById<Button> (Resource.Id.Exit);
			exit.Click += Exit_Click;
			Button reportProblem = FindViewById<Button> (Resource.Id.reportProblem);
			reportProblem.Click += ReportProblemClick;
			Button loadedPhotos = FindViewById<Button> (Resource.Id.loadedPhotos);
			loadedPhotos.Click += LoadedPhotos_Click;
			Button markersOnMap = FindViewById<Button> (Resource.Id.seeOnMap);
			markersOnMap.Click += MarkersOnMap_Click;
		}

		void MarkersOnMap_Click (object sender, EventArgs e)
		{
			/*var uri = Android.Net.Uri.Parse ("http://myrivne.parseapp.com/ShowingMarkersOnMap.html");
			var intent = new Intent (Intent.ActionView, uri);  
		*/
			Intent intent = new Intent (this, typeof(ShowMap));
			StartActivity (intent);
		}

		private void LoadedPhotos_Click (object sender, EventArgs e)
		{
			Intent intent = new Intent (this, typeof(OnLoadedPhotosClick));
			StartActivity (intent);
		}

		void Exit_Click (object sender, EventArgs e)
		{
		//	Finish ();
			Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
		}


		private void ReportProblemClick (object sender, EventArgs e)
		{
			isStarted = true;
			Intent intent = new Intent(this, typeof(GetGps));
			StartActivity (intent);
		}
			
		public override void OnBackPressed()
		{
			//System.Environment.Exit(0);
			Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
		}

	}
}
