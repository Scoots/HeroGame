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
	class PassiveAbilityDataEditor : BaseDataEditor<PassiveAbilityData>
	{
		/// <summary>
		/// Gets the data type name for use in the base class functionality
		/// </summary>
		protected override string DataTypeName
		{
			get
			{
				return "PassiveAbilityData";
			}
		}

		/// <summary>
		/// Gets the single data name for use in the base class functionality
		/// </summary>
		protected override string SingleDataName
		{
			get
			{
				return "Passive Ability";
			}
		}

		/// <summary>
		/// Initializes the ability data editor window
		/// </summary>
		[MenuItem("Hero/PassiveAbilityData Editor")]
		public static void Init()
		{
			PassiveAbilityDataEditor editor = GetWindow<PassiveAbilityDataEditor>();
			editor.minSize = new Vector2(300, 300);
			editor.Show();
		}

		/// <summary>
		/// Determines what file we are going to be reading/writing
		/// </summary>
		/// <returns>The ability data file location</returns>
		protected override string GetXMLFileLocation()
		{
			return "Data/PassiveAbilityData";
		}

		/// <summary>
		/// Draws the contents of the passive ability data editor for each single ability
		/// </summary>
		protected override void DrawActorInfo()
		{
			GUILayout.BeginHorizontal();
			RenderReadonly(CurrentItem.Id, "Id :");
			GUILayout.EndHorizontal();

			CurrentItem.Name = RenderText(CurrentItem.Name, "Name :", "Display the name of the passive");
			CurrentItem.Description = RenderText(
					CurrentItem.Description, "Description :",
					"A brief description of what the passive does.");
			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();

			CurrentItem.TypeOfPassiveAbility = (PassiveAbilityData.PassiveAbilityType)EditorGUILayout.EnumPopup("Ability Type :", CurrentItem.TypeOfPassiveAbility);

			if (CurrentItem.TypeOfPassiveAbility != PassiveAbilityData.PassiveAbilityType.PermChangeColor)
			{
				CurrentItem.PassiveTarget = (PassiveAbilityData.ModificationTarget)EditorGUILayout.EnumPopup("Target Type :", CurrentItem.PassiveTarget);
			}
			
			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();

			if (CurrentItem.TypeOfPassiveAbility == PassiveAbilityData.PassiveAbilityType.ModifyMana)
			{
				CurrentItem.ManaModifier = RenderFloat(CurrentItem.ManaModifier, "Mana Modifier :");
			}

			if (CurrentItem.TypeOfPassiveAbility == PassiveAbilityData.PassiveAbilityType.ChangeTime)
			{
				CurrentItem.TimeDelta = RenderFloat(CurrentItem.TimeDelta, "Time Modification :");
			}

			// Changes all NEW orbs of the first type into the second type
			if (CurrentItem.TypeOfPassiveAbility == PassiveAbilityData.PassiveAbilityType.PermChangeColor)
			{
				CurrentItem.TargetColor = (Enumerations.OrbColor)EditorGUILayout.EnumPopup("Color (1st) :", CurrentItem.TargetColor);
				CurrentItem.SelectColor = (Enumerations.OrbColor)EditorGUILayout.EnumPopup("Color (2nd) :", CurrentItem.SelectColor);
			}

			if (CurrentItem.TypeOfPassiveAbility == PassiveAbilityData.PassiveAbilityType.BuffDamage)
			{
				CurrentItem.DamageBuff = RenderInt(CurrentItem.DamageBuff, "Damage Buff :");
				GUILayout.BeginHorizontal();
				GUILayout.EndHorizontal();
			}

			if (CurrentItem.TypeOfPassiveAbility == PassiveAbilityData.PassiveAbilityType.BuffHealth)
			{
				CurrentItem.HealthBuff = RenderInt(CurrentItem.HealthBuff, "Health Buff :");
				GUILayout.BeginHorizontal();
				GUILayout.EndHorizontal();
			}

			if (CurrentItem.TypeOfPassiveAbility == PassiveAbilityData.PassiveAbilityType.BuffDamageAndHealth)
			{
				CurrentItem.DamageBuff = RenderInt(CurrentItem.DamageBuff, "Damage Buff :");
				CurrentItem.HealthBuff = RenderInt(CurrentItem.HealthBuff, "Health Buff :");
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
