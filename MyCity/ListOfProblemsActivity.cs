using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Locations;
using Android.Util;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Provider;
using Java.IO;
using Enviroment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



namespace MyNewProject
{
	public class HomeScreenAdapter : BaseAdapter<string> {
		string[] items;
		Activity context;
		public HomeScreenAdapter(Activity context, string[] items) : base() {
			this.context = context;
			this.items = items;
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override string this[int position] {  
			get { return items[position]; }
		}
		public override int Count {
			get { return items.Length; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
//			view.SetBackgroundColor(Color.White);
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
			TextView txtView = view.FindViewById<TextView> (Android.Resource.Id.Text1);
			txtView.Text = items[position];
			txtView.SetTextColor (Color.Black);
			txtView.SetBackgroundColor (Color.White);
			view.SetBackgroundColor (Color.White);
			return view;
		}
	}

	[Activity (Label = "Список проблем")]			
	public class ListOfProblemsActivity : ListActivity
	{
		string [] items;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			items = new string[]{"Яма на дорозі", "Відкритий люк", "Непрацюючий світлофор", "Аварійний будинок", "Занедбаний майданчик" };
//			ListAdapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1, items);
			//ListView list = FindViewById<ListView>(Resource.Id.listView);
			//ListAdapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1, items);
			ListAdapter = new HomeScreenAdapter(this, items);
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id)
		{
			var t = items [position];
			Android.Widget.Toast.MakeText(this, t, Android.Widget.ToastLength.Short).Show();
//			MainActivity.problems.AddProblems (t);
//			MainActivity.listProblems.AddItem(t);

       //     ++GetGps.index;
		//	WorkingWithFiles work = new WorkingWithFiles ();

        //    work.ExportNumberInFile("num002.dat", GetGps.index);


            GetGps.type = t;
			//int idx = MainActivity.index;
     //       work.ExportTypeInFile(t, GetGps.index);

			//SavingToServer.ExportFilesOnServer (MainActivity.type, Ma);

			Intent intent = new Intent (this, typeof(AddComment));
			StartActivity (intent);
		}

		public override void OnBackPressed()
		{
	//		Intent intent = new Intent (this, typeof(MainActivity));
	//		StartActivity (intent);
		}
	}
}

