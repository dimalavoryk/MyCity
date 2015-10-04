using System;
using Android.Runtime;
using Android.Content;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Views;
using Android.OS;
using Android.Widget;
using Android.App;


namespace MyNewProject
{
	public class AddPhotos: DialogFragment
	{
	//akeAPhoto xT;
/*		private Button takeAPhoto;
		private Button uploadFromGallery;
*/		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);

			var view = inflater.Inflate(Resource.Layout.AddPhotos, container, false);
//			takeAPhoto = view.FindViewById<Button> (Resource.Id.TakeAPhoto);
//			takeAPhoto.Click += TakeAPhoto_Click;
//			uploadFromGallery = view.FindViewById<Button> (Resource.Id.uploadFromgGallery);
//			uploadFromGallery.Click += UploadFromGallery_Click;
			//xT = new TakeAPhoto ();
//			takeAPhoto.Click += TakeAPhoto_Click;
	//		TakeAPhoto x = new TakeAPhoto ();
	//		takeAPhoto.Click += ;
			return view;
		}

		/*		void UploadFromGallery_Click (object sender, EventArgs e)
		{
			Intent intent1 = new Intent (this, typeof(UploadFromGallery));
			StartActivity (intent1);
		}

		void TakeAPhoto_Click (object sender, EventArgs e)
		{
			Intent intent = new Intent (this, typeof(TakeAPhoto));
			StartActivity (intent);
			//			
		}
*/


	}
}

