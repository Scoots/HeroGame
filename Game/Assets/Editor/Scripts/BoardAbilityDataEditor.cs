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
	class BoardAbilityDataEditor : BaseDataEditor<BoardAbilityData>
	{
		/// <summary>
		/// Gets the data type name for use in the base class functionality
		/// </summary>
		protected override string DataTypeName
		{
			get
			{
				return "BoardAbilityData";
			}
		}

		/// <summary>
		/// Gets the single data name for use in the base class functionality
		/// </summary>
		protected override string SingleDataName
		{
			get
			{
				return "Board Ability";
			}
		}

		/// <summary>
		/// Initializes the ability data editor window
		/// </summary>
		[MenuItem("Hero/BoardAbilityData Editor")]
		public static void Init()
		{
			BoardAbilityDataEditor editor = GetWindow<BoardAbilityDataEditor>();
			editor.minSize = new Vector2(300, 300);
			editor.Show();
		}

		/// <summary>
		/// Determines what file we are going to be reading/writing
		/// </summary>
		/// <returns>The ability data file location</returns>
		protected override string GetXMLFileLocation()
		{
			return "Data/BoardAbilityData";
		}

		/// <summary>
		/// Draws the contents of the ability data editor for each single ability
		/// </summary>
		protected override void DrawActorInfo()
		{
			GUILayout.BeginHorizontal();
			RenderReadonly(CurrentItem.Id, "Id :");
			GUILayout.EndHorizontal();

			CurrentItem.Name = RenderText(CurrentItem.Name, "Name :", "Display the name of the ability");
			CurrentItem.Description = RenderText(
					CurrentItem.Description, "Description :",
					"A brief description of what the ability does.");
			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();

			CurrentItem.TypeOfBoardAbility = (BoardAbilityData.BoardAbilityType)EditorGUILayout.EnumPopup("Ability Type :", CurrentItem.TypeOfBoardAbility);
			CurrentItem.Activate = (BaseAbilityData.ActivationType)EditorGUILayout.EnumPopup("Activate Type :", CurrentItem.Activate);
			CurrentItem.BoardTarget = (BoardAbilityData.BoardTargetType)EditorGUILayout.EnumPopup("Target Type :", CurrentItem.BoardTarget);

			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();

			if (CurrentItem.TypeOfBoardAbility == BoardAbilityData.BoardAbilityType.ChangeColor)
			{
				CurrentItem.TargetColor = (Enumerations.OrbColor)EditorGUILayout.EnumPopup("Color (1st) :", CurrentItem.TargetColor);
			}

			if (CurrentItem.BoardTarget == BoardAbilityData.BoardTargetType.Grid)
			{
				CurrentItem.GridSizeX = RenderInt(CurrentItem.GridSizeX, "X Grid Size :");
				CurrentItem.GridSizeY = RenderInt(CurrentItem.GridSizeY, "Y Grid Size :");
			}

			if (CurrentItem.BoardTarget == BoardAbilityData.BoardTargetType.Color)
			{
				CurrentItem.SelectColor = (Enumerations.OrbColor)EditorGUILayout.EnumPopup("Color (2nd) :", CurrentItem.SelectColor);
			}

			if (CurrentItem.BoardTarget == BoardAbilityData.BoardTargetType.Selection)
			{
				CurrentItem.NumOfSelections = RenderInt(CurrentItem.NumOfSelections, "Num Selections :");
			}

			if (CurrentItem.BoardTarget == BoardAbilityData.BoardTargetType.RandomSelection)
			{
				CurrentItem.NumOfSelections = RenderInt(CurrentItem.NumOfSelections, "Num Randoms :");
			}

			CurrentItem.CanBeRemoved = RenderBool(CurrentItem.CanBeRemoved, "Can Be Removed :");
			CurrentItem.Image = RenderIcon("Image :", CurrentItem.Image);
			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();
		}
	}
}
