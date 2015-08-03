using System;
using System.Collections.Generic;

namespace Common.Data
{
	/// <summary>
	/// BoardAbilityData implementation of BaseAbilityData
	/// </summary>
	[Serializable]
	public class BoardAbilityData : BaseAbilityData
	{
		/// <summary>
		/// Enumeration for the types of abilities that can be done to the borad/engine
		/// </summary>
		public enum BoardAbilityType
		{
			ChangeColor,
			Freeze,
			Refresh,
			Destroy,
			Dark,
			SuperOrb
		}

		/// <summary>
		/// Enumeration for the target of the bard effect
		/// </summary>
		public enum BoardTargetType
		{
			Grid,
			Color,
			Row,
			Column,
			Selection,
			RandomSelection,
			All
		}

		/// <summary>
		/// Gets or sets the type of board ability this is
		/// </summary>
		public BoardAbilityType TypeOfBoardAbility { get; set; }

		/// <summary>
		/// Gets or sets the target the ability can hit.  Should determine what kind of targetting the UI shows
		/// </summary>
		public BoardTargetType BoardTarget { get; set; }

		/// <summary>
		/// Gets or sets the color of the piece that you are targetting for an activity
		/// As an example, if TargetColor is red, and you select Destroy as the ability type,
		/// it will destroy all red gems on the board.
		/// </summary>
		public Enumerations.OrbColor TargetColor { get; set; }

		/// <summary>
		/// Gets or sets the time delta to apply to the player's orb-match time
		/// </summary>
		public float TimeDelta { get; set; }

		/// <summary>
		/// Gets or sets the size (x-direction) of the target grid
		/// </summary>
		public int GridSizeX { get; set; }

		/// <summary>
		/// Gets or sets the size (y-direction) of the target grid
		/// </summary>
		public int GridSizeY { get; set; }

		/// <summary>
		/// Gets or sets the color you want to change the selection to.  If you target a row,
		/// and SelectColor is red, it will change the whole row to red
		/// </summary>
		public Enumerations.OrbColor SelectColor { get; set; }

		/// <summary>
		/// Gets or sets the number of selections a user can make to perform an action
		/// </summary>
		public int NumOfSelections { get; set; }
	}
}