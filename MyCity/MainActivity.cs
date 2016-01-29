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


using Parse;

namespace MyNewProject
{
	[Activity (Label = "MyCity", Icon = "@drawable/icon", MainLauncher = true)]
	public class MainActivity : Activity
	{

		public static bool isNeeded;
	//	public static MainActivity Instance;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
           // GetGps.index = -1;

          //  WorkingWithFiles work = new WorkingWithFiles();
          //  GetGps.index = work.ImportNumberFromFile("num002.dat");

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			Button exit = FindViewById<Button> (Resource.Id.Exit);
			exit.Click += Exit_Click;
			Button reportProblem = FindViewById<Button> (Resource.Id.reportProblem);
			reportProblem.Click += ReportProblemClick;
			Button loadedPhotos = FindViewById<Button> (Resource.Id.loadedPhotos);
			loadedPhotos.Click += LoadedPhotos_Click;


		}

		private void LoadedPhotos_Click (object sender, EventArgs e)
		{
			Intent intent = new Intent (this, typeof(OnLoadedPhotosClick));
			StartActivity (intent);
		}

		void Exit_Click (object sender, EventArgs e)
		{
			Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
		}


		private void ReportProblemClick (object sender, EventArgs e)
		{
			Intent intent = new Intent (this, typeof(ListOfProblemsActivity));
		//	Intent intent = new Intent(this, typeof(GetGps));
			isNeeded = true;
			StartActivity (intent);
		}
			
		public override void OnBackPressed()
		{
			//System.Environment.Exit(0);
			Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
		}

	}
}
