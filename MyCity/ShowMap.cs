
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
using Android.Gms.Maps;
using Android.Gms.Maps.Model;


using Java.Lang;

using Parse;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Android.Graphics;

using Android.Support.V4;


namespace MyNewProject
{
	[Activity (Label = "Відмітки на карті")]			
	public class ShowMap : Activity, IOnMapReadyCallback//, Android.Gms.Maps.GoogleMap.IInfoWindowAdapter, Android.Gms.Maps.GoogleMap.IOnInfoWindowClickListener
	{
		ProgressDialog progress;

		List<string> descriptionList = new List<string> ();
		List<ParseGeoPoint> locationList = new List<ParseGeoPoint> ();
		List<ParseFile> parseFiles = new List<ParseFile> ();
		List<string> addressList = new List<string> ();
		List<string> commentList = new List<string> ();
		WebClient webClient;


		public async void GetFiles ()
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery ("problem");
			IEnumerable<ParseObject> relatedObjects = await query.FindAsync ();

			int index = 0;
			foreach (ParseObject problem in relatedObjects) 
			{
				descriptionList.Add (problem.Get<string> ("description"));
				locationList.Add (problem.Get<ParseGeoPoint> ("coordinates"));
				parseFiles.Add (problem.Get<ParseFile> ("image"));
				addressList.Add (problem.Get<string> ("address"));
				commentList.Add (problem.Get<string> ("comment"));

	/*			MarkerOptions markerOptions = new MarkerOptions ()
					.SetPosition (new Android.Gms.Maps.Model.LatLng 
						(locationList [index].Latitude, locationList [index].Longitude))
					.SetTitle (descriptionList [index])
					.SetSnippet ("kek");


				map.AddMarker (markerOptions);
*/
				++index;
			}


	//		SetUpMap ();

		}



		private GoogleMap map;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			Parse.ParseClient.Initialize ("ZF2JYEfxIM7QyKVdOBn0AJEOUr1Mj5h1UMKsWqeC",
				"CEkjpD569RxuYtIYcJ9SNLMDt6FfL76fjJ48Qe3z");
			SetContentView (Resource.Layout.ShowMap);




		/*	MapFragment mapFrag = (MapFragment) FragmentManager.FindFragmentById(Resource.Id.map);
		/*	GoogleMap map = mapFrag.Map;

			if (map != null) {
				map.UiSettings.ZoomControlsEnabled = true;
				map.UiSettings.CompassEnabled = true;
			
				Marker marker = map.AddMarker (new MarkerOptions ()
					.SetPosition (new Android.Gms.Maps.Model.LatLng (50.379444, 2.773611))
					.SetTitle ("MyTitle")
					.SetSnippet ("MySnippet")
					.InvokeIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.Icon))
	
				);
*/

				/*MarkerOptions markerOpt1 = new MarkerOptions();
				markerOpt1.SetPosition(new Android.Gms.Maps.Model.LatLng(50.379444, 2.773611));
				markerOpt1.SetTitle("Vimy Ridge");
				map.AddMarker (markerOpt1);*/


			/*	CircleOptions circleOptions = new CircleOptions ();
				circleOptions.InvokeCenter (new Android.Gms.Maps.Model.LatLng(37.4, -122.1));
				circleOptions.InvokeRadius (10000000);
				map.AddCircle (circleOptions);*/
			SetUpMap ();
			//GetFiles();
		}





		public void SetUpMap ()
		{
			if (map == null) {
				FragmentManager.FindFragmentById<MapFragment> (Resource.Id.map).GetMapAsync (this);
			}
		}


		public void OnMapReady (GoogleMap googleMap)
		{
			Android.Gms.Maps.Model.LatLng latLng = new Android.Gms.Maps.Model.LatLng (50.618, 26.257);

			map = googleMap;
			CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom (latLng, 10); 
			map.MoveCamera (camera);


			GetFiles ();


	//		MarkerOptions options = new MarkerOptions ()

	//			.SetPosition (latLng)
		//		.SetTitle ("My First Title")
		//		.SetSnippet ("My First Snippet")
	//			.Draggable (true);

	//		map.AddMarker (options);


			//map.MarkerClick += Map_MarkerClick;

	//		map.MarkerDragEnd += Map_MarkerDragEnd;

	//		map.SetInfoWindowAdapter (this);

	//		map.SetOnInfoWindowClickListener (this);


		}

