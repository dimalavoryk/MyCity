using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Java.Security;

using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyNewProject
{
	[Activity (Label = "My City", Icon = "@drawable/icon", ScreenOrientation =  Android.Content.PM.ScreenOrientation.Portrait, MainLauncher = true)]			
	public class FaceBookLogin : Activity, IFacebookCallback
	{
		private ICallbackManager mCallBackManager;
		private MyProfileTracker mProfileTracker;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);



			// Set our view from the "main" layout resource
			FacebookSdk.SdkInitialize (this.ApplicationContext);


			SetContentView (Resource.Layout.LoginWithFacebook);

			mProfileTracker = new MyProfileTracker ();
			mProfileTracker.mOnProfileChanged +=  MProfileTracker_mOnProfileChanged;
			mProfileTracker.StartTracking ();

			Button facebookButton = FindViewById<Button> (Resource.Id.button);

			if (AccessToken.CurrentAccessToken != null && Profile.CurrentProfile != null) 
			{
				//	Console.WriteLine ("Logged in");
				facebookButton.Text = "Logged out";
			}
			mCallBackManager = CallbackManagerFactory.Create ();
		}

		void MProfileTracker_mOnProfileChanged (object sender, OnProfileChangedEventsArgs e)
		{
		}

		public void OnCancel ()
		{
			//throw new NotImplementedException ();
		}

		public void OnError (FacebookException p0)
		{
			//	throw new NotImplementedException ();
			Android.Widget.Toast.MakeText(this, p0.ToString(), Android.Widget.ToastLength.Long).Show();
		}

		public void OnSuccess (Java.Lang.Object result)
		{
			LoginResult loginResult = result as LoginResult;
			Console.WriteLine( loginResult.AccessToken.UserId);

	/*		var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			if (!Directory.Exists(sdCardPath +"/Application")) Directory.CreateDirectory (sdCardPath +"/Application");
			if (!Directory.Exists(sdCardPath +"/Application/Root Studio")) Directory.CreateDirectory (sdCardPath +"/Application/Root Studio");
			var filePath = System.IO.Path.Combine(sdCardPath+"/Application/Root Studio", "UserId.xml");
			FileStream fStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			var myBinaryFormatter = new BinaryFormatter();
			myBinaryFormatter.Serialize (fStream, loginResult.AccessToken.UserId); 
			fStream.Close();
*/
			Intent intent = new Intent (this, typeof(MainActivity));
			Finish ();
			StartActivity (intent);

		}


		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			mCallBackManager.OnActivityResult(requestCode, (int)resultCode, data);
		}

		protected override void OnDestroy ()
		{
			mProfileTracker.StopTracking ();
			base.OnDestroy ();
		}

	}

	public class MyProfileTracker : ProfileTracker
	{
		public event EventHandler<OnProfileChangedEventsArgs> mOnProfileChanged;

		protected override void OnCurrentProfileChanged(Profile oldProfile, Profile newProfile)
		{
			if (newProfile != null) 
			{
				mOnProfileChanged.Invoke(this, new OnProfileChangedEventsArgs(newProfile));
			}
		}
	}

	public class OnProfileChangedEventsArgs : EventArgs
	{
		public Profile mProfile;

		public OnProfileChangedEventsArgs(Profile profile)
		{
			mProfile = profile;
		}
	}

}
