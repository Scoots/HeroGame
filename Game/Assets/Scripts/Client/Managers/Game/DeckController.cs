using UnityEngine;
using System.Collections;

namespace Client.Managers.Game
{
	public class DeckController : Controller
	{
		public static DeckController Instance { get; private set; }

		public override void Init()
		{
			if (Instance == null)
				Instance = this;
		}

		public override void Begin()
		{

		}
	}
}
