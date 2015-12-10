using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

using Parse;

namespace MyNewProject
{
	[Activity (Label = "GetGps")]			
	public class GetGps : Activity
	{
		GPSServiceBinder _binder;  
		GPSServiceConnection _gpsServiceConnection;  
		Intent _gpsServiceIntent;  
		private GPSServiceReciever _receiver;  
		public static GetGps Instanse;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			Instanse = this;
			RegisterService ();

			// Create your application here
		}

		private void RegisterService()  
		{  
			_gpsServiceConnection = new GPSServiceConnection(_binder);  
			_gpsServiceIntent = new Intent(Android.App.Application.Context, typeof(GPSService));  
			BindService(_gpsServiceIntent, _gpsServiceConnection, Bind.AutoCreate);  
		}  
		private void RegisterBroadcastReceiver()  
		{  
			IntentFilter filter = new IntentFilter(GPSServiceReciever.LOCATION_UPDATED);  
			filter.AddCategory(Intent.CategoryDefault);  
			_receiver = new GPSServiceReciever();  
			RegisterReceiver(_receiver, filter);  
		}  

		private void UnRegisterBroadcastReceiver()  
		{  
			UnregisterReceiver(_receiver);  
		}   

		protected override void OnResume()  
		{  
			base.OnResume();  
			RegisterBroadcastReceiver();  
		}  

		protected override void OnPause()  
		{  
			base.OnPause();  
			UnRegisterBroadcastReceiver();  
		}  


	/*	private void ExportCoordinatesInFile (Intent intent)
		{
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			if (!Directory.Exists(sdCardPath +"/Application")) Directory.CreateDirectory (sdCardPath +"/Application");
			if (!Directory.Exists(sdCardPath +"/Application/Root Studio")) Directory.CreateDirectory (sdCardPath +"/Application/Root Studio");
			var filePath = System.IO.Path.Combine(sdCardPath+"/Application/Root Studio", "_" + (MainActivity.index + 1).ToString() + "crdnts.xml");
			FileStream fStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			var myBinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			myBinaryFormatter.Serialize(fStream, intent.GetStringExtra("Location")); 
			fStream.Close();
		}
*/

		[BroadcastReceiver]  
		internal class GPSServiceReciever : BroadcastReceiver  
		{  
			public static readonly string LOCATION_UPDATED = "LOCATION_UPDATED";  
			public override void OnReceive(Context context, Intent intent)  
			{  
				if (intent.Action.Equals(LOCATION_UPDATED))  
				{  
					MainActivity.flag = true;
					//GetGps.Instanse.ExportCoordinatesInFile (intent);
					//MainActivity.Instance.UpdateUI(intent);  
				}  

			}  
		} 




	}
}

