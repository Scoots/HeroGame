namespace GameEngine.Engine
{
    using System;
    using System.Collections.Generic;
	using Common.Data;
	using Common.DataTypes;
    using Common.Logging;
    using System.Linq;

    /// <summary>
    /// Pulls user card data into the engine
    /// </summary>
    public class Card
    {
        static readonly Logger Log = LogManager.CreateLog();

        /// <summary>
        /// Gets or sets the id of the card
        /// </summary>
        public int CardId { get; set; }

		public bool CanUseAttack { get; set; }

        /// <summary>
        /// Gets or sets the name of the card
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// Gets or sets the description of the card
        /// </summary>
        public string CardDescription { get; set; }

        /// <summary>
        /// Gets or sets the active attack ability for this card
        /// </summary>
        public CardGameAbility CardAttackAbility { get; set; }

        /// <summary>
        /// Gets or sets the base health of the card
        /// </summary>
		public int CardHealth { get; set; }

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
				if (this._currentHealth > this.CardHealth)
				{
					this._currentHealth = this.CardHealth;
				}

				if (this._currentHealth <= 0)
				{
					UnityEngine.Debug.Log("Killed card " + this.CardName);
					this.ResetCard();
					this.OnCardDeath();
				}
			}
		}

		public CardBoardAbility CardBoardAbility { get; set; }

		/// <summary>
		/// Gets the sprite name of the card
		/// </summary>
		public string SpritePortrait { get; set; }

		/// <summary>
		/// Gets the sprite name of the card
		/// </summary>
		public string SpriteTeam { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the card is currently in play
		/// </summary>
		public bool InPlay
		{
			get { return this._inPlay; }
			set 
			{ 
				this._inPlay = value;
				if (this._inPlay)
				{
					this.ActivateOnPlayAbilities();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the card is currently blind
		/// </summary>
		public bool Blind { get; set; }

        /// <summary>
        /// Gets or sets the list of cooldown data for this card
        /// </summary>
        public List<CooldownData> CardCooldownData { get; set; }

        /// <summary>
        /// The list of passive abilities that this card has
        /// </summary>
		public List<CardPassiveAbility> PassiveAbilities = new List<CardPassiveAbility>();

        /// <summary>
        /// The list of battlecry abilities this card has
        /// </summary>
		public List<CardGameAbility> BattlecryAbilities = new List<CardGameAbility>();

        /// <summary>
        /// The list of OnTurn abilities this card has
        /// </summary>
		public List<CardGameAbility> OnTurnAbilities = new List<CardGameAbility>();

        /// <summary>
        /// The list of Deathrattle abilities this card has
        /// </summary>
		public List<CardGameAbility> OnDeathAbilities = new List<CardGameAbility>();

        /// <summary>
        /// The original list of cooldown data so that we can reset when the card is moved back into your hand
        /// </summary>
        private List<CooldownData> _originalCooldownDataList = new List<CooldownData>();

		/// <summary>
		/// Boolean indicating whether the card is in play, needed to do InPlay logic
		/// </summary>
		private bool _inPlay;

		/// <summary>
		/// The current health of the card, needed to have the property do logic
		/// </summary>
		private int _currentHealth;

		/// <summary>
		/// The data loaded for the card
		/// </summary>
		private CardData _cardData { get; set; }

        public Card(int cardId)
        {
			this._inPlay = false;

            this.CardId = cardId;
            this._cardData = DataStore.Instance.GetData<CardData>(cardId);

            this.CardName = this._cardData.Name;
            this.CardDescription = this._cardData.Description;
            this.CardAttackAbility = new CardGameAbility(this._cardData.AttackAbility);
			if (this.CardAttackAbility.AbilityActivate != BaseAbilityData.ActivationType.Active)
			{
				UnityEngine.Debug.LogError("Error: The submitted attack ability is not an active ability.");
			}
			this.CanUseAttack = true;

			this.CardBoardAbility = new CardBoardAbility(this._cardData.BoardAbility);
            this.CardHealth = this._cardData.Health;
            this._currentHealth = this._cardData.Health;

            // We assume that CooldownColorData and CooldownCountList are going to be the same size
            // The editor forces them to update in lock step with each other
            this.CardCooldownData = new List<CooldownData>();
            for (int i = 0; i < this._cardData.CooldownColorData.Count; ++i )
            {
                this.CardCooldownData.Add(new CooldownData(this._cardData.CooldownColorData[i], this._cardData.CooldownCountList[i]));
            }

            this._originalCooldownDataList = this.CardCooldownData.Select(d => new CooldownData(d.Color, d.Count)).ToList();

			this.SpritePortrait = this._cardData.Image; 
			this.SpriteTeam = this._cardData.ThumbImage;

            foreach (int gameAbilityId in this._cardData.GameAbilityList)
            {
				var abil = new CardGameAbility(gameAbilityId);
				this.SetupAbilityTypeLists(abil.AbilityActivate, abil);
            }

            foreach (int passiveAbilityId in this._cardData.PassiveAbilityList)
            {
                var abil = new CardPassiveAbility(passiveAbilityId);
                this.PassiveAbilities.Add(abil);
            }
        }

		/// <summary>
		/// When the turn starts, reset active abilities
		/// If the card is blinded, all active abilities will be set to unusable
		/// </summary>
		public void OnTurnStart()
        {
			// If the card is blind, then you can't use the ability this turn
			this.CanUseAttack = !this.Blind;

			this.ActivateOnTurnAbilities();
		}

		/// <summary>
		/// Actions that are performed at the end of a user's turn
		/// </summary>
		public void OnTurnEnd()
		{
			this.Blind = false;
		}

		/// <summary>
		/// Actions to be performed at the death of this card
		/// </summary>
		public void OnCardDeath()
		{
			this.ActivateOnDeathAbilities();
		}

		/// <summary>
		/// Determines if the card can be played based on it's current cooldowns and whether it is in play
		/// </summary>
		/// <returns>If the card can be played</returns>
		public bool CanPlayCard()
		{
            // Can't play it if it is in play
            if (this.InPlay)
            {
				UnityEngine.Debug.LogError("Error: Card already in play");
                return false;
            }

            // Can't play it if there is still some cooldown to be had
			foreach (CooldownData cd in this.CardCooldownData)
			{
				if (cd.Count > 0)
				{
					UnityEngine.Debug.LogError("Error: Still waiting on cooldown " + cd.Color.ToString());
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Plays this card onto the game board
		/// </summary>
		public void PlayCard()
		{
			if (!this.CanPlayCard())
			{
				return;
			}

			// If we can play it, mark it in play
			this.InPlay = true;
		}

		/// <summary>
		/// Resets the card to it's default state in regards to cooldown and health, and removes it from the game board
		/// </summary>
        public void ResetCard()
        {
            UnityEngine.Debug.Log("Resetting card " + this.CardName + " to default cooldown and inplay state");
			this.InPlay = false;
			this.CardHealth = this._cardData.Health;
			this.CurrentHealth = this._cardData.Health;

			// Do a full copy so we aren't referencing the second list after the card is put back in the hand
			// TODO - Remove this when we go to a deck/draw mechanic, unless cards can be drawn multiple times
			this.CardCooldownData = this._originalCooldownDataList.Select(d => new CooldownData(d.Color, d.Count)).ToList();

        }

		/// <summary>
		/// Helper function for setting up the lists of the different types of abilities that this card has
		/// </summary>
		/// <param name="activationType">When the ability is activated</param>
		/// <param name="ability">The ability to be activated</param>
		private void SetupAbilityTypeLists(BaseAbilityData.ActivationType activationType, CardGameAbility ability)
		{
			switch (activationType)
			{
				case BaseAbilityData.ActivationType.Active:
					// There should be no active abilities that aren't tied to attack anymore
					UnityEngine.Debug.LogError("Error: The data isn't correct, there shouldn't be active abilities here.");
					break;
				case BaseAbilityData.ActivationType.Battlecry:
					this.BattlecryAbilities.Add(ability);
					break;
				case BaseAbilityData.ActivationType.OnTurn:
					this.OnTurnAbilities.Add(ability);
					break;
				case BaseAbilityData.ActivationType.OnDeath:
					this.OnDeathAbilities.Add(ability);
					break;
			}
		}

		/// <summary>
		/// Activates any abilities that are triggered when the card is played
		/// </summary>
		private void ActivateOnPlayAbilities()
		{
			foreach (CardGameAbility ability in this.BattlecryAbilities)
			{
				// TODO: Set up the ability to target your Battlecry abilities
                ability.ActivateAbility();
			}
		}

		/// <summary>
		/// Activates any abilities that are triggered when the card is active when your turn starts
		/// </summary>
		private void ActivateOnTurnAbilities()
		{
			foreach (CardGameAbility ability in this.OnTurnAbilities)
			{
				// TODO: Set up the ability to target your own OnTurn abilities
                ability.ActivateAbility();
			}
		}

		/// <summary>
		/// Activates any abilities that are triggered when the card dies
		/// </summary>
		private void ActivateOnDeathAbilities()
		{
			foreach (CardGameAbility ability in this.OnDeathAbilities)
			{
				// TODO: Set up the ability to target your own OnDeath abilities
                ability.ActivateAbility();
			}
		}
    }
}
