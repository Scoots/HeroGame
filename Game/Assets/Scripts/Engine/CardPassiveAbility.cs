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
	public class CardPassiveAbility : ICardAbility
	{
		static readonly Logger Log = LogManager.CreateLog();

		/// <summary>
		/// Gets or sets the id of the ability
		/// </summary>
		public int PassiveAbilityId { get; set; }

		/// <summary>
		/// Gets or sets the name of the ability
		/// </summary>
		public string PassiveAbilityName { get; set; }

		/// <summary>
		/// Gets or sets the description of the ability
		/// </summary>
		public string PassiveAbilityDescription { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not this ability can be removed via an ability removal
		/// </summary>
		public bool PassiveAbilityCanBeRemoved { get; set; }

		/// <summary>
		/// Gets or sets the type of ability this is
		/// </summary>
        public PassiveAbilityData.PassiveAbilityType TypeOfPassiveAbility { get; set; }

		/// <summary>
		/// Gets or sets the target the ability can hit.  Should determine what kind of targetting the UI shows
		/// </summary>
        public PassiveAbilityData.ModificationTarget PassiveTarget { get; set; }

		/// <summary>
		/// Gets or sets the modifier for mana
		/// </summary>
		public float ManaModifier { get; set; }

		/// <summary>
		/// Gets or sets the time delta to apply to the player's orb-match time
		/// </summary>
		public float TimeDelta { get; set; }

		/// <summary>
		/// Gets or sets the color of the piece that you are targetting for an activity
		/// As an example, if TargetColor is red, and you select Destroy as the ability type,
		/// it will destroy all red gems on the board.
		/// </summary>
		public Enumerations.OrbColor TargetColor { get; set; }

		/// <summary>
		/// Gets or sets the color you want to change the selection to.  If you target a row,
		/// and SelectColor is red, it will change the whole row to red
		/// </summary>
		public Enumerations.OrbColor SelectColor { get; set; }

        /// <summary>
        /// Gets or sets the amount of damage to buff the targets
        /// </summary>
        public int DamageBuff { get; set; }

        /// <summary>
        /// Gets or sets the amount of health to buff the targets
        /// </summary>
        public int HealthBuff { get; set; }

		/// <summary>
		/// Private member that stores all of the ability XML data
		/// </summary>
		private PassiveAbilityData _PassiveAbilityData { get; set; }

		public CardPassiveAbility(int passiveAbilityId)
		{
            this.PassiveAbilityId = passiveAbilityId;
            this._PassiveAbilityData = DataStore.Instance.GetData<PassiveAbilityData>(passiveAbilityId);

            this.PassiveAbilityName = this._PassiveAbilityData.Name;
            this.PassiveAbilityDescription = this._PassiveAbilityData.Description;
            this.TypeOfPassiveAbility = this._PassiveAbilityData.TypeOfPassiveAbility;

			this.PassiveAbilityCanBeRemoved = this._PassiveAbilityData.CanBeRemoved;
            this.PassiveTarget = this._PassiveAbilityData.PassiveTarget;

			this.SelectColor = this._PassiveAbilityData.SelectColor;
			this.TargetColor = this._PassiveAbilityData.TargetColor;
		}

		private void GetTargets()
		{
			UnityEngine.Debug.Log("Getting orbs to modify");
			List<OrbGem> orbs;
            switch (this.PassiveTarget)
            {
                case PassiveAbilityData.ModificationTarget.Self:
                    break;
                case PassiveAbilityData.ModificationTarget.Enemy:
                    break;
                case PassiveAbilityData.ModificationTarget.All:
                    break;
				default:
					UnityEngine.Debug.Log("DEFAULT CASE FOR TARGET FOR PASSIVE");
                    break;
			}
		}

		public void ActivateAbility()
		{
			UnityEngine.Debug.Log("Activating ability " + this.PassiveAbilityName);

			switch(this.TypeOfPassiveAbility)
            {
                case PassiveAbilityData.PassiveAbilityType.ModifyMana:
                    break;
                case PassiveAbilityData.PassiveAbilityType.ChangeTime:
                    break;
                case PassiveAbilityData.PassiveAbilityType.PermChangeColor:
                    break;
                case PassiveAbilityData.PassiveAbilityType.BuffDamage:
                    break;
                case PassiveAbilityData.PassiveAbilityType.BuffHealth:
                    break;
                case PassiveAbilityData.PassiveAbilityType.BuffDamageAndHealth:
                    break;
				default:
					UnityEngine.Debug.Log("DEFAULT CASE FOR ACTIVATE PASSIVE");
					break;
			}
		}
	}
}
