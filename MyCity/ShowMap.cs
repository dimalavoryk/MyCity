
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

using Java.Lang;
using Android.Gms.Maps.Model;

namespace MyNewProject
{
	[Activity (Label = "ShowMap")]			
	public class ShowMap : Activity, IOnMapReadyCallback, Android.Gms.Maps.GoogleMap.IInfoWindowAdapter, Android.Gms.Maps.GoogleMap.IOnInfoWindowClickListener
	{


		

		private GoogleMap map;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
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

		}

		public void SetUpMap ()
		{
			if (map == null) {
				FragmentManager.FindFragmentById<MapFragment> (Resource.Id.map).GetMapAsync (this);
			}
		}


		public void OnMapReady (GoogleMap googleMap)
		{
			Android.Gms.Maps.Model.LatLng latLng = new Android.Gms.Maps.Model.LatLng (40.776408, -73.970755);

			map = googleMap;
			CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom (latLng, 10); 
			map.MoveCamera (camera);

			MarkerOptions options = new MarkerOptions ()

				.SetPosition (latLng)
		//		.SetTitle ("My First Title")
		//		.SetSnippet ("My First Snippet")
				.Draggable (true);

			map.AddMarker (options);

			//map.MarkerClick += Map_MarkerClick;

			map.MarkerDragEnd += Map_MarkerDragEnd;

			map.SetInfoWindowAdapter (this);

			map.SetOnInfoWindowClickListener (this);

		}

		void Map_MarkerDragEnd (object sender, GoogleMap.MarkerDragEndEventArgs e)
		{
			Android.Gms.Maps.Model.LatLng pos = e.Marker.Position;

		}

	/*	void Map_MarkerClick (object sender, GoogleMap.MarkerClickEventArgs e)
		{
			Android.Gms.Maps.Model.LatLng pos = e.Marker.Position;
			map.AnimateCamera (CameraUpdateFactory.NewLatLngZoom (pos, 10));
		}*/

		public View GetInfoContents (Marker marker)
		{
			return null;
		}

		public View GetInfoWindow (Marker marker)
		{
			View view = LayoutInflater.Inflate(Resource.Layout.info_window, null, false);

			return view;
		}

		public void OnInfoWindowClick (Marker marker)
		{

		}
	}
}

