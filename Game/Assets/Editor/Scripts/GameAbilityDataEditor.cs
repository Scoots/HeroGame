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
	class GameAbilityDataEditor : BaseDataEditor<GameAbilityData>
	{
		/// <summary>
		/// Gets the data type name for use in the base class functionality
		/// </summary>
		protected override string DataTypeName
		{
			get
			{
				return "GameAbilityData";
			}
		}

		/// <summary>
		/// Gets the single data name for use in the base class functionality
		/// </summary>
		protected override string SingleDataName
		{
			get
			{
				return "Game Ability";
			}
		}

		/// <summary>
		/// Initializes the ability data editor window
		/// </summary>
		[MenuItem("Hero/GameAbilityData Editor")]
		public static void Init()
		{
			GameAbilityDataEditor editor = GetWindow<GameAbilityDataEditor>();
			editor.minSize = new Vector2(300, 300);
			editor.Show();
		}

		/// <summary>
		/// Determines what file we are going to be reading/writing
		/// </summary>
		/// <returns>The ability data file location</returns>
		protected override string GetXMLFileLocation()
		{
			return "Data/GameAbilityData";
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

			CurrentItem.TypeOfAbility = (GameAbilityData.AbilityType)EditorGUILayout.EnumPopup("Ability Type :", CurrentItem.TypeOfAbility);
			CurrentItem.Target = (GameAbilityData.TargetType)EditorGUILayout.EnumPopup("Target Type :", CurrentItem.Target);
			CurrentItem.Activate = (BaseAbilityData.ActivationType)EditorGUILayout.EnumPopup("Activate Type :", CurrentItem.Activate);
			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();

			if (CurrentItem.TypeOfAbility == GameAbilityData.AbilityType.Damage)
			{
				CurrentItem.DamageOrHealing = RenderInt(CurrentItem.DamageOrHealing, "Damage :");
				GUILayout.BeginHorizontal();
				GUILayout.EndHorizontal();
			}

			if (CurrentItem.TypeOfAbility == GameAbilityData.AbilityType.Healing)
			{
				CurrentItem.DamageOrHealing = RenderInt(CurrentItem.DamageOrHealing, "Healing :");
				GUILayout.BeginHorizontal();
				GUILayout.EndHorizontal();
			}

			if (CurrentItem.TypeOfAbility == GameAbilityData.AbilityType.BuffDamage)
			{
				CurrentItem.DamageBuff = RenderInt(CurrentItem.DamageBuff, "Damage Buff :");
				GUILayout.BeginHorizontal();
				GUILayout.EndHorizontal();
			}

			if (CurrentItem.TypeOfAbility == GameAbilityData.AbilityType.BuffHealth)
			{
				CurrentItem.HealthBuff = RenderInt(CurrentItem.HealthBuff, "Health Buff :");
				GUILayout.BeginHorizontal();
				GUILayout.EndHorizontal();
			}

			if (CurrentItem.TypeOfAbility == GameAbilityData.AbilityType.BuffDamageAndHealth)
			{
				CurrentItem.DamageBuff = RenderInt(CurrentItem.DamageBuff, "Damage Buff :");
				CurrentItem.HealthBuff = RenderInt(CurrentItem.HealthBuff, "Health Buff :");
				GUILayout.BeginHorizontal();
				GUILayout.EndHorizontal();
			}

			if (CurrentItem.TypeOfAbility == GameAbilityData.AbilityType.AbilityRemoval)
			{

				CurrentItem.TypeToRemove = (GameAbilityData.AbilityRemovalType)EditorGUILayout.EnumPopup("Removal Type :", CurrentItem.TypeToRemove);
				GUILayout.BeginHorizontal();
				GUILayout.EndHorizontal();
			}

			if (CurrentItem.TypeOfAbility == GameAbilityData.AbilityType.AbilityGrant)
			{
				CurrentItem.AbilityToGrant = RenderInt(CurrentItem.AbilityToGrant, "Ability to Add :");
				GUILayout.BeginHorizontal();
				GUILayout.EndHorizontal();
			}

			CurrentItem.CanBeRemoved = RenderBool(CurrentItem.CanBeRemoved, "Can Be Removed :");
			CurrentItem.Image = RenderIcon("Image :", CurrentItem.Image);
			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();
		}
	}
}
