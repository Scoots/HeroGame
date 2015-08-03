using Client.Managers;
using Common.Data;
using Common.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameEngine.Engine;
using Client.Managers.Game;

namespace Client
{
	public class GameController : MonoBehaviour
	{
		public static GameController Instance { get; private set; }
		public static DateTime StartupTime { get; private set; }
		public static DataLoader Loader;

		private Controller[] mControllers = new Controller[0];

        protected EventManager _eventManager;
        public EventManager Events
        {
            get;
            private set; 
        }

		private GameState _currentGameState;
		public GameState CurrentGameState
		{
			get { return _currentGameState; }
			set
			{
				OnGameStateChange(_currentGameState, value);
				_currentGameState = value;
			}
		}

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);

				LogManager.IsEnabled = true;

				// Setup the Unity Logger!
				//LogManager.AttachLogTarget(new UnityTarget(Logger.Level.Trace, Logger.Level.Error, true));

				StartupTime = DateTime.Now;

				Loader = new DataLoader();
				Loader.DataLoadPath = Application.dataPath;
				Loader.LoadAll();

				// This will pull the player data, after we have instantiated all available cards
				PlayerProfile profile = new PlayerProfile();

				// Losd the Board Scene into the world!
				Application.LoadLevelAdditive("UI");
				Application.LoadLevelAdditive("BoardScene");

				InitializeControllers();

                _eventManager = new EventManager();
                Events = _eventManager; 

				_currentGameState = GameState.Invalid;
			}
		}

		private void InitializeControllers()
		{
			List<Controller> controllers = new List<Controller>();

			controllers.Add(new CameraController());
			controllers.Add(new MainMenuController());
			controllers.Add(new BattleController());

			mControllers = controllers.ToArray();
			foreach (Controller c in mControllers)
			{
				c.Init();
			}
		}

		private bool started = false;
		private void Start()
		{
			if (!started)
			{
				started = true;
				foreach (Controller c in mControllers)
				{
					c.Begin();
				}
			}
		}

		private void Update()
		{
			float time = Time.time;
			float dt = Time.deltaTime;

			foreach (Controller c in mControllers)
			{
				c.Update(time, dt);
			}
		}

		private void OnDestroy()
		{
			foreach (Controller c in mControllers)
			{
				c.Destroy();
			}
		}

		private void OnGameStateChange(GameState from, GameState to)
		{
			if (from == to)
			{
				return;
			}

			foreach (Controller c in mControllers)
			{
				c.OnGameStateChange(from, to);
			}

		}

		private void OnLevelWasLoaded(int inLevel)
		{
			switch (inLevel)
			{
				//case 3:
				//	CurrentGameState = GameState.MainMenu;
				//	break;
				//case 1:
				//	CurrentGameState = GameState.Overworld;
				//	break;
				//default:
				//	CurrentGameState = GameState.Battle;
				//	break;
			}

		}

		IEnumerator LoadGameScene(string inScene)
		{
			// The game level is very simple and loads instantly;
			// Add some artificial delay so we can display the loading screen.
			yield return new WaitForSeconds(1.5f);

			// Load the game level
			Application.LoadLevel(inScene);
		}
	}
}

