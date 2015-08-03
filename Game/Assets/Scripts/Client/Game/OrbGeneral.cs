using UnityEngine;
using System.Collections;
using GameEngine.Engine;
using Common.DataTypes;
using System.Collections.Generic;
using Common.Data;

public class OrbGeneral : ITargetableObject
{
	public bool Blind { get; set; }

	private CardGameGeneral internalGeneral;

	public void Setup(CardGameGeneral inGeneral)
	{
		this.internalGeneral = inGeneral;
	}

	public bool CanBeTargeted()
	{
		return true;
	}

    public bool IsDamaged()
    {
        return this.internalGeneral.Health > this.internalGeneral.CurrentHealth;
    }

    public void ModifyCurrentHealth(int amount)
    {
        this.internalGeneral.CurrentHealth += amount;
    }

    public void BuffDamage(int amount)
    {
        // Generals don't have attack!!
    }

    public void BuffHealth(int amount)
    {
		// TODO - Possible error with "buffing" negative health amounts
		// If a player has lost health (2 / 10) and then is buffed by -2, they will die.
		// Check with Jack and team to determine if we just want to lower max, not total, in that case
        this.internalGeneral.Health += amount;
		this.internalGeneral.CurrentHealth += amount;
    }

	public void Kill()
	{
		Debug.Log("Killing general! " + this.TargetName);
		this.internalGeneral.CurrentHealth = 0;
	}

    public void OnTurnStart() { }

	public void OnTurnEnd() { }


	public string TargetName {
		get { return this.internalGeneral.Name; }
		set { this.internalGeneral.Name = value; }
	}
	public int GetCurrentHealth()
	{
		return this.internalGeneral.CurrentHealth;
	}
	public int GetMaxHealth()
	{
		return this.internalGeneral.Health;
	}
}