/*		void Map_MarkerDragEnd (object sender, GoogleMap.MarkerDragEndEventArgs e)
		{
			Android.Gms.Maps.Model.LatLng pos = e.Marker.Position;

		}

	/*	void Map_MarkerClick (object sender, GoogleMap.MarkerClickEventArgs e)
		{
			Android.Gms.Maps.Model.LatLng pos = e.Marker.Position;
			map.AnimateCamera (CameraUpdateFactory.NewLatLngZoom (pos, 10));
		}*/

/*		public View GetInfoContents (Marker marker)
		{
			return null;
		}

		public View GetInfoWindow (Marker marker)
		{
			GC.Collect ();
			View view = LayoutInflater.Inflate(Resource.Layout.info_window, null, false);
			int index = Convert.ToInt32((marker.Id.Remove(0,1)));
			Bitmap bitmap = null;
			bitmap = DownloadImage (index);

			view.FindViewById<TextView> (Resource.Id.description).Text = descriptionList[index];
			view.FindViewById<TextView> (Resource.Id.coordinates).Text = "Широта: " + 
				locationList[index].Latitude + "Довгота: " + locationList[index].Longitude;
			
			view.FindViewById<TextView> (Resource.Id.address).Text = addressList[index];
			view.FindViewById<TextView> (Resource.Id.comment).Text = commentList [index];
			view.FindViewById<ImageView>(Resource.Id.image).SetImageBitmap(bitmap);

			return view;
		}




		public void OnInfoWindowClick (Marker marker)
		{

		}
		*/



		public Bitmap DownloadImage (int index)
		{
			webClient = new WebClient ();
			var url = new Uri (parseFiles[index].Url.ToString());
			byte[] bytes = null;

			try{
				//bytes = await webClient.DownloadDataTaskAsync(url);
				bytes = webClient.DownloadData(url);
			}
			catch(TaskCanceledException){
				Console.WriteLine ("Task Canceled!");
//				return;
			}
			catch(System.Exception e){
				Console.WriteLine (e.ToString());
//				return;
			}
			string documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);	
			string localFilename = "downloaded" + index.ToString () + ".png";
			string localPath = System.IO.Path.Combine (documentsPath, localFilename);

			//Sive the Image using writeAsync
			FileStream fs = new FileStream (localPath, FileMode.OpenOrCreate);
			//await fs.WriteAsync (bytes, 0, bytes.Length);
			fs.Write(bytes, 0, bytes.Length);
			Console.WriteLine("localPath:"+localPath);
			fs.Close ();

			var metrics = Resources.DisplayMetrics;
	//		var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
	//		var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);


			BitmapFactory.Options options = new BitmapFactory.Options ();
			options.InJustDecodeBounds = true;
			//await BitmapFactory.DecodeFileAsync (localPath, options);

			BitmapFactory.DecodeFile (localPath, options);

			options.InSampleSize = CalculateInSampleSize (options, 100, 100);//options.OutWidth > options.OutHeight ? options.OutHeight /  (heightInDp-50) : options.OutWidth /(widthInDp-50);
			options.InJustDecodeBounds = false;

			//Bitmap bitmap = await BitmapFactory.DecodeFileAsync (localPath, options);

			Bitmap bitmap = BitmapFactory.DecodeFile (localPath, options);


			//image.SetImageBitmap (bitmap);
			//image = bitmap;
	//		CreateInfo (bitmap, index);

			Console.WriteLine ("Loaded!");
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



/*		private int ConvertPixelsToDp(float pixelValue)
		{
			var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
			return dp;
		}*/
	}
}


