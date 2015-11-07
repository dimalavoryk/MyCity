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
namespace MyNewProject
{
	[Activity (Label = "Головне Меню", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		static public List<Problems> problems;// = new List<Problems>();
		static public int index;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
		//	list = new SaveProblems ();
		//	list = list.ReadProblems ();

			index = -1;
			string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),"m_Index.dat");
			BinaryReader br;
			if ((File.Exists (dbPath))) 
			{
				br = new BinaryReader(new FileStream( dbPath, FileMode.Open));
				for(;;)
				{
					try
					{
						//adapter_name_mash.Add(br.ReadDouble());
						index = br.ReadInt32();
					}
					catch(EndOfStreamException) 
					{
						br.Close();
						break;
					}
				}
			}

			problems = new List<Problems> ();

			for (int i = 0; i <= index; ++i) 
			{
				problems.Add (new Problems ());
				problems [i].SetType (problems[i].ReadType(i));
				problems [i].ProblemsItems = new ProblemsItem ();
				problems [i].ProblemsItems.SetStreet (problems [i].ProblemsItems.ReadStreet (i));
				problems [i].ProblemsItems.SetCoordinates (problems [i].ProblemsItems.ReadCoordinates (i));
				problems [i].ProblemsItems.SetImgBitmap (problems [i].ProblemsItems.ImportBitmapFromFile (i)); 
			}


			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			Button reportProblem = FindViewById<Button> (Resource.Id.reportProblem);
			reportProblem.Click += ReportProblemClick;
			Button loadedPhotos = FindViewById<Button> (Resource.Id.loadedPhotos);
			loadedPhotos.Click += LoadedPhotos_Click;

		}

		private void LoadedPhotos_Click (object sender, EventArgs e)
		{
			//Intent intent = new Intent (this, typeof(LoadedPhotos));
			Intent intent = new Intent (this, typeof(OnLoadedPhotosClick));
			StartActivity (intent);
		}


		private void ReportProblemClick (object sender, EventArgs e)
		{
			Intent intent = new Intent (this, typeof(ListOfProblemsActivity));
			StartActivity (intent);
		}

	}
}


