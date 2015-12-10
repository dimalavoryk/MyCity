namespace MyNewProject
{
	using System;
	using System.Collections.Generic;
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

				work.ExportBitmapAsJPG (App.bitmap, MainActivity.index);
				save.ExportFilesOnServer (MainActivity.type, MainActivity.latitude, MainActivity.longitude, MainActivity.index);
				_imageView.SetImageBitmap (App.bitmap);
				App.bitmap = null;
			}
			// Dispose of the Java side bitmap.
			GC.Collect();
		}

		WorkingWithFiles work;
		SavingToServer save;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.LoadedPhotos);
			Parse.ParseClient.Initialize("ZF2JYEfxIM7QyKVdOBn0AJEOUr1Mj5h1UMKsWqeC",
				"CEkjpD569RxuYtIYcJ9SNLMDt6FfL76fjJ48Qe3z");
			work = new WorkingWithFiles ();
			save = new SavingToServer ();
			if (IsThereAnAppToTakePictures ()) 
			{
				CreateDirectoryForPictures ();
				_imageView = FindViewById<ImageView> (Resource.Id.imageView);
				Intent intent = new Intent(MediaStore.ActionImageCapture);
				App._file = new File(App._dir, "myPhoto_" + MainActivity.index.ToString() + ".jpg");

				intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));

				StartActivityForResult(intent, 0);

			}
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

		public override void OnBackPressed()
		{
			Intent intent = new Intent (this, typeof(MainActivity));
			StartActivity (intent);
		}
	}
}

