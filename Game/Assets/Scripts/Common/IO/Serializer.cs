using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Common.IO
{
	public class Serializer
	{
		public static void Serialize(Type type, object serializedObject, string path)
		{
			XmlSerializer serializer = new XmlSerializer(type);
			Console.WriteLine("PATH" + path);
			string directoryPath = Path.GetDirectoryName(path);
			Console.WriteLine("DIRECTORYPATH" + directoryPath);
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}
			using (TextWriter tw = new StreamWriter(path))
			{
				tw.NewLine = "\r\n";
				serializer.Serialize(tw, serializedObject);
			}
		}

		public static void Serialize<T>(T serializedObject, string path)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			Console.WriteLine("PATH" + path);
			string directoryPath = Path.GetDirectoryName(path);
			Console.WriteLine("DIRECTORYPATH" + directoryPath);
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}
			using (TextWriter tw = new StreamWriter(path))
			{
				tw.NewLine = "\r\n";
				serializer.Serialize(tw, serializedObject);
			}
		}

		public static T Deserialize<T>(string path)
		{
			object output = Deserialize(typeof(T), path);
			if (output != null)
			{
				return (T)output;
			}
			return default(T);
		}

		public static object Deserialize(Type t, string path)
		{
			XmlSerializer serializer = new XmlSerializer(t);
			if (!File.Exists(path))
			{
				return null;
			}
			using (XmlReader xr = new XmlTextReader(path))
			{
				if (serializer.CanDeserialize(xr))
				{
					return serializer.Deserialize(xr);
				}
			}
			return null;
		}

		public static T DeserializeString<T>(string str)
		{
			object output = DeserializeString(typeof(T), str);
			if (output != null)
			{
				return (T)output;
			}
			return default(T);
		}

		public static object DeserializeString(Type t, string str)
		{
			XmlSerializer serializer = new XmlSerializer(t);
			using (StringReader sr = new StringReader(str))
			{
				return serializer.Deserialize(sr);
			}
		}

		public static T Deserialize<T>(byte[] bytes)
		{
			T output;
			System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
			using (System.IO.MemoryStream memStream = new System.IO.MemoryStream(bytes))
			{
				output = (T)serializer.Deserialize(memStream);
			}
			return output;
		}

		public static string ClassName(string path)
		{
			using (XmlReader xr = new XmlTextReader(path))
			{
				if (xr.Read() && xr.Read() && xr.Read())
				{
					return xr.Name;
				}
			}
			return "";
		}
	}
}

