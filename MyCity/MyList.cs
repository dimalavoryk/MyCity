using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Xml.Serialization;
using Android.App;


namespace MyNewProject
{	
	[Serializable]
	public class MyList
	{
		private List<string> m_list;
		public MyList ()
		{
			m_list = new List<string> ();
		}

		public void AddItem (string item)
		{
			m_list.Add (item);	
		}

		public MyList ReadList(string str)
		{
			//string s = str + ".xml";
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			var filePath = System.IO.Path.Combine(sdCardPath+"/Application/Root Studio", str.ToString() + ".xml");
			FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			var myBinaryFormatter = new BinaryFormatter();
			var mc = (MyList) myBinaryFormatter.Deserialize(fStream);
			fStream.Close();
			return mc;
		}

		public void WriteList (string str)
		{
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			if (!Directory.Exists(sdCardPath +"/Application")) Directory.CreateDirectory (sdCardPath +"/Application");
			if (!Directory.Exists(sdCardPath +"/Application/Root Studio")) Directory.CreateDirectory (sdCardPath +"/Application/Root Studio");
			var filePath = System.IO.Path.Combine(sdCardPath+"/Application/Root Studio", str + ".xml");
			FileStream fStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			var myBinaryFormatter = new BinaryFormatter();
			myBinaryFormatter.Serialize(fStream, this);
			fStream.Close();
		}

		public string GetItem (int idx)
		{
			return m_list [idx];
		}


	}
}

