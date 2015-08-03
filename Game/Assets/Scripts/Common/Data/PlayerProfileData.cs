namespace Common.Data
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Xml.Serialization;
	using System.Text;

	[Serializable]
	public class PlayerProfileData
	{
		public string PlayerNickname { get; set; }

		public List<int> PlayerCards { get; set; }

		public List<List<int>> PlayerDecks { get; set; }

		public List<int> PlayerGenerals { get; set; }

		public PlayerProfileData()
		{
			this.PlayerCards = new List<int>();
			this.PlayerGenerals = new List<int>();
		}

		public void SaveData(string saveLocation)
		{
			XmlSerializer ser = new XmlSerializer(typeof(PlayerProfileData));
			TextWriter writer = new StreamWriter(saveLocation);
			ser.Serialize(writer, this);
			writer.Close();
		}
	}
}
