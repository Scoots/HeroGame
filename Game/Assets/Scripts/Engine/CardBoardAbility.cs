namespace GameEngine.Engine
{
	using System;
	using Common.Data;
	using Common.Logging;
	using Client.Managers.Game;
	using System.Collections.Generic;

	/// <summary>
	/// Pulls user card data into the engine
	/// </summary>
	public class CardBoardAbility : ICardAbility
	{
		static readonly Logger Log = LogManager.CreateLog();

		/// <summary>
		/// Gets or sets the id of the ability
		/// </summary>
		public int BoardAbilityId { get; set; }

		/// <summary>
		/// Gets or sets the name of the ability
		/// </summary>
		public string BoardAbilityName { get; set; }

		/// <summary>
		/// Gets or sets the description of the ability
		/// </summary>
		public string BoardAbilityDescription { get; set; }

		/// <summary>
		/// Gets or sets the time when this ability will activate
		/// </summary>
		public BaseAbilityData.ActivationType BoardAbilityActivate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not this ability can be removed via an ability removal
		/// </summary>
		public bool BoardAbilityCanBeRemoved { get; set; }

		/// <summary>
		/// Gets or sets the type of ability this is
		/// </summary>
		public BoardAbilityData.BoardAbilityType TypeOfBoardAbility { get; set; }

		/// <summary>
		/// Gets or sets the target the ability can hit.  Should determine what kind of targetting the UI shows
		/// </summary>
		public BoardAbilityData.BoardTargetType BoardAbilityTarget { get; set; }

		/// <summary>
		/// Gets or sets the color of the piece that you are targetting for an activity
		/// As an example, if TargetColor is red, and you select Destroy as the ability type,
		/// it will destroy all red gems on the board.
		/// </summary>
		public Enumerations.OrbColor TargetColor { get; set; }

		/// <summary>
		/// Gets or sets the size (x-direction) of the target grid
		/// </summary>
		public int BoardGridSizeX { get; set; }

		/// <summary>
		/// Gets or sets the size (y-direction) of the target grid
		/// </summary>
		public int BoardGridSizeY { get; set; }

		/// <summary>
		/// Gets or sets the color you want to change the selection to.  If you target a row,
		/// and SelectColor is red, it will change the whole row to red
		/// </summary>
		public Enumerations.OrbColor SelectColor { get; set; }

		/// <summary>
		/// Gets or sets the number of selections a user can make to perform an action
		/// </summary>
		public int NumOfSelections { get; set; }

		/// <summary>
		/// Private member that stores all of the ability XML data
		/// </summary>
		private BoardAbilityData _BoardAbilityData { get; set; }

		public CardBoardAbility(int boardAbilityId)
		{
			this.BoardAbilityId = boardAbilityId;
			this._BoardAbilityData = DataStore.Instance.GetData<BoardAbilityData>(boardAbilityId);

			this.BoardAbilityName = this._BoardAbilityData.Name;
			this.BoardAbilityDescription = this._BoardAbilityData.Description;
			this.TypeOfBoardAbility = this._BoardAbilityData.TypeOfBoardAbility;

			this.BoardAbilityActivate = this._BoardAbilityData.Activate;
			this.BoardAbilityTarget = this._BoardAbilityData.BoardTarget;

			this.BoardGridSizeX = this._BoardAbilityData.GridSizeX;
			this.BoardGridSizeY = this._BoardAbilityData.GridSizeY;

			this.SelectColor = this._BoardAbilityData.SelectColor;
			this.TargetColor = this._BoardAbilityData.TargetColor;

			this.NumOfSelections = this._BoardAbilityData.NumOfSelections;

			this.BoardAbilityCanBeRemoved = this._BoardAbilityData.CanBeRemoved;
		}

		public void ActivateAbility(List<OrbGem> targetOrbs)
		{
			UnityEngine.Debug.Log("Activating ability " + this.BoardAbilityName);

			switch(this.TypeOfBoardAbility)
			{
				case BoardAbilityData.BoardAbilityType.ChangeColor:
					foreach (OrbGem o in targetOrbs)
					{
						o.CellType = this.SelectColor;
					}
					break;
				case BoardAbilityData.BoardAbilityType.Freeze:
					UnityEngine.Debug.Log("Attempting to freeze orbs");
					foreach (OrbGem o in targetOrbs)
					{
						// Setting IsFrozen updates the alpha in the property to have it display half invisible
						o.IsFrozen = true;
					}
					break;
				case BoardAbilityData.BoardAbilityType.Refresh:
					UnityEngine.Debug.Log("Attempting to refresh orbs");
					foreach (OrbGem o in targetOrbs)
					{
						// Refreshing an orb resets the frozen state, color, and updates the sprite
						o.RefreshOrb();
					}
					break;
				case BoardAbilityData.BoardAbilityType.Destroy:
					UnityEngine.Debug.Log("Attempting to destroy orbs");
					foreach(OrbGem o in targetOrbs)
					{
						o.CellType = Enumerations.OrbColor.Empty;
					}
					break;
				case BoardAbilityData.BoardAbilityType.Dark:
					foreach (OrbGem o in targetOrbs)
					{
						// Setting dark changes the color in the property
						o.IsDark = true;
					}
					break;
				case BoardAbilityData.BoardAbilityType.SuperOrb:
					foreach (OrbGem o in targetOrbs)
					{
						o.IsSuperOrb = true;
					}
					break;
				default:
					UnityEngine.Debug.Log("DEFAULT CASE FOR ACTIVATE");
					break;
			}
		}

		public bool NeedBoardTarget
		{
			get
			{
				return
					this.BoardAbilityTarget == BoardAbilityData.BoardTargetType.Grid ||
					this.BoardAbilityTarget == BoardAbilityData.BoardTargetType.Row	||
					this.BoardAbilityTarget == BoardAbilityData.BoardTargetType.Column ||
					this.BoardAbilityTarget == BoardAbilityData.BoardTargetType.Selection;
			}
		}
	}
}
