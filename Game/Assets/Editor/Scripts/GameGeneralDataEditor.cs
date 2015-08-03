using System.CodeDom.Compiler;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Common.Data;

namespace Assets.Editor.Scripts
{
	/// <summary>
	/// Editor that allows you to create and update ability data
	/// </summary>
	class GameGeneralDataEditor : BaseDataEditor<GameGeneralData>
	{
		/// <summary>
		/// Gets the data type name for use in the base class functionality
		/// </summary>
		protected override string DataTypeName
		{
			get
			{
				return "GameGeneralData";
			}
		}

		/// <summary>
		/// Gets the single data name for use in the base class functionality
		/// </summary>
		protected override string SingleDataName
		{
			get
			{
				return "Game General Data";
			}
		}

		/// <summary>
		/// Initializes the ability data editor window
		/// </summary>
		[MenuItem("Hero/GameGeneralData Editor")]
		public static void Init()
		{
			GameGeneralDataEditor editor = GetWindow<GameGeneralDataEditor>();
			editor.minSize = new Vector2(300, 300);
			editor.Show();
		}

		/// <summary>
		/// Determines what file we are going to be reading/writing
		/// </summary>
		/// <returns>The general data file location</returns>
		protected override string GetXMLFileLocation()
		{
			return "Data/GameGeneralData";
		}

		/// <summary>
		/// Draws the contents of the general data editor for each single ability
		/// </summary>
		protected override void DrawActorInfo()
		{
			GUILayout.BeginHorizontal();
			RenderReadonly(CurrentItem.Id, "Id :");
			GUILayout.EndHorizontal();

			CurrentItem.Name = RenderText(CurrentItem.Name, "Name :", "Display the name of the general");
			CurrentItem.Description = RenderText(
					CurrentItem.Description, "Description :",
					"A brief description of the general.");
			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();
			CurrentItem.Health = RenderInt(CurrentItem.Health, "Health :");
			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();
			CurrentItem.Image = RenderIcon("Image :", CurrentItem.Image);
			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();
		}
	}
}
