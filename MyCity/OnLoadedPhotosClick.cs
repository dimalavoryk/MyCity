using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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


			// Now we add items to show them 
			// :TODO ДОДАТИ ЕКРАН ЗАГРУЗОЧНИЙ

			if (MainActivity.problems [0].ProblemsItems.GetImgBitmap () != null) 
			{

				for (int i = 0; i <= MainActivity.index; ++i) {
					ImageView img = new ImageView (this);
					TextView problems = new TextView (this);
					TextView coordinates = new TextView (this);
					TextView street = new TextView (this);

					img.SetImageBitmap (MainActivity.problems [i].ProblemsItems.GetImgBitmap ());
					problems.Text = MainActivity.problems [i].GetStringType ();
					coordinates.Text = MainActivity.problems [i].ProblemsItems.GetCoordinates ();
					street.Text = MainActivity.problems [i].ProblemsItems.GetStreet ();


					img.Visibility = ViewStates.Gone;
					coordinates.Visibility = ViewStates.Gone;
					street.Visibility = ViewStates.Gone;


					problems.Click += (sender, e) => 
					{	
						if (img.Visibility == ViewStates.Gone)
						{
							img.Visibility = ViewStates.Visible;
							coordinates.Visibility = ViewStates.Visible;
							street.Visibility = ViewStates.Visible;
						}
						else
						{
							img.Visibility = ViewStates.Gone;
							coordinates.Visibility = ViewStates.Gone;
							street.Visibility = ViewStates.Gone;
						}
					};


					layot.AddView (problems);
					layot.AddView (coordinates);
					layot.AddView (street);

					layot.AddView (img);

				}
			}
			// Create your application here
		}
	}
}

