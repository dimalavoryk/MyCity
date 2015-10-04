
using System;
using System.Collections.Generic;
using Android.App;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Uri = Android.Net.Uri;

namespace MyNewProject
{
	[Activity (Label = "Завантажити з галереї")]			
	public class UploadFromGallery : Activity
	{

		public static readonly int PickImageId = 1000;
		private ImageView _imageView;

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null)) 
			{
				Uri uri = data.Data;
				_imageView.SetImageURI (uri);
			}
		}
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.LoadedPhotos);
			_imageView = FindViewById<ImageView> (Resource.Id.imageView);
			Intent = new Intent ();
			Intent.SetType ("image/*");
			Intent.SetAction (Intent.ActionGetContent);
			StartActivityForResult (Intent.CreateChooser (Intent, "Оберіть фото"), PickImageId);
			/*
			Button uploadFromGallery = FindViewById<Button> (Resource.Id.uploadFromgGallery);
			uploadFromGallery.Click += UploadFromGallery_Click;*/

			// Create your application here
		}

		/*void UploadFromGallery_Click (object sender, EventArgs e)
		{
			Intent = new Intent ();
			Intent.SetType ("image/*");
			Intent.SetAction (Intent.ActionGetContent);
			StartActivityForResult (Intent.CreateChooser (Intent, "Оберіть фото"), PickImageId);
		}*/
	}
}

