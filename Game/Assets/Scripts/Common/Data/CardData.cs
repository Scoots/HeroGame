namespace Common.Data
{
	using System;
	using System.Collections.Generic;
    using Common.DataTypes;

	/// <summary>
	/// CardData implementation of BaseData
	/// </summary>
	[Serializable]
	public class CardData : BaseData
	{
		/// <summary>
		/// The list of game abilities for this card
		/// </summary>
		private List<int> _gameAbilityList = new List<int>();

		/// <summary>
		/// The list of board abilities for this card
		/// </summary>
		private List<int> _boardAbilityList = new List<int>();

		/// <summary>
		/// The list of passive abilities for this card
		/// </summary>
		private List<int> _passiveAbilityList = new List<int>();

        /// <summary>
        /// The list of the colors (in order) required for this card to play
        /// </summary>
        private List<Enumerations.OrbColor> _cooldownColorList = new List<Enumerations.OrbColor>();
        
        /// <summary>
        /// The list of the amount of each color needed (in order) required for this card to play
        /// </summary>
        private List<int> _cooldownCountList = new List<int>();

		/// <summary>
		/// Gets or sets the description of what the card does
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the id of the board ability you want this card to have
		/// Note - All board abilities are one-use
		/// </summary>
		public int BoardAbility { get; set; }

        /// <summary>
        /// Gets or sets the id of the active ability
		/// Note - This will throw errors if it is not an Active ability
        /// </summary>
        public int AttackAbility { get; set; }

		/// <summary>
		/// Gets or sets the health of the card
		/// </summary>
		public int Health { get; set; }

        /// <summary>
        /// Gets or sets the list of cooldown color data for this card
        /// </summary>
        public List<Enumerations.OrbColor> CooldownColorData
        {
            get { return this._cooldownColorList; }
            set { this._cooldownColorList = value; }
        }

        /// <summary>
        /// Gets or sets the list of cooldown count data for this card
        /// </summary>
        public List<int> CooldownCountList
        {
            get { return this._cooldownCountList; }
            set { this._cooldownCountList = value; }
        }
		/// <summary>
		/// Gets or sets the list of id of the active ability that this card can use on it's turn
		/// </summary>
		public List<int> GameAbilityList
		{
			get { return this._gameAbilityList; }
			set { this._gameAbilityList = value; }
		}

		/// <summary>
		/// Gets or sets the list of id of the passive abilities that this card can use on it's turn
		/// </summary>
		public List<int> PassiveAbilityList
		{
			get { return this._passiveAbilityList; }
			set { this._passiveAbilityList = value; }
		}

		/// <summary>
		/// Gets or sets the path to the image you wish to use for this card
		/// </summary>
		public string Image { get; set; }

		/// <summary>
		/// Gets or sets the path to the image you wish to use for this card
		/// </summary>
		public string ThumbImage { get; set; }
	}
}