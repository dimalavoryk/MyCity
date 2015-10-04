using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace MyNewProject
{
	[Activity (Label = "MyNewProject", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			Button reportProblem = FindViewById<Button> (Resource.Id.reportProblem);
			reportProblem.Click += ReportProblemClick;
			Button loadedPhotos = FindViewById<Button> (Resource.Id.loadedPhotos);
			loadedPhotos.Click += LoadedPhotos_Click;
		}

		private void LoadedPhotos_Click (object sender, EventArgs e)
		{
			Intent intent = new Intent (this, typeof(LoadedPhotos));
			StartActivity (intent);
		}


		private void ReportProblemClick (object sender, EventArgs e)
		{
			Intent intent = new Intent (this, typeof(ListOfProblemsActivity));
			StartActivity (intent);
		}




	}
}


