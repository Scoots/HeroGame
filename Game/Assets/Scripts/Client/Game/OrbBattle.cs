using UnityEngine;
using System.Collections;
using GameEngine.Engine;
using Common.Data;
using Common.DataTypes;
using System.Linq;
using System.Collections.Generic;

public class OrbBattle : MonoBehaviour
{
    //manually set card containers from board scene
    public GameObject playerCardContainer;
    public GameObject enemyCardContainer; 
    public int maxHandSize = 6;

    public OrbCard[] cards;
    public OrbEnemyCard[] enemyCards; 

    public OrbCardProfile cardProfile;

    // TODO - Ideally the player data would be stored in the card or orbcard, and this list would just
    // be the game cards + generals (who also would have the data in them).  Just through in a hacky
    // implementation to get myself unblocked
    public List<GameTarget> TargetableObjects { get; set; }

    private PlayerProfile _playerOneProfile;
    private PlayerProfile _playerTwoProfile; 

    // TODO - Remove this!  I'm leaving this in so I have stuff to target
    private OrbGeneral _playerOneGeneral = new OrbGeneral();
    private OrbGeneral _playerTwoGeneral = new OrbGeneral();

    public Enumerations.Player CurrentPlayerTurn = Enumerations.Player.PlayerOne;

    public void Init()
    {
        this.TargetableObjects = new List<GameTarget>();

        this._playerOneProfile = new PlayerProfile();
        this._playerTwoProfile = new PlayerProfile();
        this._playerOneGeneral.Setup(new CardGameGeneral(13805062));
        this._playerTwoGeneral.Setup(new CardGameGeneral(1813937494));

        Debug.Log("Adding the two generals to the target list");
        this.TargetableObjects.Add(new GameTarget(Enumerations.Player.PlayerOne, this._playerOneGeneral));
        this.TargetableObjects.Add(new GameTarget(Enumerations.Player.PlayerTwo, this._playerTwoGeneral));

        DisplayCards();

        //Hide the Card Profile
        HideCardProfile();
    }

    public virtual void OnDragOver(GameObject obj)
    {
        var cardObject = obj.GetComponent<OrbCard>();
        if (cardObject == null)
        {
            return;
        }

        cardObject.BoardDrop = Enumerations.DragTargets.Battle;
        // We have a card object!
        Debug.Log("Battle - We have a card");
    }

    private void DisplayCards()
    {
        //TODO : move an initializer out of the display code, move it into where shit gets initialized. 
        if (playerCardContainer != null)
        {
            // we get all the cards!
            cards = playerCardContainer.GetComponentsInChildren<OrbCard>();
        }
        if (enemyCardContainer != null)
        {
            Debug.Log("fetch enemy cards"); 
            enemyCards = enemyCardContainer.GetComponentsInChildren<OrbEnemyCard>(); 
        }

        if (cards != null)
        {
            if (this._playerOneProfile.PlayerCards.Count == 0)
            {
                Debug.LogError("You need at LEAST one card to play");
                return;
            }

            if (this._playerOneProfile.PlayerCards.Count < cards.Length)
            {
                Debug.LogError("You don't have enough cards to play.  I'll fake it for now.");
            }

            for (int i = 0; i < cards.Length; ++i)
            {
                OrbCard card = cards[i];
                if (this._playerOneProfile.PlayerCards.Count - 1 < i)
                {
                    // Add more cards until you have a full board
                    card.Setup(this._playerOneProfile.PlayerCards[0]);
                }
                else
                {
                    card.Setup(this._playerOneProfile.PlayerCards[i]);
                }

                this.TargetableObjects.Add(new GameTarget(Enumerations.Player.PlayerOne, card));
            }
        }

        //enemy card setup
        Debug.Log("Enemy Card Stuff"); 
        if (enemyCards != null)
        {
            Debug.Log("build enemy deck"); 
            if (this._playerTwoProfile.PlayerCards.Count == 0)
            {
                Debug.LogError("You need at LEAST one card to play");
                return;
            }

            if (this._playerTwoProfile.PlayerCards.Count < cards.Length)
            {
                Debug.LogError("You don't have enough cards to play.  I'll fake it for now.");
            }

            for (int i = 0; i < enemyCards.Length; ++i)
            {
                OrbEnemyCard enemycard = enemyCards[i];
                if (this._playerTwoProfile.PlayerCards.Count - 1 < i)
                {
                    // Add more cards until you have a full board
                    enemycard.Setup(this._playerTwoProfile.PlayerCards[0]);
                }
                else
                {
                    enemycard.Setup(this._playerTwoProfile.PlayerCards[i]);
                }

                this.TargetableObjects.Add(new GameTarget(Enumerations.Player.PlayerTwo, enemycard));
            }
        }
    }

    public void UpdateMana(Enumerations.OrbColor inColor, int count)
    {
        foreach (var card in cards)
        {
            foreach (CooldownData cd in card.internalCard.CardCooldownData)
            {
                if (cd.Color == inColor && !card.internalCard.InPlay)
                {
                    cd.Count -= count;
                }
            }

            // TODO - Remove - this is here for debug purposes only
            if (card.internalCard.CardCooldownData.All(cd => cd.Count == 0) && !card.internalCard.InPlay)
            {
                Debug.Log("Card " + card.internalCard.CardName + " is ready to be played");
            }
        }
    }

    public void ShowCardProfile(Card tCard)
    {
        Debug.Log("OrbBattle - ShowCardProfile");
        cardProfile.ShowProfile();
        cardProfile.Setup(tCard);

    }

    public void HideCardProfile()
    {
        cardProfile.HideProfile();
    }

    public void OnTurnStart()
    {
        // Get all objects that need OnTurnStart called for the player whos turn is starting
        List<GameTarget> currentGameObjects = this.TargetableObjects.FindAll(gt => gt.Player == this.CurrentPlayerTurn);

        foreach (GameTarget gt in currentGameObjects)
        {
            gt.Target.OnTurnStart();
        }
    }

    public void OnTurnEnd()
    {
        // Get all objects that need to have EndTurn called for the player whos turn is ending
        List<GameTarget> previousPlayerGameObjects = this.TargetableObjects.FindAll(gt => gt.Player == this.CurrentPlayerTurn);

        foreach (GameTarget gt in previousPlayerGameObjects)
        {
            gt.Target.OnTurnEnd();
        }
    }
}
