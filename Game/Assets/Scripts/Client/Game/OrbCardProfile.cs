using UnityEngine;
using System.Collections;
using GameEngine.Engine;
using Common.DataTypes;
using System.Collections.Generic;
using Common.Data;
using Client.Managers.Game;
using Client; 

public class OrbCardProfile : MonoBehaviour
{
	/// <summary>
	/// The internal card data for this UI object
	/// </summary>
	public Card internalCard;

	// UI Components
	private UISprite cardPortrait; 
	private UISprite cardFrame;
	private UISprite cardPrimaryElement; 
	private UISprite cardSecondaryElement;
	private UISprite cardBackground;
    private UISprite cardRare;
    private UISprite cardMana1;
    private UISprite cardMana2;
    private UISprite cardNameBackground;
    private UISprite cardAbility1Background;
    private UISprite cardAbility2Background;
    private UISprite cardShroud; 

    private UILabel nameLabel;
    private UILabel ability1Label;
    private UILabel ability2Label;
    private UILabel hpLabel;
    private UILabel atkLabel;
    private UILabel mana1Label;
    private UILabel mana2Label;

	public Color fireColor; 
	public Color waterColor; 
	public Color woodColor; 
	public Color lightColor; 
	public Color darkColor; 
	public Color heartColor; 
	public Color emptyColor;

    public Enumerations.DragTargets BoardDrop = Enumerations.DragTargets.None;

    private EventManager Events; 
   
