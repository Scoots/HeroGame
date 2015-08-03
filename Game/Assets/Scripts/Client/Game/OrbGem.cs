using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Client.Managers.Game;
using Common.Data;

public class OrbGem : MonoBehaviour 
{
	public Enumerations.OrbColor CellType
	{
		get { return this.Cell.CellType; }
		set
		{
			if (this.Cell.CellType == value)
			{
				return;
			}

			this.Cell.CellType = value;
			this.UpdateSprite();
		}
	}

	private bool _isFrozen;
	public bool IsFrozen {
		get { return this._isFrozen; }
		set
		{
			if (this._isFrozen == value)
			{
				return;
			}

			this._isFrozen = value;
			var widget = GetComponent<UIWidget>();
			if (this._isFrozen)
			{
				widget.color = Color.cyan;
			}
			else
			{
				widget.color = Color.white;
			}
		}
	}

	private bool _isDark;
	public bool IsDark {
		get { return this._isDark; }
		set
		{
			if(this._isDark == value)
			{
				return;
			}

			this._isDark = value;
			var widget = GetComponent<UIWidget>();
			if(this._isDark)
			{
				widget.color = Color.black;
			}
			else
			{
				widget.color = Color.white;
			}
		}
	}

	public bool IsSuperOrb
	{
		get { return this.Cell.IsSuperOrb; }
		set
		{
			if (this.Cell.IsSuperOrb == value)
			{
				return;
			}

			this.Cell.IsSuperOrb = value;
			var widget = GetComponent<UIWidget>();
			if (this.Cell.IsSuperOrb)
			{
				widget.color = Color.magenta;
			}
			else
			{
				widget.color = Color.white;
			}
		}
	}

	private bool _isTargeted; 
	public bool IsTargeted { 
		get{return this._isTargeted; } 
		set{this._isTargeted = value;}
	}

	public Cell Cell { get; set; }
	public OrbPoint Loc { get; set; }
	public OrbPuzzle Puzzle { get; set; }

	private string[] sprites = new string[] { "dark", "fire", "heart", "light", "water", "wood" };

	void OnDrop(GameObject go)
	{
		var cardObject = go.GetComponent<OrbCard>();
		if (cardObject == null)
		{
			Debug.Log("No card :(");
			return;
		}

		Debug.Log("We have a card");
		cardObject.BoardDrop = Enumerations.DragTargets.Board;
	}
	/// <summary>
	/// Function used to allow the puzzle board to shade orbs that are being targeted appropriately
	/// </summary>
	/// <param name="c">The color to make this orb</param>
	public void SetColor(Color c)
	{
		this.GetComponent<UIWidget>().color = c;
	}

	/// <summary>
	/// Resets the color to it's default state
	/// TODO - Make this not only color one thing, orbs can be multiple types at once
	/// </summary>
	public void ResetColor()
	{
		var widget = GetComponent<UIWidget>();
		widget.color = Color.white;

		if (this._isFrozen)
		{
			widget.color = Color.cyan;
		}

		if (this.Cell.IsSuperOrb)
		{
			widget.color = Color.magenta;
		}

		if (this._isDark)
		{
			widget.color = Color.black;
		}
	}

	/// <summary>
	/// Function for displaying the board when this orb is hovered over while we are targeting
	/// </summary>
	private void OnHover()
	{
		if(Puzzle.IsBoardTargeting)
		{
			Puzzle.SetupTarget(this);
		}
	}

	public virtual void OnPress(bool isDown)
	{
		if (isDown)
		{
			// If we are board targeting and this orb is hit, activate the ability
			if(Puzzle.IsBoardTargeting)
			{
				if(this.IsTargeted)
				{
					Debug.LogError("Error: Unable to re-select a target");
					return;
				}

				this.IsTargeted = true;

				Puzzle.SelectTarget();
				return;
			}
			
			if (!Puzzle.CanDrag()) return;

			OrbPuzzle.CurrentOrb = gameObject;
			if(this.IsFrozen)
			{
				Debug.Log("Can't drag frozen orb");
				OrbPuzzle.CurrentOrb = null;
				return;
			}

			var sprite = GetComponent<UISprite>();
			if (sprite != null)
			{
				sprite.alpha = 0.5f;
				OrbCursor.Set(sprite.atlas, sprite.spriteName);
			}

			// If we are starting to match with this orb, we can reveal the color
			this.IsDark = false;
			Puzzle.StartMatching();
		}
		else
		{
			if (OrbPuzzle.CurrentOrb != null)
			{
				Puzzle.ResolveTurn();
			}
		}
	}

	public virtual void OnDragOver(GameObject obj)
	{
		var gemObject = obj.GetComponent<OrbGem>();
		if (gemObject == null || gameObject == OrbPuzzle.CurrentOrb)
		{
            var cardProfileObject = obj.GetComponent<OrbCardProfile>(); 
            if(cardProfileObject != null)
            {
                cardProfileObject.BoardDrop = Enumerations.DragTargets.Orb;
                Debug.Log("Gem - We have a Card");
                return; 
            }
			var cardObject = obj.GetComponent<OrbCard>();
			if (cardObject == null)
			{
				return;
			}

			cardObject.BoardDrop = Enumerations.DragTargets.Orb;
			// We have a card object!
			Debug.Log("Gem - We have a card");
			return;
		}

		if (obj != OrbPuzzle.CurrentOrb)
		{
			return;
		}

		if (this.IsFrozen || gemObject.IsFrozen)
		{
			return;
		}

		DoSwapMotion(gameObject, obj);

		// If we are being swapped, we can reveal the color if it is dark
		this.IsDark = false;
		Puzzle.DoSwapOrb(this, gemObject);
	}

	void DoSwapMotion(GameObject newGem, GameObject oldGem)
	{
		Puzzle.DoAnimatedSwap(newGem, oldGem);
	}

	public void UpdateSprite()
	{
		UISprite sprite = this.GetComponent<UISprite>();
		if(this.CellType == Enumerations.OrbColor.Empty)
		{
			sprite.enabled = false;
			return;
		}

		string spriteName = this.sprites[(int)this.CellType - 1];
		sprite.spriteName = "icon_orb_" + spriteName;
		sprite.enabled = true;

        if (IsTargeted)
        {
            //target test
            Debug.Log("Testing Targeted Orb");
            GameObject targeted = (GameObject)Instantiate(Resources.Load("Prefabs/TargetReticle"));
            targeted.transform.parent = gameObject.transform;
            targeted.transform.localPosition = Vector3.zero;
            targeted.transform.localScale = Vector3.one; 
        }
		
	}

	public void RefreshOrb()
	{
		this.Cell.SetRandomOrb(6);
		this.IsFrozen = false;
		this.IsSuperOrb = false;
		this.UpdateSprite();
	}
}
