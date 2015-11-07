using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Xml.Serialization;

using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace MyNewProject
{	
	[Serializable]
	public class SaveBitmap
	{
		private List<Bitmap> m_list;
		public SaveBitmap ()
		{
			m_list = new List<Bitmap> ();
		}

		public void AddItem (Bitmap item)
		{
			m_list.Add (item);	
		}

		public Bitmap ReadList(int idx)
		{
			//string s = str + ".xml";
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			var filePath = System.IO.Path.Combine(sdCardPath+"/Application/Root Studio", idx.ToString() + ".jpg");
		/*	FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			var myBinaryFormatter = new BinaryFormatter();
			byte mc = (byte) myBinaryFormatter.Deserialize(fStream);
			fStream.Close();
		*/	Bitmap bitmap;
			bitmap = BitmapFactory.DecodeFile (filePath);
			return bitmap;
		//	return mc;
		}

		public void WriteList (byte [] picData)
		{
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			if (!Directory.Exists(sdCardPath +"/Application")) Directory.CreateDirectory (sdCardPath +"/Application");
			if (!Directory.Exists(sdCardPath +"/Application/Root Studio")) Directory.CreateDirectory (sdCardPath +"/Application/Root Studio");
			var filePath = System.IO.Path.Combine(sdCardPath+"/Application/Root Studio", (MainActivity.index - 1).ToString() + ".png");
			FileStream fStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			var myBinaryFormatter = new BinaryFormatter();
			myBinaryFormatter.Serialize(fStream, picData); //this замість picdata
			fStream.Close();
		}

		public Bitmap GetItem (int idx)
		{
			return m_list [idx];
		}


	}
}
