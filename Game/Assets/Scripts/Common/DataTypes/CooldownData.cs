namespace Common.DataTypes
{
	using Common.Data;

	public class CooldownData
	{
		public Enumerations.OrbColor Color { get; set; }

		private int _count;
		public int Count 
		{
			get { return this._count; }
			set
			{
				this._count = value;
				if (this._count < 0)
				{
					this._count = 0;
				}
			}
		}

		public CooldownData(Enumerations.OrbColor color, int count)
		{
			this.Color = color;
			this.Count = count;
		}
	}
}
