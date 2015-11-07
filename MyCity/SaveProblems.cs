using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Xml.Serialization;

using Android.Graphics;

using Android.Views;
using Android.Widget;

namespace MyNewProject
{
	public class Problems 
	{
		private string Type;// { get; set; }
	//	public ProblemsItem[] ProblemsItems { get; set; }
		public ProblemsItem ProblemsItems ;//{ get; set; }
	
		public void SetType (string type)
		{
			Type = type;
		}

		public string GetStringType ()
		{
			return Type;
		}

		public void WriteType (string str)
		{
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			if (!Directory.Exists(sdCardPath +"/Application")) Directory.CreateDirectory (sdCardPath +"/Application");
			if (!Directory.Exists(sdCardPath +"/Application/Root Studio")) Directory.CreateDirectory (sdCardPath +"/Application/Root Studio");
			var filePath = System.IO.Path.Combine(sdCardPath+"/Application/Root Studio", "_" + (MainActivity.index).ToString() + "T.xml");
			FileStream fStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			var myBinaryFormatter = new BinaryFormatter();
			myBinaryFormatter.Serialize(fStream, str); 
			fStream.Close();
		}

		public string ReadType(int idx)
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
	}


	public class ProblemsItem
	{
	//	public string coordinates { get; set; }
	//	public string street { get; set; }
	//	public Bitmap photo { get; set; }
		private string coordinates;// { get; set; }
		private string street;// { get; set; }
		private Bitmap imgBitmap;

		public void SetCoordinates (string sCoordinates)
		{
			coordinates = sCoordinates;
		}

		public string GetCoordinates ()
		{
			return coordinates;
		}

		public void SetStreet (string sStreet)
		{
			street = sStreet;
		}

		public string GetStreet ()
		{
			return street;
		}	

		public void SetImgBitmap (Bitmap bitmap)
		{
			imgBitmap = bitmap;
		}

		public Bitmap GetImgBitmap ()
		{
			return imgBitmap;
		}



		public void WriteCoordinates (string str)
		{
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			if (!Directory.Exists(sdCardPath +"/Application")) Directory.CreateDirectory (sdCardPath +"/Application");
			if (!Directory.Exists(sdCardPath +"/Application/Root Studio")) Directory.CreateDirectory (sdCardPath +"/Application/Root Studio");
			var filePath = System.IO.Path.Combine(sdCardPath+"/Application/Root Studio", "_" + (MainActivity.index).ToString() + "crdnts.xml");
			FileStream fStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			var myBinaryFormatter = new BinaryFormatter();
			myBinaryFormatter.Serialize(fStream, str); 
			fStream.Close();
		}


		public string ReadCoordinates(int idx)
		{
			//string s = str + ".xml";
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			var filePath = System.IO.Path.Combine(sdCardPath+"/Application/Root Studio", "_" + idx.ToString() + "crdnts.xml");
			FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			var myBinaryFormatter = new BinaryFormatter();
			var mc = (string) myBinaryFormatter.Deserialize(fStream);
			fStream.Close();
			return mc;
		}


		public void WriteStreet (string str)
		{
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			if (!Directory.Exists(sdCardPath +"/Application")) Directory.CreateDirectory (sdCardPath +"/Application");
			if (!Directory.Exists(sdCardPath +"/Application/Root Studio")) Directory.CreateDirectory (sdCardPath +"/Application/Root Studio");
			var filePath = System.IO.Path.Combine(sdCardPath+"/Application/Root Studio", "_" + (MainActivity.index).ToString() + "str.xml");
			FileStream fStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			var myBinaryFormatter = new BinaryFormatter();
			myBinaryFormatter.Serialize(fStream, str); 
			fStream.Close();
		}

		public string ReadStreet (int idx)
		{
			//string s = str + ".xml";
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			var filePath = System.IO.Path.Combine(sdCardPath+"/Application/Root Studio", "_" + idx.ToString() + "str.xml");
			FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			var myBinaryFormatter = new BinaryFormatter();
			var mc = (string) myBinaryFormatter.Deserialize(fStream);
			fStream.Close();
			return mc;
		}


		public void ExportBitmapAsPNG()
		{
			Bitmap bitmap = imgBitmap;
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			var filePath = System.IO.Path.Combine(sdCardPath, "_" + MainActivity.index.ToString() + ".png");
			var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
			bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
			stream.Close();
		}

		public Bitmap ImportBitmapFromFile (int idx)
		{
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			var filePath = System.IO.Path.Combine(sdCardPath, "_" + idx.ToString() + ".png");
			var stream = new System.IO.FileStream (filePath, System.IO.FileMode.Open);
			Bitmap bitmap = BitmapFactory.DecodeStream (stream);
			stream.Close ();
			return bitmap;
		}
	}
}