    /// <summary>
    /// Initializes all the widget references
    /// </summary>
    /// <param name="inCard">The card that this UI element references</param>
    public void Awake()
    {
        // Assign Prefab components
        cardPortrait = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "CardPortrait");
        cardFrame = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "PlayerFraming");
        cardPrimaryElement = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "ElementPrimary");
        cardSecondaryElement = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "ElementSecondary");
        cardBackground = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "CardBG");
        cardRare = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "CardRarity");
        cardMana1 = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "ManaCost1");
        cardMana2 = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "ManaCost2");
        cardNameBackground = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "NameBackground");
        cardAbility1Background = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "Ability1BG");
        cardAbility2Background = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "Ability2BG");
        cardShroud = UIUtils.GetChildOfTypeWithName<UISprite>(gameObject, "CardShroud");

        nameLabel = UIUtils.GetChildOfTypeWithName<UILabel>(gameObject, "NameLabel");
        ability1Label = UIUtils.GetChildOfTypeWithName<UILabel>(gameObject, "Ability1Label");
        ability2Label = UIUtils.GetChildOfTypeWithName<UILabel>(gameObject, "Ability2Label");
        hpLabel = UIUtils.GetChildOfTypeWithName<UILabel>(gameObject, "HealthStatLabel");
        atkLabel = UIUtils.GetChildOfTypeWithName<UILabel>(gameObject, "AttackStatLabel");
        mana1Label = UIUtils.GetChildOfTypeWithName<UILabel>(gameObject, "ManaCost1Label");
        mana2Label = UIUtils.GetChildOfTypeWithName<UILabel>(gameObject, "ManaCost2Label");

        //let's just batch check and see if we're missing anything
        if (cardPortrait == null || cardFrame == null || cardPrimaryElement == null ||
                        cardSecondaryElement == null || cardBackground == null)
        {
            UnityEngine.Debug.Log("CARD PROFILE PREFAB FAILED");
            return;
        }

        Events = GameController.Instance.Events; 
    }
   
	/// <summary>
	/// Sets up the card data when given a specific card
	/// </summary>
	/// <param name="inCard">The card that this UI element references</param>
	public void Setup(Card inCard)
	{
		this.internalCard = inCard;

		RenderCard();
	}

	public void RenderCard()
	{
		if (internalCard == null ) 
		{
			return; 
		}

		cardPortrait.spriteName = internalCard.SpritePortrait; 

        // TODO: Fix this so it displays ALL colors, not just the first one
		//check color
        if(internalCard.CardCooldownData.Count == 0)
        {
            Debug.LogError("Card doesn't have cooldown data, you should fix this. " + internalCard.CardName);
            internalCard.CardCooldownData.Add(new Common.DataTypes.CooldownData(Enumerations.OrbColor.Empty, 1));
        }

		string iconName = "icon_orb_heart";
        Color manaColor = emptyColor; 
        switch(internalCard.CardCooldownData[0].Color)
        {
            case Enumerations.OrbColor.Empty:
                iconName = "empty";
                manaColor = emptyColor; 
                break;
            case Enumerations.OrbColor.Fire:
			    iconName = "icon_orb_fire";
                manaColor = fireColor;
                break;
            case Enumerations.OrbColor.Wood:
			    iconName = "icon_orb_wood";
                manaColor = woodColor;
                break;
            case Enumerations.OrbColor.Water:
                iconName = "icon_orb_water";
                manaColor = waterColor;
                break;
            case Enumerations.OrbColor.Dark:
			    iconName = "icon_orb_dark";
                manaColor = darkColor;
                break;
            case Enumerations.OrbColor.Light:
			    iconName = "icon_orb_light";
                manaColor = lightColor;
                break;
            case Enumerations.OrbColor.Heart:
			    iconName = "icon_orb_heart";
                manaColor = heartColor;
                break;
        }

		if (iconName == "empty")
		{
			cardPrimaryElement.gameObject.SetActive(false);
            cardMana1.gameObject.SetActive(false);
		}
		else
		{
			cardPrimaryElement.gameObject.SetActive(true);
			cardPrimaryElement.spriteName = iconName;

            cardMana1.gameObject.SetActive(true);
            cardMana1.GetComponent<UIWidget>().color = manaColor;
            mana1Label.text = ""+ internalCard.CardCooldownData[0].Count;
		}

		// for now disable secondary element (we don't have any of them...) 
		cardSecondaryElement.gameObject.SetActive(false);
        cardMana2.gameObject.SetActive(false); 

		//Setup Name Label
        if (internalCard.CardName == null)
        {
            nameLabel.gameObject.SetActive(false);
            cardNameBackground.gameObject.SetActive(false);
            cardPrimaryElement.gameObject.SetActive(false);
            cardSecondaryElement.gameObject.SetActive(false); 
        }
        else
        {
            nameLabel.gameObject.SetActive(true);
            cardNameBackground.gameObject.SetActive(true);
            nameLabel.text = internalCard.CardName; 
        }

        //Setup Stats
        atkLabel.text = "" + internalCard.CardAttackAbility.AbilityDamageOrHealing;
        hpLabel.text = "" + internalCard.CurrentHealth; 

        //Setup Descriptions
        if(internalCard.CardAttackAbility == null)
        {
            cardAbility1Background.gameObject.SetActive(false);
            ability1Label.gameObject.SetActive(false);
        }
		else
        {
            cardAbility1Background.gameObject.SetActive(true);
            ability1Label.gameObject.SetActive(true);

            ability1Label.text = internalCard.CardAttackAbility.AbilityDescription; 
        }

        if (internalCard.CardBoardAbility == null)
        {
            cardAbility2Background.gameObject.SetActive(false);
            ability2Label.gameObject.SetActive(false);
        }
        else
        {
            cardAbility2Background.gameObject.SetActive(true);
            ability2Label.gameObject.SetActive(true);

            ability2Label.text = internalCard.CardBoardAbility.BoardAbilityDescription;
        }

		//check to see if this card is active
		if (internalCard.InPlay == true)
		{
			cardFrame.color = Color.white; 
		}
		else
		{
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
			cardFrame.color = frameColor; 
		}
	}

	void Update()
	{
		RenderCard(); 
	}

    void OnEnable()
    {
        Debug.Log("OrbcardProfileEnabled"); 
        
        UIEventListener.Get(cardBackground.gameObject).onPress += OnPress;
        UIEventListener.Get(cardShroud.gameObject).onPress += HideProfile; 
    }
    void OnDisable()
    {
        Debug.Log("OrbcardProfileDisabled"); 
        
        UIEventListener.Get(cardBackground.gameObject).onPress -= OnPress;
        UIEventListener.Get(cardShroud.gameObject).onPress -= HideProfile; 
    }

    public void ShowProfile(GameObject go)
    {
        gameObject.SetActive(true);
        Events.HandleEvent(CardProfileEvent.GetInstance(this, CardProfileEvent.UIEventType.Open));
        //cardShroud.gameObject.SetActive(true); 
    }
    public void ShowProfile()
    {
        GameObject go = null;
        ShowProfile(go); 
    }

    public void HideProfile(GameObject go, bool isPressed)
    {
        isPressed = UICamera.IsPressed(cardBackground.gameObject);
        HideProfile(go); 
    }

    public void HideProfile(GameObject go)
    {
        gameObject.SetActive(false);
        Events.HandleEvent(CardProfileEvent.GetInstance(this));
        //cardShroud.gameObject.SetActive(false); 
    }
    public void HideProfile()
    {
        GameObject go = null;
        HideProfile(go); 
    }

   
    public void OnPress(GameObject go, bool isPressed) { isPressed = UICamera.IsPressed(cardBackground.gameObject);  OnPress(isPressed); }

	/// <summary>
	/// Called when the card is selected from the UI
	/// Depending on the state of the card, it will do different things, such as use an ability or play a card
	/// </summary>
	/// <param name="isPressed">Whether or not the card is pressed</param>
    void OnPress(bool isPressed)
    {
        if (isPressed)
        {
            //NGUITools.SetActive(GetComponent<UIWidget>().gameObject, false);
            Events.HandleEvent(CardProfileEvent.GetInstance(this, CardProfileEvent.UIEventType.Press));
        }
        else
        {
            //NGUITools.SetActive(GetComponent<UIWidget>().gameObject, true);
            Events.HandleEvent(CardProfileEvent.GetInstance(this, CardProfileEvent.UIEventType.Press));
        }
        Debug.Log("CardProfile - OnPress " + isPressed); 
    }
}
