using System;
using System.Collections.Generic;

namespace Common.Data
{
	/// <summary>
	/// GeneralData implementation of BaseData
	/// </summary>
	[Serializable]
	public class GameGeneralData : BaseData
	{
		/// <summary>
		/// Gets or sets the description of what the card does
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the health of the card
		/// </summary>
		public int Health { get; set; }

		/// <summary>
		/// Gets or sets the path to the image you wish to use for this card
		/// </summary>
		public string Image { get; set; }
	}
}