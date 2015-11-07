using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;	
using Android.Util;



namespace MyNewProject
{
	[Activity (Label = "UserLocation")]			
	public class UserLocation : Activity, ILocationListener 
	{
		Location _currentLocation;
		LocationManager _locationManager;
//		TextView _locationText;
//		TextView _addressText;
		string strCoordinates;
		string strStreet;

		String _locationProvider;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			//InitializeLocationManager();
			RequestCurrentLocation();
			MainActivity.problems[MainActivity.index].ProblemsItems = new ProblemsItem();

			AddressButton_OnClick();	

			Intent intent = new Intent (this, typeof(LoadPhotos));
			StartActivity (intent);
			 
			// Create your application here
		}



		async void AddressButton_OnClick()//(object sender, EventArgs eventArgs)
		{



			if (_currentLocation == null)
			{
				//_addressText.Text = "Can't determine the current address.";

				strCoordinates = "Can't determine the current location.";
				strStreet = "Can't determine the current address.";

				MainActivity.problems [MainActivity.index].ProblemsItems.SetCoordinates (strCoordinates);
				MainActivity.problems [MainActivity.index].ProblemsItems.SetStreet (strStreet);

				MainActivity.problems [MainActivity.index].ProblemsItems.WriteCoordinates (strCoordinates);
				MainActivity.problems [MainActivity.index].ProblemsItems.WriteStreet (strStreet);


				return;
			}

			Geocoder geocoder = new Geocoder(this);
			IList<Address> addressList = await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

			Address address = addressList.FirstOrDefault();
			if (address != null)
			{
				StringBuilder deviceAddress = new StringBuilder();
				for (int i = 0; i < address.MaxAddressLineIndex; i++)
				{
					deviceAddress.Append(address.GetAddressLine(i))
						.AppendLine(",");
				}
				//MainActivity.list.AddStreet (deviceAddress.ToString ());

				//MainActivity.listStreet.AddItem(deviceAddress.ToString ());
				strStreet = deviceAddress.ToString ();

			}
			else
			{
				//MainActivity.list.AddStreet ("Unable to determine the address.");

			//	MainActivity.listStreet.AddItem ("Unable to determine the address.");
			
				strStreet = "Unable to determine the address.";
			}

			MainActivity.problems [MainActivity.index].ProblemsItems.SetStreet (strStreet);
			MainActivity.problems [MainActivity.index].ProblemsItems.WriteStreet (strStreet);
		}


		private void RequestCurrentLocation()
		{
			Criteria locationCriteria = new Criteria () { Accuracy = Accuracy.NoRequirement, PowerRequirement = Power.NoRequirement };

			this._locationManager = GetSystemService (Context.LocationService) as LocationManager;
			_locationProvider = this._locationManager.GetBestProvider (locationCriteria, true);
			this._locationManager.RequestLocationUpdates (_locationProvider, 0, 0, this);
		}


/*		void InitializeLocationManager()
		{
			_locationManager = (LocationManager)GetSystemService(LocationService);
			Criteria criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.Fine
			};
			IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

			if (acceptableLocationProviders.Any())
			{
				_locationProvider = acceptableLocationProviders.First();
			}
			else
			{
				_locationProvider = String.Empty;
			}
		}
*/
		public void OnProviderDisabled(string provider) {}

		public void OnProviderEnabled(string provider) {}

		public void OnStatusChanged(string provider, Availability status, Bundle extras) {}

		protected override void OnResume()
		{
			base.OnResume();
			_locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
		}

		protected override void OnPause()
		{
			base.OnPause();
			_locationManager.RemoveUpdates(this);
		}

		public void OnLocationChanged(Location location)
		{
			_currentLocation = location;
			if (_currentLocation == null)
			{
				//MainActivity.list.AddCoordinates("Unable to determine your location.");

		//		MainActivity.listCoordinates.AddItem("Unable to determine your location.");
				strCoordinates = "Unable to determine your location.";
			}
			else
			{
				//MainActivity.list.AddCoordinates (String.Format ("{0},{1}", _currentLocation.Latitude, _currentLocation.Longitude));
			
				//MainActivity.listCoordinates.AddItem (String.Format ("{0},{1}", _currentLocation.Latitude, _currentLocation.Longitude));
			
				strCoordinates = String.Format ("{0},{1}", _currentLocation.Latitude, _currentLocation.Longitude);
			}
			MainActivity.problems [MainActivity.index].ProblemsItems.SetCoordinates (strCoordinates);
			MainActivity.problems [MainActivity.index].ProblemsItems.WriteCoordinates (strCoordinates);

		}
	}
}

