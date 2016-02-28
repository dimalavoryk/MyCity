using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using Android.Graphics;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Parse;


//using Java.Util;

namespace MyNewProject
{
	[Activity (Label = "Завантажені фото", ScreenOrientation =  Android.Content.PM.ScreenOrientation.Portrait)]			
	public class OnLoadedPhotosClick : Activity
	{
		bool onNextPageClick = false;
		bool onPreviousPageClick = false;
		int count = 0;
		const byte maxCountInView = 10;
		int pagesCount;
		int currentPageNumber = 0;
		int currentCount = 0;
		WebClient webClient;
		LinearLayout linearLayout;
		List<string> descriptionList = new List<string> ();
		List<ParseGeoPoint> locationList = new List<ParseGeoPoint> ();
		List<ParseFile> parseFiles = new List<ParseFile> ();
		List<string> addressList = new List<string> ();
		List<string> commentList = new List<string> ();
		Button nextPage, previousPage;

		ProgressDialog progress;

		TextView pageNumber;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			Parse.ParseClient.Initialize ("ZF2JYEfxIM7QyKVdOBn0AJEOUr1Mj5h1UMKsWqeC",
				"CEkjpD569RxuYtIYcJ9SNLMDt6FfL76fjJ48Qe3z");

			SetContentView (Resource.Layout.OnLoadedPhotosClickScreen);

			linearLayout = FindViewById<LinearLayout> (Resource.Id.linearLayout1);

			GetFilesFromParse ();

			nextPage = FindViewById<Button> (Resource.Id.nextPage);
			previousPage = FindViewById<Button> (Resource.Id.previousPage);
			pageNumber = FindViewById<TextView> (Resource.Id.pageNumber);

			nextPage.Enabled = false;
			previousPage.Enabled = false;

			nextPage.Click += NextPage_Click;
			previousPage.Click += PreviousPage_Click;


		}

		void PreviousPage_Click (object sender, EventArgs e)
		{
			linearLayout.RemoveAllViews ();
			GC.Collect ();
			DownloadPreviousPage ();
		}

		void NextPage_Click (object sender, EventArgs e)
		{
			linearLayout.RemoveAllViews ();
			GC.Collect ();
			DownloadNextPage ();
		}

		public void DownloadPreviousPage ()
		{
			int start, end;
			onPreviousPageClick = true;
			if (onNextPageClick) {
				currentCount -= maxCountInView;
				onNextPageClick = false;
			}

			if (currentCount <= 0) 
			{
				currentCount = count;
				end = currentCount;
				currentCount -= maxCountInView;
				start = currentCount;
				currentPageNumber = pagesCount;
			} 
			else 
			{
				end = currentCount;
				currentCount -= maxCountInView;
				--currentPageNumber;
				if (currentCount < 0) 
				{
					currentCount = 0;
					currentPageNumber = 1;
				}
				start = currentCount;
			}
			pageNumber.Text = currentPageNumber.ToString ();

			for (int i = start; i < end; ++i) 
			{
				downloadAsync (i);
			}

		}

		public void DownloadNextPage ()
		{
			onNextPageClick = true;
			if (onPreviousPageClick) {
				currentCount += maxCountInView;
				onPreviousPageClick = false;
			}
			int start = currentCount;
			int end;
			if (start >= count) 
			{
				start = 0;
				end = maxCountInView;
				currentCount = 0;
				currentPageNumber = 1;
			} 
			else 
			{
				++currentPageNumber;
				currentCount += maxCountInView;
				end = currentCount;

				if (end > count) {
					end = count;
				}
			}

			pageNumber.Text = currentPageNumber.ToString ();

			for (int i = start; i < end; ++i) 
			{
				downloadAsync (i);
			}

		}

		async void downloadAsync (int index)
		{
			webClient = new WebClient ();
			var url = new Uri (parseFiles[index].Url.ToString());
			byte[] bytes = null;

			try{
				bytes = await webClient.DownloadDataTaskAsync(url);
			}
			catch(TaskCanceledException){
				Console.WriteLine ("Task Canceled!");
				return;
			}
			catch(Exception e){
				Console.WriteLine (e.ToString());
				return;
			}
			string documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);	
			string localFilename = "downloaded" + index.ToString () + ".png";
			string localPath = System.IO.Path.Combine (documentsPath, localFilename);

			//Sive the Image using writeAsync
			FileStream fs = new FileStream (localPath, FileMode.OpenOrCreate);
			await fs.WriteAsync (bytes, 0, bytes.Length);

			Console.WriteLine("localPath:"+localPath);
			fs.Close ();

			var metrics = Resources.DisplayMetrics;
			var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
			var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);


			BitmapFactory.Options options = new BitmapFactory.Options ();
			options.InJustDecodeBounds = true;
			await BitmapFactory.DecodeFileAsync (localPath, options);

			options.InSampleSize = options.OutWidth > options.OutHeight ? options.OutHeight /  (heightInDp-50) : options.OutWidth /(widthInDp-50);
			options.InJustDecodeBounds = false;

			Bitmap bitmap = await BitmapFactory.DecodeFileAsync (localPath, options);

			CreateInfo (bitmap, index);

			Console.WriteLine ("Loaded!");

		}

		void CreateInfo (Bitmap bitmap, int index)
		{
			string coordinates = "Широта: " + locationList [index].Latitude.ToString () + "; Довгота: " + locationList [index].Longitude.ToString ();
			CreateTextView (descriptionList [index]);
			CreateTextView (coordinates);
			CreateTextView (addressList [index]);
			CreateTextView (commentList [index]);
			ImageView img = new ImageView (this);
			img.SetImageBitmap (bitmap);

			linearLayout.AddView (img);
		}

		void CreateTextView (string text)
		{
			TextView newTextview = new TextView (this);
			newTextview.Text = text;
			newTextview.SetTextColor (Color.Blue);
			newTextview.SetTextSize (Android.Util.ComplexUnitType.Pt, 10.0f);
			linearLayout.AddView (newTextview);
		}

		public async void GetFilesFromParse ()
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery ("problem");
			IEnumerable<ParseObject> relatedObjects = await query.FindAsync ();

			foreach (ParseObject problem in relatedObjects) {
				descriptionList.Add (problem.Get<string> ("description"));
				locationList.Add (problem.Get<ParseGeoPoint> ("coordinates"));
				parseFiles.Add (problem.Get<ParseFile> ("image"));
				addressList.Add (problem.Get<string> ("address"));
				commentList.Add (problem.Get<string> ("comment"));
				++count;
			}

			pagesCount = (count / maxCountInView);
			if (count % maxCountInView != 0) {
				++pagesCount;
			}
			nextPage.Enabled = true;
			previousPage.Enabled = true;

			DownloadNextPage ();

		}

		private int ConvertPixelsToDp(float pixelValue)
		{
			var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
			return dp;
		}
	}


}
		



/*
 * 		*/
