using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
//using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Net;

using Environment = Android.OS.Environment;
using Parse;
//using Uri = Android.Net.Uri;
using Java.IO;


namespace MyNewProject
{
	[Activity (Label = "Завантажити з галереї")]			
	public class UploadFromGallery : Activity
	{

		WorkingWithFiles work;
		SavingToServer save;

		private void PickSelected (ImageView selectedPic)
		{
			
		}



		public static readonly int PickImageId = 1000;
		private ImageView _imageView;



		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null)) 
			{
		//		Stream stream = ContentResolver.OpenInputStream (data);
	//			Uri uri = data.Data;
	//			_imageView.SetImageURI (uri);
				Stream stream = ContentResolver.OpenInputStream(data.Data);
		

				_imageView.SetImageBitmap (DecodeBitmapFromStream (data.Data, 150, 150));
			//	Bitmap bitmap = DecodeBitmapFromStream (data.Data, 150, 150);
				Bitmap bitmap = BitmapFactory.DecodeStream (stream);


				work.ExportBitmapAsJPG (bitmap, MainActivity.index);

				save.ExportFilesOnServer (MainActivity.type, MainActivity.latitude, MainActivity.longitude, MainActivity.index);


				//Android.Net.Uri uri = new Android.Net.Uri ("../Application");
			}
		}
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			Parse.ParseClient.Initialize("ZF2JYEfxIM7QyKVdOBn0AJEOUr1Mj5h1UMKsWqeC",
				"CEkjpD569RxuYtIYcJ9SNLMDt6FfL76fjJ48Qe3z");
			work = new WorkingWithFiles ();
			save = new SavingToServer ();
			SetContentView (Resource.Layout.LoadedPhotos);
			_imageView = FindViewById<ImageView> (Resource.Id.imageView);
			Intent = new Intent ();
			Intent.SetType ("image/*");
			Intent.SetAction (Intent.ActionGetContent);
			StartActivityForResult (Intent.CreateChooser (Intent, "Оберіть фото"), PickImageId);

			

			// Create your application here
		}
		private Bitmap DecodeBitmapFromStream (Android.Net.Uri data, int requestedWidth, int requestedHeigth)
		{
			Stream stream = ContentResolver.OpenInputStream (data);	
			BitmapFactory.Options options = new BitmapFactory.Options ();
			options.InJustDecodeBounds = true;
			BitmapFactory.DecodeStream (stream);

			//  calculate InSampleSize
			options.InSampleSize = CalculateInSampleSize (options, requestedWidth, requestedHeigth);

			// Decode Bitmap with SimpleSize

			stream = ContentResolver.OpenInputStream (data); // must read again
			options.InJustDecodeBounds = false;
			Bitmap bitmap = BitmapFactory.DecodeStream (stream, null, options);
			return bitmap;

		}


		private int CalculateInSampleSize (BitmapFactory.Options options, int requestedWidth, int requestedHeigth)
		{
			int height = options.OutHeight;
			int width = options.OutWidth;
			int inSampleSize = 1;
			if (height > requestedHeigth || width > requestedWidth) 
			{
				//the image is bigger than we want it to be
				int halfHeigth = height / 2;
				int halfWidth = width / 2;

				while ((halfHeigth / inSampleSize) > requestedHeigth && (halfWidth / inSampleSize) > requestedWidth)
				{
					inSampleSize *= 2;
				}
			}

			return inSampleSize;
		}

		public override void OnBackPressed()
		{
			Intent intent = new Intent (this, typeof(MainActivity));
			StartActivity (intent);
		}
	}
}

