using UnityEngine;
using System.Collections;

namespace Client.Managers.Game
{
	public class MainMenuController : Controller
	{
		public static MainMenuController Instance { get; private set; }

		private GameObject mainUI = null;
		private UIButton startGameButton = null;

		public override void Init()
		{
			if (Instance == null)
				Instance = this;
		}

		public override void Begin()
		{
			if (mainUI == null)
			{
				mainUI = GameObject.FindGameObjectWithTag("MainMenu");
				if (mainUI == null)
				{
					Debug.LogError("WE ARE FUCKED");
					Debug.Break();
				}
				else
				{
					// We do not load the board here :)!
					//mainUI.SetActive(false);


					startGameButton = UIUtils.GetChildOfTypeWithName<UIButton>(mainUI, "StartGameButton");
					if (startGameButton == null)
					{
						Debug.LogError("WE ARE FUCKED");
						Debug.Break();
					}
					else
					{
						EventDelegate.Add(startGameButton.onClick, TestFunction);
					}
				}
			}
		}

		private void TestFunction()
		{
			GameController.Instance.CurrentGameState = GameState.Battle;
		}

		public override void OnGameStateChange(GameState from, GameState to)
		{
			base.OnGameStateChange(from, to);

			if (from == GameState.MainMenu)
			{
				mainUI.SetActive(false);
			}

			if (to == GameState.MainMenu)
			{
				mainUI.SetActive(true);
			}
		}
	}
}
