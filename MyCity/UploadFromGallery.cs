using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
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
		ProgressDialog progress;
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

				Task.Factory.StartNew (
					// tasks allow you to use the lambda syntax to pass work
					() => 
					{
						string sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
						string name = "image.jpg";
						string filePath = System.IO.Path.Combine(sdCardPath, name);
						var _stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);

						bitmap.Compress (Bitmap.CompressFormat.Jpeg, 20, _stream);
						_stream.Close ();	

	//					System.IO.Stream fileStream = new System.IO.MemoryStream();
	//					App.bitmap.Compress(Bitmap.CompressFormat.Jpeg, 5, fileStream);
	//					fileStream.Close();
	//					ExportFilesOnParse(GetGps.type, GetGps.latitude, GetGps.longitude, GetGps.address, fileStream);
					}
					// ContinueWith allows you to specify an action that runs after the previous thread
					// completes
					// 
					// By using TaskScheduler.FromCurrentSyncrhonizationContext, we can make sure that 
					// this task now runs on the original calling thread, in this case the UI thread
					// so that any UI updates are safe. in this example, we want to hide our overlay, 
					// but we don't want to update the UI from a background thread.
				).ContinueWith ( 
					t => {
						if (progress != null)
							progress.Hide();
						//save.ExportFilesOnServer(GetGps.type, GetGps.latitude, GetGps.longitude, 1);
						ExportFilesOnParse(GetGps.type, GetGps.latitude, GetGps.longitude, GetGps.address, GetGps.comment);
					/*	double lat = GetGps.latitude;
						double lng = GetGps.longitude;
						if (lat == 0 || lng == 0)
						{
							lat = work.ImportCoordinatesFromFile("latitude.dat");
							lng = work.ImportCoordinatesFromFile("longitude.dat");
							work.ExportCoordinatesInFile(lat, "latitude" + (GetGps.index).ToString() + ".dat");
							work.ExportCoordinatesInFile(lng, "longitude" + (GetGps.index).ToString() + ".dat");
						}
                        save.ExportFilesOnServer(GetGps.type, lat, lng, GetGps.index);
						//Console.WriteLine ( "Finished, hiding our loading overlay from the UI thread." );
				*/	}, TaskScheduler.FromCurrentSynchronizationContext()
				);
			}
		}
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			Parse.ParseClient.Initialize("ZF2JYEfxIM7QyKVdOBn0AJEOUr1Mj5h1UMKsWqeC",
				"CEkjpD569RxuYtIYcJ9SNLMDt6FfL76fjJ48Qe3z");
			save = new SavingToServer ();
			work = new WorkingWithFiles ();
			SetContentView (Resource.Layout.LoadedPhotos);

			Button backToMainMenu = FindViewById<Button> (Resource.Id.BackToMainMenu);
			backToMainMenu.Click += BackToMainMenu_Click;

			_imageView = FindViewById<ImageView> (Resource.Id.imageView);
			Intent = new Intent ();
			Intent.SetType ("image/*");
			Intent.SetAction (Intent.ActionGetContent);
			StartActivityForResult (Intent.CreateChooser (Intent, "Оберіть фото"), PickImageId);
		}

		void BackToMainMenu_Click (object sender, EventArgs e)
		{
			Intent intent = new Intent (this, typeof(MainActivity));
			StartActivity (intent);
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


		public async void ExportFilesOnParse (string type,  double latitude, double longitude, string address, string comment)
		{
			ParseGeoPoint location = new ParseGeoPoint (latitude, longitude);
			string sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			string name = "image.jpg";
			string filePath = System.IO.Path.Combine(sdCardPath, name);
			byte[] data = System.IO.File.ReadAllBytes (filePath);

			ParseFile file = new ParseFile (name, data);

			var problemObject = new ParseObject ("problem");
			problemObject ["description"] = type;
			problemObject ["coordinates"] = location;
			problemObject ["image"] = file;
			problemObject ["address"] = address;
			problemObject ["comment"] = comment;
			Android.Widget.Toast.MakeText(this, "it's work", Android.Widget.ToastLength.Short).Show();
			await problemObject.SaveAsync ();
		}



		public override void OnBackPressed()
		{
			Intent intent = new Intent (this, typeof(MainActivity));
			StartActivity (intent);
		}
	}
}

