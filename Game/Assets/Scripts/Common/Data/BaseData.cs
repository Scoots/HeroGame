namespace Common.Data
{
	using System;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.IO;

	/// <summary>
	/// Contains the base information for any data the game has
	/// </summary>
	[Serializable]
	public class BaseData
	{
		/// <summary>
		/// Gets or sets the Id of the game data
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the data
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets the Id of the data and returns it
		/// </summary>
		/// <returns>The Id of the data</returns>
		public int GetID()
		{
			return this.Id;
		}

		/// <summary>
		/// Gets the name of the data and returns it
		/// </summary>
		/// <returns>The name of the data</returns>
		public string InternalName()
		{
			return this.Name;
		}

		/// <summary>
		/// Base function for initializing the editors
		/// </summary>
		public virtual void Initialize()
		{
		}

		public static T DeepClone<T>(T obj)
		{
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, obj);
				ms.Position = 0;

				return (T)formatter.Deserialize(ms);
			}
		}
	}
}
