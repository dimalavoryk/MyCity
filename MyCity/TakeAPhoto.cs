namespace MyNewProject
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using Android.App;
	using Android.Content;
	using Android.Content.PM;
	using Android.Graphics;
	using Android.Graphics.Drawables;
	using Android.OS;
	using Android.Provider;
	using Android.Widget;
	using Java.IO;
	using Environment = Android.OS.Environment;
	using Uri = Android.Net.Uri;

	using Parse;


	public static class App 
	{
		public static File _file;
		public static File _dir;     
		public static Bitmap bitmap;
	}

	public static class BitmapHelpers
	{
		/// <summary>
		/// This method will recyle the memory help by a bitmap in an ImageView
		/// </summary>
		/// <param name="imageView">Image view.</param>
		public static void RecycleBitmap(this ImageView imageView)
		{
			if (imageView == null) {
				return;
			}

			Drawable toRecycle = imageView.Drawable;
			if (toRecycle != null) {
				((BitmapDrawable)toRecycle).Bitmap.Recycle ();
			}
		}
			
		public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
		{
			// First we get the the dimensions of the file on disk
			BitmapFactory.Options options = new BitmapFactory.Options
			{
				InPurgeable = true,
				InJustDecodeBounds = true
			};
			BitmapFactory.DecodeFile(fileName, options);

			// Next we calculate the ratio that we need to resize the image by
			// in order to fit the requested dimensions.
			int outHeight = options.OutHeight;
			int outWidth = options.OutWidth;
			int inSampleSize = 1;

			if (outHeight > height || outWidth > width)
			{
				inSampleSize = outWidth > outHeight
					? outHeight / height
					: outWidth / width;
			}

			// Now we will load the image and have BitmapFactory resize it for us.
			options.InSampleSize = inSampleSize;
			options.InJustDecodeBounds = false;
			Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

			return resizedBitmap;
		}
	}

	[Activity (Label = "Сфотографувати")]			
	public class TakeAPhoto : Activity
	{
		ProgressDialog progress;
		private ImageView _imageView;
	//	private Button takeAPhoto;
//		SaveBitmap img;

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			// Make it available in the gallery
			Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
			Uri contentUri = Uri.FromFile(App._file);
			mediaScanIntent.SetData(contentUri);
			SendBroadcast(mediaScanIntent);

			// Display in ImageView. We will resize the bitmap to fit the display
			// Loading the full sized image will consume to much memory 
			// and cause the application to crash.

			int height = Resources.DisplayMetrics.HeightPixels;
			int width = _imageView.Height;
			App.bitmap = App._file.Path.LoadAndResizeBitmap (width, height);

//			img.AddItem (App.bitmap);
//			img.WriteList ("1");

			if (App.bitmap != null) 
			{

				Task.Factory.StartNew (
					// tasks allow you to use the lambda syntax to pass work
					() => 
					{
						string sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                        string name = "image.jpg";
						string filePath = System.IO.Path.Combine(sdCardPath, name);
						var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
						App.bitmap.Compress (Bitmap.CompressFormat.Jpeg, 20, fileStream);

						fileStream.Close() ;

//						System.IO.Stream fileStream = new System.IO.MemoryStream();
//						App.bitmap.Compress(Bitmap.CompressFormat.Jpeg, 5, fileStream);
//						ExportFilesOnParse(GetGps.type, GetGps.latitude, GetGps.longitude, GetGps.address, fileStream);

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
						ExportFilesOnParse(GetGps.type, GetGps.latitude, GetGps.longitude, GetGps.address, GetGps.comment);
			/*			double lat = GetGps.latitude;
						double lng = GetGps.longitude;
						if (lat == 0 || lng == 0)
						{
							lat = work.ImportCoordinatesFromFile("latitude.dat");
							lng = work.ImportCoordinatesFromFile("longitude.dat");
							work.ExportCoordinatesInFile(lat, "latitude" + (GetGps.index).ToString() + ".dat");
							work.ExportCoordinatesInFile(lng, "longitude" + (GetGps.index).ToString() + ".dat");
						}
						save.ExportFilesOnServer(GetGps.type, lat, lng, GetGps.index);
			*/			//Console.WriteLine ( "Finished, hiding our loading overlay from the UI thread." );
					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
				_imageView.SetImageBitmap (App.bitmap);
			//	App.bitmap = null;
			}
			// Dispose of the Java side bitmap.
			GC.Collect();
		}

		SavingToServer save;
		WorkingWithFiles work;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.LoadedPhotos);
			Parse.ParseClient.Initialize("ZF2JYEfxIM7QyKVdOBn0AJEOUr1Mj5h1UMKsWqeC",
				"CEkjpD569RxuYtIYcJ9SNLMDt6FfL76fjJ48Qe3z");
			save = new SavingToServer ();
			work = new WorkingWithFiles ();

			Button backToMainMenu = FindViewById<Button> (Resource.Id.BackToMainMenu);
			backToMainMenu.Click += BackToMainMenu_Click;

			if (IsThereAnAppToTakePictures ()) 
			{
				CreateDirectoryForPictures ();
				_imageView = FindViewById<ImageView> (Resource.Id.imageView);
				Intent intent = new Intent(MediaStore.ActionImageCapture);
                App._file = new File(App._dir, "myPhoto_" /*+ GetGps.index.ToString()*/ + ".jpg");

				intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));

				StartActivityForResult(intent, 0);

			}
		}

		void BackToMainMenu_Click (object sender, EventArgs e)
		{
			Intent intent = new Intent (this, typeof(MainActivity));
			StartActivity (intent);
		}
		private void CreateDirectoryForPictures()
		{
			App._dir = new File(
				Environment.GetExternalStoragePublicDirectory(
					Environment.DirectoryPictures), "Проблеми_Рівного");
			if (!App._dir.Exists())
			{
				App._dir.Mkdirs();
			}	
		}

		private bool IsThereAnAppToTakePictures()
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			IList<ResolveInfo> availableActivities = 
				PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
			return availableActivities != null && availableActivities.Count > 0;
		}


		public async void ExportFilesOnParse (string type,  double latitude, double longitude, string address, string comment)
		{
			ParseGeoPoint location = new ParseGeoPoint (latitude, longitude);
			string sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			string name = "image.jpg";
			string filePath = System.IO.Path.Combine(sdCardPath, name);
			byte[] data = System.IO.File.ReadAllBytes (filePath);

			ParseFile file = new ParseFile (name, data);

			//		stream.Close ();
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

