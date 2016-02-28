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

namespace MyNewProject
{
	[Activity (Label = "Короткий опис", ScreenOrientation =  Android.Content.PM.ScreenOrientation.Portrait)]			
	public class AddComment : Activity
	{
		TextView textView;
		Button nextButton;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.AddComment);

			EditText editText = FindViewById<EditText> (Resource.Id.editText);
			textView = FindViewById<TextView> (Resource.Id.comment);

			nextButton = FindViewById<Button> (Resource.Id.NextButtonS);

			nextButton.Click += NextButton_Click;

			editText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {      
				textView.Text = e.Text.ToString ();
			};
		}

		void NextButton_Click (object sender, EventArgs e)
		{
			if (textView.Text == null) {
				textView.Text = "Опис відсутній";
			}
			MainActivity.comment = textView.Text;
			Intent intent = new Intent (this, typeof(LoadPhotos));
			Finish ();
			StartActivity (intent);
		}
		public override void OnBackPressed()
		{
		}
	}
}

