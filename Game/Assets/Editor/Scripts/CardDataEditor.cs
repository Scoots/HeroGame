using System.CodeDom.Compiler;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Common.Data;
using Common.DataTypes;

namespace Assets.Editor.Scripts
{
	/// <summary>
	/// Editor that allows you to create and update card data
	/// </summary>
	class CardDataEditor : BaseDataEditor<CardData>
	{
		protected override string DataTypeName
		{
			get
			{
				return "CardData";
			}
		}

		protected override string SingleDataName
		{
			get
			{
				return "Card";
			}
		}

		private static List<BoardAbilityData> _boardAbilityData = new List<BoardAbilityData>();

		private static List<GameAbilityData> _gameAbilityData = new List<GameAbilityData>();

		private static List<PassiveAbilityData> _passiveAbilityData = new List<PassiveAbilityData>();

		/// <summary>
		/// Initializes the card data editor window
		/// </summary>
		[MenuItem("Hero/CardData Editor")]
		public static void Init()
		{
			CardDataEditor editor = GetWindow<CardDataEditor>();
			editor.minSize = new Vector2(300, 300);
			editor.Show();

			LoadDataList<BoardAbilityData>("Data/BoardAbilityData", ref _boardAbilityData);
			LoadDataList<GameAbilityData>("Data/GameAbilityData", ref _gameAbilityData);
			LoadDataList<PassiveAbilityData>("Data/PassiveAbilityData", ref _passiveAbilityData);
		}

		/// <summary>
		/// Determines what file we are going to be reading/writing
		/// </summary>
		/// <returns>The card data file location</returns>
		protected override string GetXMLFileLocation()
		{
			return "Data/CardData";
		}

		/// <summary>
		/// Draws the contents of the card data editor for each single card
		/// </summary>
		protected override void DrawActorInfo()
		{
			GUILayout.BeginHorizontal();
			RenderReadonly(CurrentItem.Id, "Id :");
			GUILayout.EndHorizontal();

			CurrentItem.Name = RenderText(CurrentItem.Name, "Name :", "Display the name of the card");
			CurrentItem.Description = RenderText(
					CurrentItem.Description, "Description :",
					"A brief description of what the card does.");
			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();

			CurrentItem.AttackAbility = RenderInt(CurrentItem.AttackAbility, "Attack Ability :");
			foreach (GameAbilityData g in _gameAbilityData)
			{
				if (this.CurrentItem.AttackAbility == g.Id)
				{
					this.RenderReadonly(g.Name, string.Empty);
					break;
				}
			}

			CurrentItem.BoardAbility = RenderInt(CurrentItem.BoardAbility, "Board Ability :");
			foreach (BoardAbilityData b in _boardAbilityData)
			{
				if (this.CurrentItem.BoardAbility == b.Id)
				{
					this.RenderReadonly(b.Name, string.Empty);
					break;
				}
			}
			
			CurrentItem.Health = RenderInt(CurrentItem.Health, "Health :");
			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();

			//CurrentItem.Image = RenderIcon("Image :", CurrentItem.Image);
			//CurrentItem.ThumbImage = RenderIcon("Thumbnail Image :", CurrentItem.ThumbImage);
			CurrentItem.Image = RenderText(CurrentItem.Image, "Image :", "Sprite Name");
			CurrentItem.ThumbImage = RenderText(CurrentItem.ThumbImage, "Thumb Image :", "Thumbnail Name");
			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();

			GUILayout.Space(6f);
			DrawArrays();
		}

		/// <summary>
		/// Draws the list of behavior data that is associated with this ability
		/// </summary>
		private void DrawArrays()
		{
			List<object> toDelete = new List<object>();
            List<object> toDeleteSecond = new List<object>();

            {
                EditorGUILayout.LabelField("Cooldown Data", GUILayout.Width(65));
                for (int i = 0; i < this.CurrentItem.CooldownColorData.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    this.CurrentItem.CooldownColorData[i] = (Enumerations.OrbColor)EditorGUILayout.EnumPopup("Cooldown Color :", CurrentItem.CooldownColorData[i]);
                    this.CurrentItem.CooldownCountList[i] = this.RenderInt(CurrentItem.CooldownCountList[i], "Cooldown :");

                    GUI.color = Color.white;

                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("-"))
                    {
                        toDelete.Add((object)this.CurrentItem.CooldownColorData[i]);
                        toDeleteSecond.Add((object)this.CurrentItem.CooldownCountList[i]);
                    }
                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.EndHorizontal();
                }

                if (toDelete.Count > 0)
                {
                    foreach (var dat in toDelete)
                    {
                        this.CurrentItem.CooldownColorData.Remove((Enumerations.OrbColor)dat);
                    }
                    foreach (var dat in toDeleteSecond)
                    {
                        this.CurrentItem.CooldownCountList.Remove((int)dat);
                    }
                }

                if (GUILayout.Button("Add Cooldown Data"))
                {
                    this.CurrentItem.CooldownCountList.Add(0);
                    this.CurrentItem.CooldownColorData.Add(Enumerations.OrbColor.Dark);
                }

            }
            toDelete.Clear();

            {
                EditorGUILayout.LabelField("Game Abilities", GUILayout.Width(65));
                for (int i = 0; i < this.CurrentItem.GameAbilityList.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    this.CurrentItem.GameAbilityList[i] = EditorGUILayout.IntField(this.CurrentItem.GameAbilityList[i]);

                    foreach (GameAbilityData g in _gameAbilityData)
                    {
                        if (this.CurrentItem.GameAbilityList[i] == g.Id)
                        {
                            this.RenderReadonly(g.Name, string.Empty);
                            break;
                        }
                    }
                    GUI.color = Color.white;

                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("-"))
                    {
                        toDelete.Add((object)this.CurrentItem.GameAbilityList[i]);
                    }
                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.EndHorizontal();
                }

                if (toDelete.Count > 0)
                {
                    foreach (var dat in toDelete)
                    {
                        this.CurrentItem.GameAbilityList.Remove((int)dat);
                    }
                }

                if (GUILayout.Button("Add Game Abilities"))
                {
                    this.CurrentItem.GameAbilityList.Add(default(int));
                }

            }
            toDelete.Clear();

			{
				EditorGUILayout.LabelField("Passive Abilities", GUILayout.Width(65));
				for (int i = 0; i < this.CurrentItem.PassiveAbilityList.Count; i++)
				{
					GUILayout.BeginHorizontal();
					this.CurrentItem.PassiveAbilityList[i] = EditorGUILayout.IntField(this.CurrentItem.PassiveAbilityList[i]);

					foreach (PassiveAbilityData b in _passiveAbilityData)
					{
						if (this.CurrentItem.PassiveAbilityList[i] == b.Id)
						{
							this.RenderReadonly(b.Name, string.Empty);
							break;
						}
					}
					GUI.color = Color.white;

					GUI.backgroundColor = Color.red;
					if (GUILayout.Button("-"))
					{
						toDelete.Add((object)this.CurrentItem.PassiveAbilityList[i]);
					}
					GUI.backgroundColor = Color.white;

					EditorGUILayout.EndHorizontal();
				}

				if (toDelete.Count > 0)
				{
					foreach (var dat in toDelete)
					{
						this.CurrentItem.PassiveAbilityList.Remove((int)dat);
					}
				}

				if (GUILayout.Button("Add Passive Abilities"))
				{
					this.CurrentItem.PassiveAbilityList.Add(default(int));
				}

			}
			toDelete.Clear();

		}
	}
}
