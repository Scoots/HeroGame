using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
	public class Enumerations
	{
		/// <summary>
		/// Enumeration of the various color types for board pieces
		/// </summary>
		public enum OrbColor
		{
			/// <summary>
			/// Enum entry for the various colors of the orbs
			/// </summary>
			Empty,
			Dark,
			Fire,
			Heart,
			Light,
			Water,
			Wood,
			Length
		}

        public enum Player
        {
            PlayerOne,
            PlayerTwo,
			PlayerNone
        }

        public enum DragTargets
        {
            Board,
            Card,
            Orb, 
            Battle, 
            Enemy,
            None
        }
	}
}
