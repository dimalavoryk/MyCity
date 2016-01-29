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
	[Activity (Label = "OnLoadedPhotosClick")]			
	public class OnLoadedPhotosClick : Activity
	{
		WebClient webClient;
		//public Task<IEnumerable<ParseObject>> results;
		public async void GetFiles ()
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("problem");
			IEnumerable<ParseObject> relatedObjects = await query.FindAsync();
			List<string> problemsTxtList = new List<string> ();
			List<ParseGeoPoint> location = new List<ParseGeoPoint> ();
			List<ParseFile> parseFiles = new List<ParseFile> ();
			List<string> addressList = new List<string> ();
			List<string> commentList = new List<string> ();
			int i = 0;
			foreach (ParseObject problem in relatedObjects) {
				problemsTxtList.Add(problem.Get<string>("description"));
				location.Add(problem.Get<ParseGeoPoint>("coordinates"));
				parseFiles.Add (problem.Get<ParseFile> ("image"));
				addressList.Add (problem.Get<string> ("address"));
				commentList.Add (problem.Get<string> ("comment"));

				ImageView img = new ImageView (this);
				TextView problemTXT = new TextView(this);
				TextView coordinates = new TextView (this);
				TextView address = new TextView (this);
				TextView comment = new TextView (this);

				problemTXT.SetTextColor (Color.Black);
				coordinates.SetTextColor (Color.Black);
				address.SetTextColor (Color.Black);
				comment.SetTextColor (Color.Black);


				problemTXT.TextSize = 20;
				coordinates.TextSize = 20;
				address.TextSize = 20;
				comment.TextSize = 20;

				problemTXT.Text = problemsTxtList [i];
				coordinates.Text = "latitude" + location [i].Latitude.ToString () + "; longitude" + location [i].Longitude.ToString ();
				address.Text = addressList [i];
				comment.Text = commentList [i];

				downloadAsync (img, parseFiles [i].Url.ToString(), problemTXT, coordinates, address, comment);
				++i;
			}

		}

		async void downloadAsync(ImageView img, string _url, TextView problemTXT, TextView coordinates, TextView address, TextView comment)
		{
			webClient = new WebClient ();
			var url = new Uri (_url);
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
			string localFilename = "downloaded.png";
			string localPath = System.IO.Path.Combine (documentsPath, localFilename);

			//Sive the Image using writeAsync
			FileStream fs = new FileStream (localPath, FileMode.OpenOrCreate);
			await fs.WriteAsync (bytes, 0, bytes.Length);

			Console.WriteLine("localPath:"+localPath);
			fs.Close ();

			var metrics = Resources.DisplayMetrics;
			var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
			var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);

			//img.SetMaxWidth(widthInDp);
			//img.SetMaxHeight(heightInDp - 50);



			BitmapFactory.Options options = new BitmapFactory.Options ();
			options.InJustDecodeBounds = true;
			await BitmapFactory.DecodeFileAsync (localPath, options);

			options.InSampleSize = options.OutWidth > options.OutHeight ? options.OutHeight / /*img.Height*/ (heightInDp-50) : options.OutWidth /(widthInDp-50)/* img.Width*/;
				options.InJustDecodeBounds = false;

			Bitmap bitmap = await BitmapFactory.DecodeFileAsync (localPath, options);

			Console.WriteLine ("Loaded!");



			img.SetImageBitmap (bitmap);

			img.Visibility = ViewStates.Gone;
			coordinates.Visibility = ViewStates.Gone;
			address.Visibility = ViewStates.Gone;
			comment.Visibility = ViewStates.Gone;

			problemTXT.Click += (sender, e) => 
			{	
				if (img.Visibility == ViewStates.Gone)
				{
					img.Visibility = ViewStates.Visible;
					coordinates.Visibility = ViewStates.Visible;
					address.Visibility = ViewStates.Visible;
					comment.Visibility = ViewStates.Visible;
				}
				else
				{
					img.Visibility = ViewStates.Gone;
					coordinates.Visibility = ViewStates.Gone;
					address.Visibility = ViewStates.Gone;
					comment.Visibility = ViewStates.Gone;
				}
			};
			layot.AddView (problemTXT);
			layot.AddView (coordinates);
			layot.AddView (address);
			layot.AddView (comment);
			layot.AddView (img);
		}
		private int ConvertPixelsToDp(float pixelValue)
		{
			var dp = (int) ((pixelValue)/Resources.DisplayMetrics.Density);
			return dp;
		}


		LinearLayout layot;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.OnLoadedPhotosClickScreen);
			layot = FindViewById<LinearLayout> (Resource.Id.linearLayout1);
			Parse.ParseClient.Initialize("ZF2JYEfxIM7QyKVdOBn0AJEOUr1Mj5h1UMKsWqeC",
				"CEkjpD569RxuYtIYcJ9SNLMDt6FfL76fjJ48Qe3z");
			GetFiles ();
		}
	}
}




