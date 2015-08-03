namespace GameEngine.Engine
{
	using System;
	using Common.Data;
	using Common.Logging;
	using Client.Managers.Game;
	using System.Collections.Generic;
	using Common.DataTypes;
	using UnityEngine;

	/// <summary>
	/// Pulls user card data into the engine
	/// </summary>
	public class CardGameAbility : ICardAbility
	{
		static readonly Logger Log = LogManager.CreateLog();

		/// <summary>
		/// Gets or sets the id of the ability
		/// </summary>
		public int AbilityId { get; set; }

		/// <summary>
		/// Gets or sets the name of the ability
		/// </summary>
		public string AbilityName { get; set; }

		/// <summary>
		/// Gets or sets the description of the ability
		/// </summary>
		public string AbilityDescription { get; set; }

		/// <summary>
		/// Gets or sets the type of ability this is
		/// </summary>
		public GameAbilityData.AbilityType AbilityTypeOfAbility { get; set; }

		/// <summary>
		/// Gets or sets the damage or healing of the ability
		/// </summary>
		public int AbilityDamageOrHealing { get; set; }

		/// <summary>
		/// Gets or sets the amount to buff damage
		/// </summary>
		public int AbilityDamageBuff { get; set; }

		/// <summary>
		/// Gets or sets the amount to buff health
		/// </summary>
		public int AbilityHealthBuff { get; set; }

		/// <summary>
		/// Gets or sets the amount to modify damage
		/// </summary>
		public int AbilityDamageModifier { get; set; }

		/// <summary>
		/// Gets or sets the type of ability that this ability should remove
		/// </summary>
		public GameAbilityData.AbilityRemovalType AbilityTypeToRemove { get; set; }

		/// <summary>
		/// Gets or sets the ability that this ability can give to another card (or all cards)
		/// </summary>
		public CardGameAbility AbilityToGrant { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the card should be removed from play after use
		/// </summary>
		public bool AbilityCanBeRemoved { get; set; }

		/// <summary>
		/// Gets or sets the time when this ability will activate
		/// </summary>
		public BaseAbilityData.ActivationType AbilityActivate { get; set; }

		/// <summary>
		/// Gets or sets the target the ability can hit.  Should determine what kind of targetting the UI shows
		/// </summary>
		public GameAbilityData.TargetType AbilityTarget { get; set; }

		/// <summary>
		/// Private member that stores all of the ability XML data
		/// </summary>
		private GameAbilityData _GameAbilityData { get; set; }

		/// <summary>
		/// Initializes a new instance of the CardGameAbility class
		/// </summary>
		/// <param name="abilityId">The id of the ability to pull out of our data store</param>
		public CardGameAbility(int abilityId)
		{
			this.AbilityId = abilityId;
			this._GameAbilityData = DataStore.Instance.GetData<GameAbilityData>(abilityId);

			this.AbilityName = this._GameAbilityData.Name;
			this.AbilityDescription = this._GameAbilityData.Description;
			this.AbilityTypeOfAbility = this._GameAbilityData.TypeOfAbility;
			this.AbilityDamageOrHealing = this._GameAbilityData.DamageOrHealing;
			this.AbilityDamageBuff = this._GameAbilityData.DamageBuff;
			this.AbilityHealthBuff = this._GameAbilityData.HealthBuff;
			this.AbilityTypeToRemove = this._GameAbilityData.TypeToRemove;

			//try
			//{
			//	this.AbilityToGrant = new CardGameAbility(this._GameAbilityData.AbilityToGrant);
			//}
			//catch (Exception e)
			//{
			//	Log.Debug("Error loading card, possible infinite loop: " + e.Message);
			//}

			this.AbilityCanBeRemoved = this._GameAbilityData.CanBeRemoved;
			this.AbilityActivate = this._GameAbilityData.Activate;
			this.AbilityTarget = this._GameAbilityData.Target;
		}

		private bool MustTarget()
		{
			return this.AbilityTarget == GameAbilityData.TargetType.Target
				|| this.AbilityTarget == GameAbilityData.TargetType.TargetEnemy
				|| this.AbilityTarget == GameAbilityData.TargetType.TargetFriendly;
		}

		/// <summary>
		/// Function that will select the appropriate target based on the ability target type
		/// </summary>
		/// <returns>The list of targets that should be affected by this ability</returns>
		private List<ITargetableObject> GetTarget()
		{
			switch(this.AbilityTarget)
			{
				case GameAbilityData.TargetType.Target:
					// TODO - Fix this to not be random
					Debug.Log("case GameAbilityData.TargetType.Target");
					return BattleController.Instance.GetRandomTarget();
				case GameAbilityData.TargetType.TargetEnemy:
					// TODO - Fix this to not be random
					Debug.Log("case GameAbilityData.TargetType.TargetEnemy");
					return BattleController.Instance.GetEnemyRandomTarget();
				case GameAbilityData.TargetType.TargetFriendly:
					// TODO - Fix this to not be random
					Debug.Log("case GameAbilityData.TargetType.TargetFriendly");
					return BattleController.Instance.GetFriendlyRandomTarget();
				case GameAbilityData.TargetType.Random:
					Debug.Log("case GameAbilityData.TargetType.Random");
					return BattleController.Instance.GetRandomTarget();
				case GameAbilityData.TargetType.EnemyRandom:
					Debug.Log("case GameAbilityData.TargetType.EnemyRandom");
					return BattleController.Instance.GetEnemyRandomTarget();
				case GameAbilityData.TargetType.FriendlyRandom:
					Debug.Log("case GameAbilityData.TargetType.FriendlyRandom");
					return BattleController.Instance.GetFriendlyRandomTarget();
				case GameAbilityData.TargetType.RandomGeneral:
					Debug.Log("case GameAbilityData.TargetType.RandomGeneral");
					return BattleController.Instance.GetRandomGeneral();
				case GameAbilityData.TargetType.FriendlyGeneral:
					Debug.Log("case GameAbilityData.TargetType.FriendlyGeneral");
					return BattleController.Instance.GetFriendlyGeneral();
				case GameAbilityData.TargetType.EnemyGeneral:
					Debug.Log("case GameAbilityData.TargetType.EnemyGeneral");
					return BattleController.Instance.GetEnemyGeneral();
				case GameAbilityData.TargetType.AllFriendlyCards:
					Debug.Log("case GameAbilityData.TargetType.AllFriendlyCards");
					return BattleController.Instance.GetAllFriendlyCardTargets();
				case GameAbilityData.TargetType.AllEnemyCards:
					Debug.Log("case GameAbilityData.TargetType.AllEnemyCards");
					return BattleController.Instance.GetAllEnemyCardTargets();
				case GameAbilityData.TargetType.AllCards:
					Debug.Log("case GameAbilityData.TargetType.AllCards");
					return BattleController.Instance.GetAllCardTargets();
				case GameAbilityData.TargetType.AllFriendly:
					Debug.Log("case GameAbilityData.TargetType.AllFriendly");
					return BattleController.Instance.GetAllFriendlyTargets();
				case GameAbilityData.TargetType.AllEnemy:
					Debug.Log("case GameAbilityData.TargetType.AllEnemy");
					return BattleController.Instance.GetAllEnemyTargets();
				case GameAbilityData.TargetType.All:
					Debug.Log("case GameAbilityData.TargetType.All");
					return BattleController.Instance.GetAllTargets();
				default:
					return null;
			}
		}

		/// <summary>
		/// Interface function for activating this ability
		/// Switches on the type of ability and performs the specified action on the targets
		/// </summary>
		public void ActivateAbility()
		{
			Debug.Log("Activating game ability " + this.AbilityName);
			var targetList = this.GetTarget();

			if(targetList == null)
			{
				Debug.LogError("Error: Returned targetList was null");
				return;
			}

			switch(this.AbilityTypeOfAbility)
			{
				case GameAbilityData.AbilityType.Damage:
					foreach (ITargetableObject ito in targetList)
					{
						int dmg = this.AbilityDamageOrHealing + this.AbilityDamageModifier;
						if (dmg <= 0)
						{
							Debug.Log("Can't do shit with no damage");
							return;
						}

						Debug.Log("Old: " + ito.GetCurrentHealth() + "/" + ito.GetMaxHealth());
						Debug.Log("Dealing " + dmg + " damage to " + ito.TargetName + " from ability " + this.AbilityName);
						ito.ModifyCurrentHealth(-1 * (dmg));
						if(ito.CanBeTargeted())
						{
							Debug.Log("New: " + ito.GetCurrentHealth() + "/" + ito.GetMaxHealth());
						}
					}
					break;
				case GameAbilityData.AbilityType.Healing:
					foreach (ITargetableObject ito in targetList)
					{
						Debug.Log("Old: " + ito.GetCurrentHealth() + "/" + ito.GetMaxHealth());
						Debug.Log("Healing " + this.AbilityDamageOrHealing + " damage from " + ito.TargetName + " from ability " + this.AbilityName);
						ito.ModifyCurrentHealth(this.AbilityDamageOrHealing);
						if (ito.CanBeTargeted())
						{
							Debug.Log("New: " + ito.GetCurrentHealth() + "/" + ito.GetMaxHealth());
						}
					}
					break;
				case GameAbilityData.AbilityType.BuffDamage:
					foreach (ITargetableObject ito in targetList)
					{
						ito.BuffDamage(this.AbilityDamageBuff);
					}
					break;
				case GameAbilityData.AbilityType.BuffHealth:
					foreach (ITargetableObject ito in targetList)
					{
						ito.BuffDamage(this.AbilityHealthBuff);
					}
					break;
				case GameAbilityData.AbilityType.BuffDamageAndHealth:
					foreach (ITargetableObject ito in targetList)
					{
						ito.BuffDamage(this.AbilityDamageBuff);
						ito.BuffDamage(this.AbilityHealthBuff);
					}
					break;
				case GameAbilityData.AbilityType.AbilityRemoval:
					Debug.LogError("AbilityRemoval isn't going to be functional just yet.");
					break;
				case GameAbilityData.AbilityType.AbilityGrant:
					Debug.LogError("AbilityGrant isn't going to be functional just yet.");
					break;
				case GameAbilityData.AbilityType.Blind:
					foreach (ITargetableObject ito in targetList)
					{
						ito.Blind = true;
					}
					break;
				case GameAbilityData.AbilityType.Kill:
					foreach (ITargetableObject ito in targetList)
					{
						ito.Kill();
					}
					break;
			}
		}
	}
}
