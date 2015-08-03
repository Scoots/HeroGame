using Common.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Scripts
{
	/// <summary>
	/// Abstract class for any list data editor
	/// </summary>
	/// <typeparam name="T">The type of objects in the list that you will be modifying</typeparam>
	public abstract class BaseDataEditor<T> : EditorWindow
		where T : BaseData
	{
		/// <summary>
		/// Allows us to get a random number.  Used for IDs
		/// </summary>
		protected static readonly System.Random GetRandom = new System.Random();

		/// <summary>
		/// The list of data that we are displaying.  Each object is a type
		/// </summary>
		private List<T> _dataList = new List<T>();

		/// <summary>
		/// TODO: Add a comment detailing what this is
		/// </summary>
		private Vector2 _listViewScrollPosition = Vector2.zero;

		/// <summary>
		/// TODO: Add a comment detailing what this is
		/// </summary>
		private Vector2 _contentScrollPosition = Vector2.zero;

		/// <summary>
		/// Gets or sets the current index of the list we are looking at
		/// </summary>
		protected int CurrentIndex { get; set; }

		/// <summary>
		/// Gets or sets the _dataList member
		/// </summary>
		protected List<T> DataList
		{
			get { return _dataList; }
			set { _dataList = value; }
		}

		/// <summary>
		/// Gets or sets the _listViewScrollPosition member
		/// </summary>
		protected Vector2 ListViewScrollPosition
		{
			get { return _listViewScrollPosition; }
			set { _listViewScrollPosition = value; }
		}

		/// <summary>
		/// Gets or sets the _contentScrollPosition member
		/// </summary>
		protected Vector2 ContentScrollPosition
		{
			get { return _contentScrollPosition; }
			set { _contentScrollPosition = value; }
		}

		/// <summary>
		/// Gets the current item in the list that we are modifying
		/// </summary>
		protected T CurrentItem
		{
			get { return DataList[CurrentIndex]; }
		}

		/// <summary>
		/// Gets the data type name - should be similar to CardData or GameAbilityData
		/// </summary>
		protected abstract string DataTypeName { get; }

		/// <summary>
		/// Gets the single name of the data - should be similar to Card or Ability
		/// </summary>
		protected abstract string SingleDataName { get; }

		/// <summary>
		/// Function required to populate the list with a first item if no data is present in the file
		/// </summary>
		public virtual void OnEnable()
		{
			LoadDataList();
			if (DataList.Count == 0)
			{
				AddNewItem();
			}
		}

		/// <summary>
		/// Unity function that gets called every tick
		/// </summary>
		public virtual void OnGUI()
		{
			EditorGUILayout.BeginHorizontal();
			DrawListView();
			GUILayout.Space(6f);
			EditorGUILayout.BeginVertical();

			GUILayout.Label(DataTypeName + " Editor - " + CurrentItem.Name, EditorStyles.boldLabel);
			EditorGUIUtility.LookLikeControls(80f);

			GUILayout.BeginHorizontal();
			EditorGUILayout.IntField("Current " + SingleDataName, CurrentIndex);
			EditorGUILayout.LabelField("of " + DataList.Count, "");
			GUILayout.EndHorizontal();

			if (CurrentIndex > DataList.Count)
			{
				CurrentIndex = DataList.Count;
			}

			if (CurrentIndex < 0)
			{
				CurrentIndex = 0;
			}

			GUILayout.Space(4f);
			DrawControls();

			ContentScrollPosition = EditorGUILayout.BeginScrollView(ContentScrollPosition);

			GUILayout.Space(6f);
			DrawActorInfo();
			GUILayout.Space(6f);

			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}

		/// <summary>
		/// Abstract function for getting the T object XML data
		/// </summary>
		/// <returns>The XML data that represents the list we are editing</returns>
		protected abstract string GetXMLFileLocation();

		/// <summary>
		/// Abstract function for drawing the actor info - needed in all editor classes
		/// </summary>
		protected abstract void DrawActorInfo();

		/// <summary>
		/// Gets and loads all the data from our xml file
		/// </summary>
		protected void LoadDataList()
		{
			LoadDataList<T>(GetXMLFileLocation(), ref _dataList);
		}

		protected static void LoadDataList<DataType>(string filePath, ref List<DataType>dataList)
		{
			dataList.Clear();

			// Load the resource
			TextAsset t = Resources.Load<TextAsset>(filePath);

			try
			{
				XmlSerializer reader = new XmlSerializer(typeof(List<DataType>));
				StringReader file = new StringReader(t.text);
				dataList = reader.Deserialize(file) as List<DataType>;
				file.Close();
			}
			catch (Exception e)
			{
				Debug.Log(e.Message);
			}
		}

		/// <summary>
		/// Saves all updated data to the original XML file location
		/// </summary>
		protected void SaveAll()
		{
			try
			{
				var resourcePath = Application.dataPath + "/Resources/" + GetXMLFileLocation() + ".xml";

				XmlSerializer ser = new XmlSerializer(typeof(List<T>));
				TextWriter writer = new StreamWriter(resourcePath);
				ser.Serialize(writer, DataList);
				writer.Close();
			}
			catch (Exception e)
			{
				Debug.Log(e.Message);
			}
		}

		/// <summary>
		/// Adds a new item to the list with default starting data
		/// </summary>
		protected virtual void AddNewItem()
		{
			var newData = Activator.CreateInstance<T>();
			newData.Id = GetRandom.Next();

			DataList.Add(newData);
			CurrentIndex = DataList.Count - 1;
		}

		/// <summary>
		/// Deletes the selected item from the data array
		/// </summary>
		/// <param name="dataObject">The object you wish to delete</param>
		protected void DeleteItem(T dataObject)
		{
			// TODO: Fix it so it doesn't delete when it is empty
			// Remove from list
			DataList.Remove(dataObject);

			// If it was the last one, change our current index
			if (CurrentIndex >= DataList.Count)
			{
				CurrentIndex--;
			}

			// If there are no items in the list, add an empty item
			if (CurrentIndex < 0)
			{
				AddNewItem();
				CurrentIndex = 0;
			}
		}

		/// <summary>
		/// Draws the base controls for the Unity GUI
		/// </summary>
		protected void DrawControls()
		{
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Prev"))
			{
				if (CurrentIndex > 0)
				{
					CurrentIndex--;
				}

				GUI.FocusControl("clearFocus");
			}

			if (GUILayout.Button("Next"))
			{
				if (CurrentIndex < DataList.Count - 1)
				{
					CurrentIndex++;
				}

				GUI.FocusControl("clearFocus");
			}

			GUILayout.Space(30);

			if (GUILayout.Button("+ New"))
			{
				GUI.FocusControl("clearFocus");
				AddNewItem();
			}

			GUI.backgroundColor = Color.red;
			if (GUILayout.Button("Delete"))
			{
				GUI.FocusControl("clearFocus");
				DeleteItem(CurrentItem);
			}

			GUI.backgroundColor = Color.green;
			if (GUILayout.Button("Save All"))
			{
				GUI.FocusControl("clearFocus");
				SaveAll();
			}

			GUI.backgroundColor = Color.white;
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
		}

		/// <summary>
		/// Utility function for rendering an icon
		/// </summary>
		/// <param name="iconText">TODO: Fill this out</param>
		/// <param name="currentGUID">TODO: Fill this out</param>
		/// <returns>TODO: Fill this out</returns>
		protected string RenderIcon(string iconText, string currentGUID)
		{
			UnityEngine.Object oldIcon = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(currentGUID), typeof(Texture));
			UnityEngine.Object icon = EditorGUILayout.ObjectField(iconText, oldIcon, typeof(Texture), false);
			return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(icon));
		}

		/// <summary>
		/// Utility function for rendering an object
		/// </summary>
		/// <param name="iconText">TODO: Fill this out</param>
		/// <param name="currentGUID">TODO: Fill this out</param>
		/// <returns>TODO: Fill this out</returns>
		protected string RenderObject(string iconText, string currentGUID)
		{
			UnityEngine.Object oldIcon = AssetDatabase.LoadAssetAtPath(currentGUID, typeof(UnityEngine.GameObject));
			UnityEngine.Object icon = EditorGUILayout.ObjectField(iconText, oldIcon, typeof(UnityEngine.GameObject), false);
			return AssetDatabase.GetAssetPath(icon);
		}

		/// <summary>
		/// Utility function for rendering text
		/// </summary>
		/// <param name="currentText">TODO: Fill this out</param>
		/// <param name="displayName">TODO: Fill this out</param>
		/// <param name="displayHelp">TODO: Fill this out</param>
		/// <returns>TODO: Fill this out</returns>
		protected string RenderText(string currentText, string displayName, string displayHelp)
		{
			return EditorGUILayout.TextField(new GUIContent(displayName, displayHelp), currentText);
		}

		/// <summary>
		/// Utility function for rendering an integer
		/// </summary>
		/// <param name="currentText">TODO: Fill this out</param>
		/// <param name="displayName">TODO: Fill this out</param>
		/// <returns>TODO: Fill this out</returns>
		protected int RenderInt(int currentText, string displayName)
		{
			return EditorGUILayout.IntField(displayName, currentText);
		}

		/// <summary>
		/// Utility function for rendering a float
		/// </summary>
		/// <param name="currentText">TODO: Fill this out</param>
		/// <param name="displayName">TODO: Fill this out</param>
		/// <returns>TODO: Fill this out</returns>
		protected float RenderFloat(float currentText, string displayName)
		{
			return EditorGUILayout.FloatField(displayName, currentText);
		}

		/// <summary>
		/// Utility function for rendering a boolean
		/// </summary>
		/// <param name="currentText">TODO: Fill this out</param>
		/// <param name="displayName">TODO: Fill this out</param>
		/// <returns>TODO: Fill this out</returns>
		protected bool RenderBool(bool currentText, string displayName)
		{
			return EditorGUILayout.Toggle(displayName, currentText);
		}

		/// <summary>
		/// Utility function for rendering text as ReadOnly
		/// </summary>
		/// <typeparam name="TCt">The type of thing you are displaying as readonly</typeparam>
		/// <param name="currentText">TODO: Fill this out</param>
		/// <param name="displayName">TODO: Fill this out</param>
		protected void RenderReadonly<TCt>(TCt currentText, string displayName)
		{
			GUILayout.Label(displayName);
			GUILayout.FlexibleSpace();
			EditorGUILayout.SelectableLabel(currentText.ToString(), GUILayout.Width(260f));
		}

		/// <summary>
		/// Draws the list of data
		/// </summary>
		private void DrawListView()
		{
			ListViewScrollPosition = GUILayout.BeginScrollView(ListViewScrollPosition, GUILayout.MaxWidth(300), GUILayout.MinWidth(300));
			for (int i = 0; i < DataList.Count; i++)
			{
				if (GUILayout.Button(DataList[i].Name))
				{
					CurrentIndex = i;
				}
			}
			GUILayout.EndScrollView();
		}
	}
}
