using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class AnimatedOrbGem : OrbGem 
{
	private GameObject ownedGem = null;
	private Vector3 oldGemPostion;

	public void Start()
	{
		enabled = false;
		GetComponent<UISprite>().enabled = false;

		Object.DestroyImmediate(GetComponent<BoxCollider>());
	}

	public void Init(Vector3 newPostion, GameObject owningGem)
	{
		gameObject.transform.localPosition = owningGem.transform.localPosition;
		oldGemPostion = newPostion;

		ownedGem = owningGem;

		ownedGem.gameObject.transform.localPosition = newPostion;
		ownedGem.GetComponent<UISprite>().enabled = false;
		
		GetComponent<UISprite>().spriteName = owningGem.GetComponent<UISprite>().spriteName;
		GetComponent<UISprite>().enabled = true;
	}

	public void Move()
	{
		TweenPosition.Begin(gameObject, 0.2f, oldGemPostion).AddOnFinished(OnFinished);
	}

	public void OnFinished()
	{
		ownedGem.GetComponent<UISprite>().enabled = true;
		GetComponent<UISprite>().enabled = false;
		enabled = false;
	}

}
