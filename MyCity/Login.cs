
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



using Android.Content.PM;
using Java.Security;


using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;

namespace MyNewProject
{
	[Activity (Label = "Login"/*, MainLauncher= true*/)]			
	public class Login : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.LoginWithFacebook);
			FacebookSdk.SdkInitialize (this.ApplicationContext);

			// Create your application here
		}
	}
}

