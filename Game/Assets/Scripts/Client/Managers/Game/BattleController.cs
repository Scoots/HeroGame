using Common.Data;
using System.Collections.Generic;
using UnityEngine;
using GameEngine.Engine;
using Common.DataTypes;

namespace Client.Managers.Game
{
	public class BattleController : Controller
	{
		public static BattleController Instance { get; private set; }

        public EventManager Events { private get; set; }

		private GameObject puzzleBoard = null;

		private OrbBattle puzzleBattleBoard = null;
		private OrbPuzzle puzzleOrbBoard = null;

		public override void Init()
		{
			if (Instance == null)
				Instance = this;
		}

		public override void Begin()
		{
			if (puzzleBoard == null)
			{
				puzzleBoard = GameObject.FindGameObjectWithTag("BoardObject");
				if (puzzleBoard == null)
				{
					Debug.LogError("WE ARE FUCKED");
					Debug.Break();
				}
				else
				{
					puzzleOrbBoard = puzzleBoard.GetComponentInChildren<OrbPuzzle>();
					puzzleBattleBoard = puzzleBoard.GetComponentInChildren<OrbBattle>();

					// We do not load the board here :)!
					puzzleBoard.SetActive(false);
				}
                //Register Event Manager
                Events = GameController.Instance.Events;
			}

            AddEventListeners();
		}

        public void AddEventListeners()
        {
            //add Event Listeners
            Events.AddEventHandler<CardProfileEvent>(OnCardProfileClicked); 
        }

        public void RemoveEventListeners()
        {
            Events.RemoveEventHandler<CardProfileEvent>(OnCardProfileClicked); 
        }

        public void ShowCardProfile(Card tCard)
        {
            Debug.Log("BattleController - ShowCardProfile"); 
            puzzleBattleBoard.ShowCardProfile(tCard); 
        }

        public void ShroudEvent()
        {
            Debug.Log("BattleCountroller - Shroud Event");
            puzzleBattleBoard.HideCardProfile(); 
        }

        private void OnCardProfileClicked(CardProfileEvent cardProfileEvent)
        {
           
            if (cardProfileEvent.Type == CardProfileEvent.UIEventType.Open)
            {
                Debug.Log("BattleController - CardProfileEvent Open");
            }
            else if (cardProfileEvent.Type == CardProfileEvent.UIEventType.Close)
            {
                puzzleBattleBoard.HideCardProfile(); 
            }
            else if (cardProfileEvent.Type == CardProfileEvent.UIEventType.Press)
            {
                puzzleBattleBoard.HideCardProfile(); 

            }
            //puzzleBattleBoard.HideCardProfile(); 
        }

        #region Game Ability Target Selectors
		public List<ITargetableObject> GetTarget()
		{
			return null;
		}

		public List<ITargetableObject> GetEnemyTarget()
		{
			return null;
		}

		public List<ITargetableObject> GetFriendlyTarget()
		{
			return null;
		}

		public List<ITargetableObject> GetRandomTarget()
		{
			Debug.Log("GetRandomTarget");
			List<GameTarget> targets = this.GetAllTargetData();
			return this.CreateRandomSingleTargetList(targets);
		}

		public List<ITargetableObject> GetEnemyRandomTarget()
		{
			Debug.Log("GetEnemyRandomTarget");
			List<GameTarget> targets = this.GetAllTargetData();
			
			// Find all targets that are NOT the current players
			targets = targets.FindAll(gt => gt.Player != this.puzzleBattleBoard.CurrentPlayerTurn);

			return this.CreateRandomSingleTargetList(targets);
		}

		public List<ITargetableObject> GetFriendlyRandomTarget()
		{
			Debug.Log("GetFriendlyRandomTarget");
			List<GameTarget> targets = this.GetAllTargetData();

			// Find all targets that are NOT the current players
			targets = targets.FindAll(gt => gt.Player == this.puzzleBattleBoard.CurrentPlayerTurn);

			return this.CreateRandomSingleTargetList(targets);
		}

		public List<ITargetableObject> GetRandomGeneral()
		{
			Debug.Log("GetRandomGeneral");
			List<GameTarget> targets = this.GetAllTargetData();

			// Find all targets that are generals
			targets = targets.FindAll(gt => gt.Target.GetType() == typeof(OrbGeneral));

			return this.CreateRandomSingleTargetList(targets);
		}

		public List<ITargetableObject> GetFriendlyGeneral()
		{
			Debug.Log("GetFriendlyGeneral");
			List<GameTarget> targets = this.GetAllTargetData();

			// Find all targets that are generals, then select the friendly general
			targets = targets.FindAll(gt => gt.Target.GetType() == typeof(OrbGeneral));
			targets = targets.FindAll(gt => gt.Player == this.puzzleBattleBoard.CurrentPlayerTurn);

			return this.CreateRandomSingleTargetList(targets);
		}

