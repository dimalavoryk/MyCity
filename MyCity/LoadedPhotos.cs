
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

namespace MyNewProject
{
	[Activity (Label = "Завантажені фото")]			
	public class LoadedPhotos :Activity
	{
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.LoadedPhotos);


			// Create your application here
		}
	}
}

