namespace GameEngine.Engine
{
    public interface ITargetableObject
    {
		bool Blind { get; set; }

		bool CanBeTargeted();
        bool IsDamaged();

        void ModifyCurrentHealth(int amount);
		void BuffDamage(int amount);
		void BuffHealth(int amount);

		void Kill();

		void OnTurnStart();
        void OnTurnEnd();

		// TODO - Delete these!
		string TargetName { get; set; }
		int GetMaxHealth();
		int GetCurrentHealth();
    }
}
