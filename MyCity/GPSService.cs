using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Text;  

using Android.App;  
using Android.Content;  
using Android.OS;  
using Android.Locations;  


namespace MyNewProject
{
	[Service]  
	public class GPSService : Service, ILocationListener  
	{  
		private string _location = string.Empty;  
	//	private string _address = string.Empty;  
	//	private string _remarks = string.Empty;  

		public const string LOCATION_UPDATE_ACTION = "LOCATION_UPDATED";  
		private Location _currentLocation;  
		IBinder _binder;  

		protected LocationManager _locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(LocationService);  
		public override IBinder OnBind(Intent intent)  
		{  
			_binder = new GPSServiceBinder(this);  
			return _binder;  
		}  

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)  
		{  
			return StartCommandResult.Sticky;  
		}  

		public void StartLocationUpdates()  
		{  
			Criteria criteriaForGPSService = new Criteria  
			{  
				//A constant indicating an approximate accuracy  
				Accuracy = Accuracy.Coarse,  
				PowerRequirement = Power.Medium  
			};  

			var locationProvider = _locationManager.GetBestProvider(criteriaForGPSService, true);  
			_locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);  

		}  





		public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };  
		public void OnLocationChanged(Location location)  
		{  
			try  
			{  
				_currentLocation = location;  

				if (_currentLocation == null)  
					_location = "Unable to determine your location.";  
				else  
				{  
					_location = String.Format("{0},{1}", _currentLocation.Latitude, _currentLocation.Longitude);  
				//	MainActivity.latitude = _currentLocation.Latitude;
				//	MainActivity.longitude = _currentLocation.Longitude;
				
					MainActivity.latitude = _currentLocation.Latitude;
					MainActivity.longitude = _currentLocation.Longitude;

					WorkingWithFiles work = new WorkingWithFiles();
					work.ExportCoordinatesInFile(MainActivity.latitude, "latitude" + MainActivity.index.ToString() + ".dat");
					work.ExportCoordinatesInFile(MainActivity.longitude, "longitude" + MainActivity.index.ToString() + ".dat");
					/*	Geocoder geocoder = new Geocoder(this);  

					//The Geocoder class retrieves a list of address from Google over the internet  
					IList<Address> addressList = geocoder.GetFromLocation(_currentLocation.Latitude, _currentLocation.Longitude, 10);  

					Address addressCurrent = addressList.FirstOrDefault();  

					if (addressCurrent != null)  
					{  
						StringBuilder deviceAddress = new StringBuilder();  

						for (int i = 0; i < addressCurrent.MaxAddressLineIndex; i++)  
							deviceAddress.Append(addressCurrent.GetAddressLine(i))  
								.AppendLine(",");  

						_address = deviceAddress.ToString();  
					}  
					else  
						_address = "Unable to determine the address.";  
*/
				//	IList<Address> source = geocoder.GetFromLocationName(_sourceAddress, 1);  
				//	Address addressOrigin = source.FirstOrDefault();  

			//		var coord1 = new LatLng(addressOrigin.Latitude, addressOrigin.Longitude);  
			//		var coord2 = new LatLng(addressCurrent.Latitude, addressCurrent.Longitude);  

					//_remarks = string.Format("Your are {0} miles away from your original location.", distanceInRadius);  



				//	Intent intent = new Intent(this, typeof(MainActivity.GPSServiceReciever));  
				//	intent.SetAction(MainActivity.GPSServiceReciever.LOCATION_UPDATED);  
					Intent intent = new Intent(this, typeof(GetGps.GPSServiceReciever));
					intent.SetAction(GetGps.GPSServiceReciever.LOCATION_UPDATED); 	intent.AddCategory(Intent.CategoryDefault);  
					intent.PutExtra("Location", _location);  
				//	intent.PutExtra("Address", _address);  
				//	intent.PutExtra("Remarks", _remarks);  
					SendBroadcast(intent);  
				}  
			}  
			catch (Exception ex)  
			{  
				//_address = "Unable to determine the address.";  
			}  

		}  

		public void OnStatusChanged(string provider, Availability status, Bundle extras)  
		{  
			//TO DO:  
		}  

		public void OnProviderDisabled(string provider)  
		{  
			//TO DO:  
		}  

		public void OnProviderEnabled(string provider)  
		{  
			//TO DO:  
		}  
	}  
	public class GPSServiceBinder : Binder  
	{  
		public GPSService Service { get { return this.LocService; } }  
		protected GPSService LocService;  
		public bool IsBound { get; set; }  
		public GPSServiceBinder(GPSService service) { this.LocService = service; }  
	}  
	public class GPSServiceConnection : Java.Lang.Object, IServiceConnection  
	{  

		GPSServiceBinder _binder;  

		public event Action Connected;  
		public GPSServiceConnection(GPSServiceBinder binder)  
		{  
			if (binder != null)  
				this._binder = binder;  
		}  

		public void OnServiceConnected(ComponentName name, IBinder service)  
		{  
			GPSServiceBinder serviceBinder = (GPSServiceBinder)service;  

			if (serviceBinder != null)  
			{  
				this._binder = serviceBinder;  
				this._binder.IsBound = true;  
				serviceBinder.Service.StartLocationUpdates();  
				if (Connected != null)  
					Connected.Invoke();  
			}  
		}  
		public void OnServiceDisconnected(ComponentName name) { this._binder.IsBound = false; }  
	}  
}



/*
 * 		public static bool isGPSEnabled() 
		{

			LocationManager locationManager = (LocationManager) Android.Test.Mock.MockContext.GetSystemService(LocationService);
			if (locationManager != null) 
			{
				return locationManager.IsProviderEnabled (LocationManager.GpsProvider);
			}

			else
			{
				Intent intent = new Intent (Android.Provider.Settings.ActionLocationSourceSettings);//android.provider.Settings.ACTION_LOCATION_SOURCE_SETTINGS);
				StartActivity(intent);
				return false;
			}
		}
		*/

