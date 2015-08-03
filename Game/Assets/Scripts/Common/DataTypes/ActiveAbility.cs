using GameEngine.Engine;

namespace Common.DataTypes
{
	public class ActiveAbility
	{
		public ICardAbility Ability { get; set; }

		public bool HasBeenUsed { get; set; }

		public ActiveAbility(ICardAbility ability, bool hasBeenUsed)
		{
			this.Ability = ability;
			this.HasBeenUsed = hasBeenUsed;
		}
	}
}
