namespace MyNewProject
{
	using System;
	using System.IO;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Collections.Generic;
	using System.Xml.Serialization;
	using System.Threading.Tasks;

	using Android.Graphics;

	using Parse;


	public class WorkingWithFiles
	{
		public async void ExportBitmapAsJPG(Bitmap bitmap, int index)
		{
			string sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			string name = "_" + index.ToString () + ".jpg";
			string filePath = System.IO.Path.Combine(sdCardPath, name);
			var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);

			await bitmap.CompressAsync (Bitmap.CompressFormat.Jpeg, 30, stream);
			stream.Close ();
		}

		public Bitmap ImportBitmapFromFile (int idx)
		{
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			var filePath = System.IO.Path.Combine(sdCardPath, "_" + idx.ToString() + ".jpg");
			var stream = new System.IO.FileStream (filePath, System.IO.FileMode.Open);
			Bitmap bitmap = BitmapFactory.DecodeStream(stream);
			stream.Close ();
			return bitmap;
		}

		public void ExportTypeInFile (string str, int index)
		{
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			if (!Directory.Exists(sdCardPath +"/Application")) Directory.CreateDirectory (sdCardPath +"/Application");
			if (!Directory.Exists(sdCardPath +"/Application/Root Studio")) Directory.CreateDirectory (sdCardPath +"/Application/Root Studio");
			var filePath = System.IO.Path.Combine(sdCardPath+"/Application/Root Studio", "_" + index.ToString() + "T.xml");
			FileStream fStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			var myBinaryFormatter = new BinaryFormatter();
			myBinaryFormatter.Serialize (fStream, str); 
			fStream.Close();
		}

		public string ImportTypeFromFile(int idx)
		{
			//string s = str + ".xml";
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			var filePath = System.IO.Path.Combine(sdCardPath+"/Application/Root Studio", "_" + idx.ToString() + "T.xml");
			FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			var myBinaryFormatter = new BinaryFormatter();
			var mc = (string) myBinaryFormatter.Deserialize(fStream);
			fStream.Close();
			return mc;
		}

		public void ExportCoordinatesInFile (double num, string name)
		{
			var dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),name);
			//запись в файл
			BinaryWriter bw = new BinaryWriter(new FileStream(dbPath, FileMode.OpenOrCreate));

			bw.Write (Convert.ToDouble (num));
			bw.Close();
		}
			
		public double ImportCoordinatesFromFile (string name)
		{
			double num = 0;
			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal), name);
			BinaryReader br;
			if ((File.Exists (dbPath))) {
				br = new BinaryReader (new FileStream (dbPath, FileMode.Open));
				for (;;) {
					try {
						num = br.ReadDouble ();
					} catch (EndOfStreamException) {
						br.Close ();
						break;
					}
				}
			}
			return num;
		}

		public void ExportNumberInFile (string name, int num)
		{
			var dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),name);
			//запись в файл
			BinaryWriter bw = new BinaryWriter(new FileStream(dbPath, FileMode.OpenOrCreate));

			bw.Write (Convert.ToInt32 (num));
			bw.Close();
		}

		public int ImportNumberFromFile (string name)
		{
			int num = -1;
			string dbPath = System.IO.Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal), name);
			BinaryReader br;
			if ((File.Exists (dbPath))) {
				br = new BinaryReader (new FileStream (dbPath, FileMode.Open));
				for (;;) {
					try {
						num = br.ReadInt32 ();
					} catch (EndOfStreamException) {
						br.Close ();
						break;
					}
				}
			}
			return num;
		}
	}

	public class SavingToServer
	{
		public async void ExportFilesOnServer (string type,  double latitude, double longitude, int index)
		{
			string sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			string name = "_" + index.ToString () + ".jpg";
			string filePath = System.IO.Path.Combine(sdCardPath, name);
			byte[] data = System.IO.File.ReadAllBytes (filePath);
			ParseGeoPoint location = new ParseGeoPoint (latitude, longitude);
			ParseFile file = new ParseFile (name, data);

			var problemObject = new ParseObject ("problem");
			problemObject ["description"] = type;
			problemObject ["coordinates"] = location;
			problemObject ["image"] = file;

			await problemObject.SaveAsync ();
		}
	}	
}