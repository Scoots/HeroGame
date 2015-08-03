namespace GameEngine.Engine
{
	using Common.Data;
	using Common.Logging;
	using UnityEngine;

	/// <summary>
	/// Pulls user general data from the data store
	/// </summary>
	public class CardGameGeneral
	{
		static readonly Logger Log = LogManager.CreateLog();

		public int GameGeneralId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int Health { get; set; }

		public string Image { get; set; }
		private int _currentHealth;

		/// <summary>
		/// Gets or sets the current health of the general
		/// If the health reaches 0, the game ends
		/// </summary>
		public int CurrentHealth
		{
			get { return this._currentHealth; }
			set
			{
				this._currentHealth = value;
				if (this._currentHealth > this.Health)
				{
					this._currentHealth = this.Health;
				}

				if (this._currentHealth <= 0)
				{
					Debug.Log("The game is over!");
				}
			}
		}

		private GameGeneralData _GameGeneralData;

		public CardGameGeneral(int gameGeneralId)
		{
			this.GameGeneralId = gameGeneralId;
			this._GameGeneralData = DataStore.Instance.GetData<GameGeneralData>(gameGeneralId);

			this.Name = this._GameGeneralData.Name;
			this.Description = this._GameGeneralData.Description;
			this.Health = this._GameGeneralData.Health;
			this._currentHealth = this._GameGeneralData.Health;
			this.Image = this._GameGeneralData.Image;
		}
	}
}