		public List<ITargetableObject> GetEnemyGeneral()
		{
			Debug.Log("GetEnemyGeneral");
			List<GameTarget> targets = this.GetAllTargetData();

			// Find all targets that are generals, and then select the one for the enemy player
			targets = targets.FindAll(gt => gt.Target.GetType() == typeof(OrbGeneral));
			targets = targets.FindAll(gt => gt.Player != this.puzzleBattleBoard.CurrentPlayerTurn);

			return this.CreateRandomSingleTargetList(targets);
		}

		public List<ITargetableObject> GetAllFriendlyCardTargets()
		{
			Debug.Log("GetAllFriendlyCardTargets");
			List<GameTarget> targets = this.GetAllTargetData();

			targets = targets.FindAll(gt => gt.Target.GetType() == typeof(OrbCard));
			targets = targets.FindAll(gt => gt.Player == this.puzzleBattleBoard.CurrentPlayerTurn);

			return this.CreateMultiTargetList(targets);
		}

		public List<ITargetableObject> GetAllEnemyCardTargets()
		{
			Debug.Log("GetAllEnemyCardTargets");
			List<GameTarget> targets = this.GetAllTargetData();

			targets = targets.FindAll(gt => gt.Target.GetType() == typeof(OrbCard));
			targets = targets.FindAll(gt => gt.Player != this.puzzleBattleBoard.CurrentPlayerTurn);

			return this.CreateMultiTargetList(targets);
		}

		public List<ITargetableObject> GetAllCardTargets()
		{
			Debug.Log("GetAllCardTargets");
			List<GameTarget> targets = this.GetAllTargetData();

			targets = targets.FindAll(gt => gt.Target.GetType() == typeof(OrbCard));

			return this.CreateMultiTargetList(targets);
		}

		public List<ITargetableObject> GetAllFriendlyTargets()
		{
			Debug.Log("GetAllFriendlyTargets");
			List<GameTarget> targets = this.GetAllTargetData();
			targets = targets.FindAll(gt => gt.Player == this.puzzleBattleBoard.CurrentPlayerTurn);

			return this.CreateMultiTargetList(targets);
		}

		public List<ITargetableObject> GetAllEnemyTargets()
		{
			Debug.Log("GetAllEnemyTargets");
			List<GameTarget> targets = this.GetAllTargetData();
			targets = targets.FindAll(gt => gt.Player != this.puzzleBattleBoard.CurrentPlayerTurn);

			return this.CreateMultiTargetList(targets);
		}

		public List<ITargetableObject> GetAllTargets()
		{
			Debug.Log("GetAllTargets");
			List<GameTarget> targets = this.GetAllTargetData();

			return this.CreateMultiTargetList(targets);
		}

		private List<GameTarget> GetAllTargetData()
		{
			if (this.puzzleBattleBoard == null)
			{
				Debug.LogError("No puzzleBattleBoard object available");
				return null;
			}

			if(this.puzzleBattleBoard.TargetableObjects == null)
			{
				Debug.LogError("No TargetableObject list in OrbBattle.");
				return null;
			}

			return this.puzzleBattleBoard.TargetableObjects.FindAll(gt => gt.Target.CanBeTargeted());
		}

		private List<ITargetableObject> CreateMultiTargetList(List<GameTarget> targetList)
		{
			List<ITargetableObject> retList = new List<ITargetableObject>();

			foreach (var target in targetList)
			{
				retList.Add(target.Target);
			}

			return retList;
		}

		private List<ITargetableObject> CreateRandomSingleTargetList(List<GameTarget> targetList)
		{
			List<ITargetableObject> retList = new List<ITargetableObject>();

			int number = UnityEngine.Random.Range(0, targetList.Count);
			GameTarget target = targetList[number];

			retList.Add(target.Target);
			return retList;
		}
        #endregion

		#region Board Ability Target Selectors
		public void StartBoardAbility(CardBoardAbility boardAbility)
		{
			if (this.puzzleOrbBoard == null)
			{
				return;
			}

			this.puzzleOrbBoard.StartBoardAbility(boardAbility);
		}
		#endregion

		public void OnMatchMade(Enumerations.OrbColor inColor, int count)
		{
			if (puzzleBattleBoard != null)
			{
				puzzleBattleBoard.UpdateMana(inColor, count);
			}
		}

		public void OnTurnStart()
		{
			if (this.puzzleBattleBoard != null)
			{
				this.puzzleBattleBoard.OnTurnStart();
			}
		}

		public void OnTurnEnd()
		{
			if (this.puzzleBattleBoard != null)
			{
				this.puzzleBattleBoard.OnTurnEnd();
			}
		}

		public override void OnGameStateChange(GameState from, GameState to)
		{
			base.OnGameStateChange(from, to);

			if (to == GameState.Battle)
			{
				
				puzzleBoard.SetActive(true);

				if (puzzleOrbBoard != null)
				{
					puzzleOrbBoard.Init();
				}

				if (puzzleBattleBoard != null)
				{
					puzzleBattleBoard.Init();
				}

			}

			if (from == GameState.Battle)
			{
				puzzleBoard.SetActive(false);
			}
		}
	}
}
