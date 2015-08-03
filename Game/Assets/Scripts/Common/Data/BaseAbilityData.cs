using System;
using System.Collections.Generic;

namespace Common.Data
{
	/// <summary>
	/// GameAbilityData implementation of BaseData
	/// </summary>
	[Serializable]
	public class BaseAbilityData : BaseData
	{
		public enum ActivationType
		{
			Active,
			Battlecry,
			OnTurn,
			OnDeath
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
		/// Gets or sets when this ability will activate
		/// </summary>
		public ActivationType Activate { get; set; }

		/// <summary>
		/// Gets or sets the path to the image you wish to use for this ability
		/// </summary>
		public string Image { get; set; }
	}
}