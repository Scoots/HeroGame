namespace GameEngine.Engine
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Xml.Serialization;

	using UnityEngine;
	using Common.Data;

	/// <summary>
	/// Class for the player details, including card data, nickname, etc
	/// </summary>
	public class PlayerProfile
	{
		/// <summary>
		/// Private member for the data loaded from the XML file
		/// </summary>
		private PlayerProfileData _playerProfileData;

		/// <summary>
		/// Location of where the ProfileData is saved
		/// </summary>
		private string _profileLocation = Application.persistentDataPath + "/playerData.xml";

		/// <summary>
		/// Gets or sets the nickname of the player
		/// </summary>
		public string PlayerNickname { get; set; }

		/// <summary>
		/// Gets or sets the list of player's cards
		/// </summary>
		public List<Card> PlayerCards { get; set; }

		public List<CardGameGeneral> PlayerGenerals { get; set; }

		/// <summary>
		/// Initializes a new instance of the PlayerProfile class
		/// Loads all data from the file and correlates it to game objects
		/// </summary>
		public PlayerProfile()
		{
			// Create a dummy profile if one doesn't exist
			if (!File.Exists(this._profileLocation))
			{
				PlayerProfileData newProfile = new PlayerProfileData();
				newProfile.PlayerNickname = "Lazy Nickname";
				newProfile.PlayerCards.Add(587235471);
				newProfile.PlayerCards.Add(1898670758);
				newProfile.PlayerCards.Add(1834989642);
				newProfile.PlayerCards.Add(2130988533);
				newProfile.PlayerCards.Add(828636498);
				newProfile.PlayerCards.Add(587235471);
				newProfile.PlayerGenerals.Add(831436277);
				XmlSerializer ser = new XmlSerializer(typeof(PlayerProfileData));
				TextWriter writer = new StreamWriter(this._profileLocation);
				ser.Serialize(writer, newProfile);
				writer.Close();
			}

			this.PlayerCards = new List<Card>();
			this.PlayerGenerals = new List<CardGameGeneral>();

			XmlSerializer reader = new XmlSerializer(typeof(PlayerProfileData));
			StreamReader file = new StreamReader(this._profileLocation);
			this._playerProfileData = reader.Deserialize(file) as PlayerProfileData;
			file.Close();

			this.PlayerNickname = this._playerProfileData.PlayerNickname;
			foreach (int i in this._playerProfileData.PlayerCards)
			{
				this.PlayerCards.Add(new Card(i));
			}

			foreach (int i in this._playerProfileData.PlayerGenerals)
			{
				this.PlayerGenerals.Add(new CardGameGeneral(i));
			}
		}

		/// <summary>
		/// Adds a new card to the player profile, and saves it
		/// Maintains the state of both the xml data and the in game representation
		/// </summary>
		/// <param name="cardId">The id of the card to add</param>
		/// <returns>Whether or not the action succeeded</returns>
		public bool AddNewCard(int cardId)
		{
			if(this._playerProfileData.PlayerCards.IndexOf(cardId) != -1)
			{
				return false;
			}

			this._playerProfileData.PlayerCards.Add(cardId);
			this._playerProfileData.SaveData(this._profileLocation);
			this.PlayerCards.Add(new Card(cardId));
			return true;
		}

		/// <summary>
		/// Removes a card from the player profile and saves it
		/// Maintains the state of both the xml data and the in game representation
		/// </summary>
		/// <param name="cardId">The id of the card to remove</param>
		/// <returns>Whether or not the action succeeded</returns>
		public bool RemoveCard(int cardId)
		{
			if(this._playerProfileData.PlayerCards.IndexOf(cardId) == -1)
			{
				return false;
			}
			
			bool retVal = this._playerProfileData.PlayerCards.Remove(cardId);
			this._playerProfileData.SaveData(this._profileLocation);
			foreach(Card c in this.PlayerCards)
			{
				if(c.CardId == cardId)
				{
					this.PlayerCards.Remove(c);
					break;
				}
			}
			return retVal;
		}
	}
}