using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using GameEngine.Engine;

namespace Common.DataTypes
{
	public class GameTarget
	{
		public ITargetableObject Target { get; set; }

		public Enumerations.Player Player { get; set; }

		public GameTarget(Enumerations.Player player, ITargetableObject target)
		{
			this.Target = target;
			this.Player = player;
		}
	}
}
