using UnityEngine;
using System.Collections;
using GameEngine.Engine;
using Common.DataTypes;
using System.Collections.Generic;
using Common.Data;
using Client.Managers.Game;
using Client; 

public class OrbEnemyCard : MonoBehaviour, ITargetableObject
{
	/// <summary>
	/// The internal card data for this UI object
	/// </summary>
	public Card internalCard;

	// UI Components
	private UISprite cardPortrait; 
	private UISprite cardFrameActive;
    private UISprite cardPrimaryElement; 
	private UISprite cardSecondaryElement;
    private UISprite cardFrameInactive;
    private UILabel cardLabel;
    private UILabel cardAttackLabel;
    private UILabel cardHealthLabel;
	private UISprite cardForeground; 
	private UISprite cardBackground; 

	public Color fireColor; 
	public Color waterColor; 
	public Color woodColor; 
	public Color lightColor; 
	public Color darkColor; 
	public Color heartColor; 
	public Color emptyColor;

    public Enumerations.DragTargets BoardDrop = Enumerations.DragTargets.None;

    private EventManager _events; 
    

	/// <summary>
	/// ITargetableObject interface member designating if the card is blind
	/// </summary>
	public bool Blind
	{
		get { return this.internalCard.Blind; }
		set { this.internalCard.Blind = value; }
	}

