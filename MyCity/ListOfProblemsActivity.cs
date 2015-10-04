﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
			view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position];
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
			Intent intent = new Intent (this, typeof(LoadPhotos));
			StartActivity (intent);
//			Intent intent1 = new Intent (this, typeof(UploadFromGallery));
//			StartActivity (intent1);
		}
	}
}
