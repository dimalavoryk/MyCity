using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Android.Graphics;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

//using Java.Util;

namespace MyNewProject
{
	[Activity (Label = "OnLoadedPhotosClick")]			
	public class OnLoadedPhotosClick : Activity
	{

		LinearLayout layot;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.OnLoadedPhotosClickScreen);
			layot = FindViewById<LinearLayout> (Resource.Id.linearLayout1);

			WorkingWithFiles import = new WorkingWithFiles ();

			// Now we add items to show them 
			// :TODO ДОДАТИ ЕКРАН ЗАГРУЗОЧНИЙ

			if (MainActivity.index > -1) 
			{

				for (int i = 0; i <= MainActivity.index; ++i) {
					ImageView img = new ImageView (this);
					TextView problems = new TextView (this);
					TextView coordinates = new TextView (this);

					problems.SetTextColor (Color.Black);
					coordinates.SetTextColor (Color.Black);
					problems.TextSize = 20;
					coordinates.TextSize = 20;



					img.SetImageBitmap (import.ImportBitmapFromFile(i));
					problems.Text = import.ImportTypeFromFile (i);
					coordinates.Text = "kek";//(import.ImportNumberFromFile ("latitude" + i + ".dat") + import.ImportNumberFromFile ("longitude" + i + ".dat")).ToString ();


					img.Visibility = ViewStates.Gone;
					coordinates.Visibility = ViewStates.Gone;


					problems.Click += (sender, e) => 
					{	
						if (img.Visibility == ViewStates.Gone)
						{
							img.Visibility = ViewStates.Visible;
							coordinates.Visibility = ViewStates.Visible;
						}
						else
						{
							img.Visibility = ViewStates.Gone;
							coordinates.Visibility = ViewStates.Gone;
						}
					};


					layot.AddView (problems);
					layot.AddView (coordinates);
					layot.AddView (img);

				}
			}
			// Create your application here
		}

	/*	public override void OnBackPressed()
		{
			Intent intent = new Intent (this, typeof(MainActivity));
			StartActivity (intent);
		}
*/	}



}

