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
	[Activity (Label = "Головне Меню", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		static public string type;
		static public double latitude;
		static public double longitude;
		static public bool flag;

		static public int index;

	//	public static MainActivity Instance;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);


		//	Instance = this;
			index = -1;

			WorkingWithFiles work = new WorkingWithFiles ();
			index = work.ImportNumberFromFile ("num002.dat");

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			Button reportProblem = FindViewById<Button> (Resource.Id.reportProblem);
			reportProblem.Click += ReportProblemClick;
			Button loadedPhotos = FindViewById<Button> (Resource.Id.loadedPhotos);
			loadedPhotos.Click += LoadedPhotos_Click;
	/*		if (!flag) 
			{
				Intent intent = new Intent (this, typeof(GetGps));
				StartActivity (intent);
			}
		*/}

		private void LoadedPhotos_Click (object sender, EventArgs e)
		{
			Intent intent = new Intent (this, typeof(OnLoadedPhotosClick));
			StartActivity (intent);
		}


		private void ReportProblemClick (object sender, EventArgs e)
		{
			Intent intent = new Intent (this, typeof(ListOfProblemsActivity));
			StartActivity (intent);
		}
			
		public override void OnBackPressed()
		{
			System.Environment.Exit(0);
		}

	}
}
