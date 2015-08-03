using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UISprite))]
public class OrbCursor : MonoBehaviour 
{
	static public OrbCursor instance;

	// Camera used to draw this cursor
	public Camera uiCamera;

	Transform internalTransform;
	UISprite currentSprite;

	UIAtlas currentAtlas;
	string currentSpriteName;

	/// <summary>
	/// Keep an instance reference so this class can be easily found.
	/// </summary>

	void Awake() { instance = this; }
	void OnDestroy() { instance = null; }

	// Use this for initialization
	void Start () 
	{
		internalTransform = transform;
		currentSprite = GetComponentInChildren<UISprite>();

		if (uiCamera == null)
			uiCamera = NGUITools.FindCameraForLayer(gameObject.layer);

		if (currentSprite != null)
		{
			currentAtlas = currentSprite.atlas;
			currentSpriteName = currentSprite.spriteName;
			if (currentSprite.depth < 200) currentSprite.depth = 200;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = UICamera.lastTouchPosition;

		if (uiCamera != null)
		{
			// Since the screen can be of different than expected size, we want to convert
			// mouse coordinates to view space, then convert that to world position.
			pos.x = Mathf.Clamp01(pos.x / Screen.width);
			pos.y = Mathf.Clamp01(pos.y / Screen.height);
			internalTransform.position = uiCamera.ViewportToWorldPoint(pos);

			// For pixel-perfect results
			if (uiCamera.isOrthoGraphic)
			{
				Vector3 lp = internalTransform.localPosition;
				lp.x = Mathf.Round(lp.x);
				lp.y = Mathf.Round(lp.y);
				internalTransform.localPosition = lp;
			}
		}
		else
		{
			pos.x -= Screen.width * 0.5f;
			pos.y -= Screen.height * 0.5f;
			pos.x = Mathf.Round(pos.x);
			pos.y = Mathf.Round(pos.y);
			internalTransform.localPosition = pos;
		}

		
	}

	static public void Clear()
	{
		if (instance != null && instance.currentSprite != null)
			Set(instance.currentAtlas, instance.currentSpriteName);
	}

	/// <summary>
	/// Override the cursor with the specified sprite.
	/// </summary>

	static public void Set(UIAtlas atlas, string sprite)
	{
		if (instance != null && instance.currentSprite)
		{
			instance.currentSprite.atlas = atlas;
			instance.currentSprite.spriteName = sprite;
			//instance.currentSprite.MakePixelPerfect();
			instance.Update();
		}
	}
}