    public void Awake()
    {
        if (_events == null)
        {
            _events = GameController.Instance.Events;
        }

        //assign the UI vars
        this.cardLabel = UIUtils.GetChildOfTypeWithName<UILabel>(gameObject, "CardLabel");
        this.cardAttackLabel = UIUtils.GetChildOfTypeWithName<UILabel>(gameObject, "AttackLabel");
        this.cardHealthLabel = UIUtils.GetChildOfTypeWithName<UILabel>(gameObject, "HealthLabel");

        // Assign Prefab components
        cardPortrait = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "CardPortrait");
        cardFrameActive = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "EnemyFraming_Active");
        cardPrimaryElement = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "ElementPrimary");
        cardSecondaryElement = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "ElementSecondary");
        cardFrameInactive = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "EnemyFraming_Inactive");
        cardForeground = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "FillForeground");
        cardBackground = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "FillBackground"); 
    }
	/// <summary>
	/// Sets up the card data when given a specific card
	/// </summary>
	/// <param name="inCard">The card that this UI element references</param>
	public void Setup(Card inCard)
	{
		this.TargetName = "2" + inCard.CardName;
		
		if (this.cardLabel != null)
		{
			this.cardLabel.text = inCard.CardName;
		}
		this.internalCard = inCard;
        
		//let's just batch check and see if we're missing anything
        if (cardPortrait == null || cardFrameActive == null || cardFrameInactive == null ||
                cardPrimaryElement == null || cardSecondaryElement == null || 
                cardForeground == null || cardBackground == null) {
						UnityEngine.Debug.Log ("CARD PREFAB FAILED");
						return; 
		}

        this.internalCard = inCard; 

		RenderCard(); 
	}

    public void OnEnable()
    {
        //add event Listeners
        _events.AddEventHandler<CardProfileEvent>(OnCardProfileClicked);
    }

    public void OnDisable()
    {
        //remove event listeners
        _events.RemoveEventHandler<CardProfileEvent>(OnCardProfileClicked); 
    }

	public void RenderCard()
	{
		if (internalCard == null) 
		{
			return; 
		}

		cardLabel.text = internalCard.CardCooldownData[0].Count.ToString();
        cardAttackLabel.text = internalCard.CardAttackAbility.AbilityDamageModifier.ToString();
        cardHealthLabel.text = internalCard.CardHealth.ToString(); 

		cardPortrait.spriteName = internalCard.SpriteTeam; 

        // TODO: Fix this so it displays ALL colors, not just the first one
		//check color
        if(internalCard.CardCooldownData.Count == 0)
        {
            Debug.LogError("Card doesn't have cooldown data, you should fix this. " + internalCard.CardName);
            internalCard.CardCooldownData.Add(new Common.DataTypes.CooldownData(Enumerations.OrbColor.Empty, 1));
        }

		string iconName = "icon_orb_heart"; 
        switch(internalCard.CardCooldownData[0].Color)
        {
            case Enumerations.OrbColor.Empty:
                iconName = "empty";
                break;
            case Enumerations.OrbColor.Fire:
			    iconName = "icon_orb_fire"; 
                break;
            case Enumerations.OrbColor.Wood:
			    iconName = "icon_orb_wood"; 
                break;
            case Enumerations.OrbColor.Water:
                iconName = "icon_orb_water"; 
                break;
            case Enumerations.OrbColor.Dark:
			    iconName = "icon_orb_dark"; 
                break;
            case Enumerations.OrbColor.Light:
			    iconName = "icon_orb_light"; 
                break;
            case Enumerations.OrbColor.Heart:
			    iconName = "icon_orb_heart"; 
                break;
        }

		if (iconName == "empty")
		{
			cardPrimaryElement.gameObject.SetActive(false); 
		}
		else
		{
			cardPrimaryElement.gameObject.SetActive(true);
			cardPrimaryElement.spriteName = iconName; 
		}

		// for now disable secondary element (we don't have any of them...) 
		cardSecondaryElement.gameObject.SetActive(false); 

		//display countdown if it has a count
        if (internalCard.CardCooldownData[0].Count > 0)
        {
            cardLabel.gameObject.SetActive(true);
        }
        else
        {
            cardLabel.gameObject.SetActive(false); 
        }
		

		//temporarily disable FG/BG fades
		cardForeground.gameObject.SetActive(false); 
		cardBackground.gameObject.SetActive(false);

		//check to see if this card is active
		if (internalCard.InPlay == true)
		{
            cardFrameInactive.gameObject.SetActive(false);
            cardFrameActive.gameObject.SetActive(true); 
			cardFrameActive.color = Color.white; 
		}
		else
		{
            cardFrameInactive.gameObject.SetActive(true);
            cardFrameActive.gameObject.SetActive(false); 
			var frameColor = Color.grey; 
			switch(internalCard.CardCooldownData[0].Color)
			{
			case Enumerations.OrbColor.Empty:
				frameColor = emptyColor;
				break;
			case Enumerations.OrbColor.Fire:
				frameColor = fireColor;
				break;
			case Enumerations.OrbColor.Wood:
				frameColor = woodColor;
				break;
			case Enumerations.OrbColor.Water:
				frameColor = waterColor;
				break;
			case Enumerations.OrbColor.Dark:
				frameColor = darkColor;
				break;
			case Enumerations.OrbColor.Light:
				frameColor = lightColor;
				break;
			case Enumerations.OrbColor.Heart:
				frameColor = heartColor;
				break;
			}
			cardFrameInactive.color = frameColor; 
		}

	}

	/// <summary>
	/// Called when the card is selected from the UI
	/// Depending on the state of the card, it will do different things, such as use an ability or play a card
	/// </summary>
	/// <param name="isPressed">Whether or not the card is pressed</param>
	void Update()
	{
		RenderCard(); 
	}

    /// <summary>
	/// Called when the card is clicked from the UI
	/// A Click will be counted if the touchevent begins and ends on the same collider
	/// </summary>
	void OnClick()
    {
        Debug.Log("Enemy Card Clicked");
        BattleController.Instance.ShowCardProfile(internalCard); 
    }

	/// <summary>
	/// Called when the card is selected from the UI
	/// Depending on the state of the card, it will do different things, such as use an ability or play a card
	/// </summary>
	/// <param name="isPressed">Whether or not the card is pressed</param>
	void OnPress(bool isPressed)
	{
        Debug.Log("Enemy Card - OnPress " + isPressed);
		// TODO: Add a check to make sure only the active player is playing cards
        // TODO: See if we are supposed to be playing a game or board ability here.  For now assuming ALWAYS board abilities
		
	}

	/// <summary>
	/// Interface function for ITargetableObject
	/// Returns true if the card is able to be targeted
	/// We can later add stealth abilities and update this function
	/// </summary>
	/// <returns>A value indicating if the card can be targeted</returns>
	public bool CanBeTargeted()
	{
		return this.internalCard.InPlay;
	}

	/// <summary>
	/// Interface function for ITargetableObject
	/// Returns true if the max health is higher than the current health of the card
	/// </summary>
	/// <returns>A value indicating if this card's health is less than max</returns>
    public bool IsDamaged()
    {
        return this.internalCard.InPlay && this.internalCard.CardHealth > this.internalCard.CurrentHealth;
    }

	/// <summary>
	/// Interface function for ITargetableObject
	/// </summary>
	/// <param name="amount">The amount to modify the current health</param>
    public void ModifyCurrentHealth(int amount)
    {
        this.internalCard.CurrentHealth += amount;

		// TODO - Remove this, it is only here to provide the TargetName
		if (this.internalCard.CurrentHealth <= 0)
		{
			Debug.Log("Killed card " + this.TargetName);
		}
    }

	/// <summary>
	/// Interface function for ITargetableObject
	/// </summary>
	/// <param name="amount">The amount to buff the card's damage</param>
    public void BuffDamage(int amount)
    {
		// The modifier is what causes damage to go up or down - we don't want to modify the card ability directly
		this.internalCard.CardAttackAbility.AbilityDamageModifier += amount;
    }

	/// <summary>
	/// Interface function for ITargetableObject
	/// </summary>
	/// <param name="amount">The amount to buff the card's health</param>
    public void BuffHealth(int amount)
    {
        this.internalCard.CardHealth += amount;
        this.internalCard.CurrentHealth += amount;
    }

	/// <summary>
	/// Interface function for ITargetableObject
	/// Kills the card
	/// </summary>
	public void Kill()
	{
		Debug.Log("Killing card " + this.TargetName);
		this.internalCard.ResetCard();
	}

	/// <summary>
	/// Interface function for ITargetableObject
	/// Notifies the card that it's turn has started
	/// </summary>
    public void OnTurnStart()
    {
        this.internalCard.OnTurnStart();
    }

	/// <summary>
	/// Interface function for ITargetableObject
	/// Notifies the card that it's turn has ended
	/// </summary>
    public void OnTurnEnd()
    {
        this.internalCard.OnTurnEnd();
    }




	public string TargetName { get; set; }
	public int GetCurrentHealth()
	{
		return this.internalCard.CurrentHealth;
	}

	public int GetMaxHealth()
	{
		return this.internalCard.CardHealth;
	}

    public void OnCardProfileClicked(CardProfileEvent cardProfileEvent)
    {
        if (cardProfileEvent.CardType == this.internalCard && cardProfileEvent.Type == CardProfileEvent.UIEventType.Press)
        {
            //TODO: This doesn't work right now because i can't seem to pass the touch or current touched object 
            //OnPress(true); 
        }
    }
}
