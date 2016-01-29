using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
//using Android.Runtime;
//using Android.Views;
using Android.Widget;

namespace MyNewProject
{
	[Activity (Label = "LoadPhotos")]			
	public class LoadPhotos : Activity
	{
		private Button takeAPhoto;
		private Button uploadFromGallery;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.AddPhotos);
			takeAPhoto = FindViewById<Button> (Resource.Id.TakeAPhoto);
			takeAPhoto.Click += TakeAPhoto_Click;;
			uploadFromGallery = FindViewById<Button> (Resource.Id.uploadFromgGallery);
			uploadFromGallery.Click += UploadFromGallery_Click;
			// Create your application here
		}

		void UploadFromGallery_Click (object sender, EventArgs e)
		{
			Intent intent1 = new Intent (this, typeof(UploadFromGallery));
			StartActivity (intent1);
		}

		void TakeAPhoto_Click (object sender, EventArgs e)
		{
			Intent intent = new Intent (this, typeof(TakeAPhoto));
			StartActivity (intent);
			//			
		}


		public override void OnBackPressed()
		{
		//	Intent intent = new Intent (this, typeof(MainActivity));
		//	StartActivity (intent);
		}


	}
}

