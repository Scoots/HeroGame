using UnityEngine;
using System.Collections;

public abstract class Controller
{
	public abstract void Init();
	public virtual void Begin() { }
	public virtual void Destroy() { }
	public virtual void Update(float time, float dt) { }
	public virtual void OnGameStateChange(GameState from, GameState to) { }

	public string Name { get; private set; }

	public Controller()
	{
		Name = this.GetType().Name;
	}
}
