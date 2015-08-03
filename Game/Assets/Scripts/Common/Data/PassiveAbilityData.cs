using System;
using System.Collections.Generic;

namespace Common.Data
{
	/// <summary>
	/// PassiveAbilityData implementation
	/// </summary>
	[Serializable]
	public class PassiveAbilityData : BaseData
	{
		/// <summary>
		/// Enumeration for the types of abilities that can be done to the board/engine
		/// </summary>
		public enum PassiveAbilityType
		{
			ModifyMana,
			ChangeTime,
			PermChangeColor,
			BuffDamage,
			BuffHealth,
			BuffDamageAndHealth
		}

		/// <summary>
		/// Enumeration for who should be affected by the game engine modification
		/// </summary>
		public enum ModificationTarget
		{
			Self,
			Enemy,
			All
		}

		/// <summary>
		/// Gets or sets the description of what the ability does
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the ability can be removed
		/// This is so that abilities granted from your enemy (debuffs) don't get removed when a played uses ability removal
		/// </summary>
		public bool CanBeRemoved { get; set; }

		/// <summary>
		/// Gets or sets the path to the image you wish to use for this ability
		/// </summary>
		public string Image { get; set; }

		/// <summary>
		/// Gets or sets the type of passive ability this is
		/// </summary>
		public PassiveAbilityType TypeOfPassiveAbility { get; set; }

		/// <summary>
		/// Gets or sets the target the passive can affect.
		/// </summary>
		public ModificationTarget PassiveTarget { get; set; }

		/// <summary>
		/// Gets or sets the modifier for mana
		/// </summary>
		public float ManaModifier { get; set; }

		/// <summary>
		/// Gets or sets the time delta to apply to the target's orb-match time
		/// </summary>
		public float TimeDelta { get; set; }

		/// <summary>
		/// Gets or sets the color of the piece that you are targetting for this passive
		/// As an example, if TargetColor is red, and SelectColor is blue, all all reds will be
		/// changed into blue on the board until the card with the ability is removed from play
		/// </summary>
		public Enumerations.OrbColor TargetColor { get; set; }

		/// <summary>
		/// Gets or sets the color you want to change the selection to.  If you target a row,
		/// and SelectColor is red, it will change the whole row to red
		/// </summary>
		public Enumerations.OrbColor SelectColor { get; set; }

		/// <summary>
		/// Gets or sets the amount to buff the target's damage
		/// Must be applied to all cards on a given side
		/// </summary>
		public int DamageBuff { get; set; }

		/// <summary>
		/// Gets or sets the amount to buff the target's health
		/// Must be applied to all cards on a given side
		/// </summary>
		public int HealthBuff { get; set; }
	}
}