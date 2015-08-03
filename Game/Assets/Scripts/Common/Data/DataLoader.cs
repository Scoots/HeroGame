namespace Common.Data
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Xml.Serialization;
	using UnityEngine;
	using Common.Logging;
	using Common.IO;

	/// <summary>
	/// Loads the game data from their XML locations
	/// </summary>
	public class DataLoader
	{
		static Logger Log = LogManager.CreateLog();

		public string DataLoadPath { get; set; }

		/// <summary>
		/// Loads all of the data for the game
		/// </summary>
		public void LoadAll()
		{
			LoadDataType<CardData>("Data/CardData");
			LoadDataType<GameAbilityData>("Data/GameAbilityData");
			LoadDataType<BoardAbilityData>("Data/BoardAbilityData");
			LoadDataType<PassiveAbilityData>("Data/PassiveAbilityData");
			LoadDataType<GameGeneralData>("Data/GameGeneralData");
		}

		/// <summary>
		/// Loads the specified data type from the xml location provided
		/// </summary>
		/// <typeparam name="T">The type to load</typeparam>
		/// <param name="folderPath">The location of the xml file</param>
		private void LoadDataType<T>(string folderPath) where T : BaseData
		{
			// Load the resource
			TextAsset t = Resources.Load<TextAsset>(folderPath);

			List<T> dataList = null;
			try
			{
				XmlSerializer reader = new XmlSerializer(typeof(List<T>));
				StringReader file = new StringReader(t.text);
				dataList = reader.Deserialize(file) as List<T>;
				file.Close();
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
			}

			if (dataList != null)
			{
				foreach (T data in dataList)
				{
					DataStore.Instance.AddData<T>(data);
				}
			}
		}
	}
}
