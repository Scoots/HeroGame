using System;
using System.Collections.Generic;

namespace Common.Data
{
	/// <summary>
	/// GameAbilityData implementation of BaseAbilityData
	/// </summary>
	[Serializable]
	public class GameAbilityData : BaseAbilityData
	{
		/// <summary>
		/// Enumeration that details what type of target the ability can hit
		/// </summary>
		public enum TargetType
		{
			/// <summary>
			/// Entry for when you want the ability target to be selected by the user
			/// </summary>
			Target,

			/// <summary>
			/// Entry for when you want the ability target to be an enemy selected by the user
			/// </summary>
			TargetEnemy,

			/// <summary>
			/// Entry for when you want the ability target to be a friendly selected by the user
			/// </summary>
			TargetFriendly,

			/// <summary>
			/// Entry for when you want the ability target to be selected completely at random
			/// </summary>
			Random,

			/// <summary>
			/// Entry for when you want the ability to target an enemy at random
			/// </summary>
			EnemyRandom,

			/// <summary>
			/// Entry for when you want the ability to target a friendly at random
			/// </summary>
			FriendlyRandom,

			/// <summary>
			/// Entry for when you want the ability to target a random general
			/// </summary>
			RandomGeneral,

			/// <summary>
			/// Entry for when you want the ability to target the friendly general
			/// </summary>
			FriendlyGeneral,

			/// <summary>
			/// Entry for when you want the ability to target the enemy general
			/// </summary>
			EnemyGeneral,

			/// <summary>
			/// Entry for when you want the ability to target all friendly cards EXCLUDING the general
			/// </summary>
			AllFriendlyCards,

			/// <summary>
			/// Entry for when you want the ability to target all enemy cards EXCLUDING the general
			/// </summary>
			AllEnemyCards,

			/// <summary>
			/// Entry for when you want the ability to target all cards EXCLUDING the generals
			/// </summary>
			AllCards,

			/// <summary>
			/// Entry for when you want the ability to target all friendly cards INCLUDING the general
			/// </summary>
			AllFriendly,

			/// <summary>
			/// Entry for when you want the ability to target all enemy cards INCLUDING the general
			/// </summary>
			AllEnemy,

			/// <summary>
			/// Entry for when you want the ability to target all cards INCLUDING the generals
			/// </summary>
			All

		}

		public enum AbilityType
		{
			Damage,
			Healing,
			BuffDamage,
			BuffHealth,
			BuffDamageAndHealth,
			AbilityRemoval,
			AbilityGrant,
			Blind,
			Kill
		}

		public enum AbilityRemovalType
		{
			/// <summary>
			/// Entry that means all types of abilities should be removed
			/// </summary>
			All,

			/// <summary>
			/// Entry for only removing board type abilities
			/// </summary>
			Board,

			/// <summary>
			/// Entry for Removing the non-board abilities
			/// This can be broken up later into damage, healing, kill, etc
			/// </summary>
			Game
		}

		/// <summary>
		/// Gets or sets the type of ability this is
		/// </summary>
		public AbilityType TypeOfAbility { get; set; }

		/// <summary>
		/// Gets or sets the damage or healing the ability does
		/// </summary>
		public int DamageOrHealing { get; set; }

		/// <summary>
		/// Gets or sets the amount to buff the creature's damage
		/// </summary>
		public int DamageBuff { get; set; }

		/// <summary>
		/// Gets or sets the amount to buff the creature's health
		/// </summary>
		public int HealthBuff { get; set; }

		/// <summary>
		/// Gets or sets the ability that you want to give to another card
		/// This is to allow for passives and damaging effects that just happen
		/// </summary>
		public int AbilityToGrant { get; set; }

		/// <summary>
		/// Gets or sets the type of ability that a removal ability removes
		/// This will allow us to remove game abilities or board abilities from a player/card
		/// </summary>
		public AbilityRemovalType TypeToRemove { get; set; }

		/// <summary>
		/// Gets or sets the type of target the ability can hit
		/// </summary>
		public TargetType Target { get; set; }
	}
}