using System.Text;
using UnityEngine;

namespace Client.Managers
{
	public class CameraController : Controller
	{
		public static CameraController Instance { get; private set; }

		public GameObject MainCameraObject { get; private set; }
		public GameObject GameBoardCamera { get; private set; }

		public override void Init()
		{
			if (Instance == null)
				Instance = this;
		}

		public override void Begin()
		{
			
			// Set the MainCameraObject and prevent it from getting destroyed!
			MainCameraObject = GameObject.Find("Main Camera");
			GameObject.DontDestroyOnLoad(MainCameraObject);

			GameBoardCamera = GameObject.Find("BoardCamera");
			GameObject.DontDestroyOnLoad(GameBoardCamera);
		}
	}
}

