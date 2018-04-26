namespace Enums
{
	[System.Flags]
	public enum DataYear
	{
		Five = 1 << 0,
		Six = 1 << 1,
		Seven = 1 << 2,
		Eight = 1 << 3,
		Nine = 1 << 4,
		Ten = 1 << 5,
		Eleven = 1 << 6,
		Twelve = 1 << 7
	}

	public static partial class CustomExtensions
	{
		public static int ToInt(this DataYear dataYear)
		{
			switch (dataYear)
			{
				case DataYear.Five:
					return 2005;
				case DataYear.Six:
					return 2006;
				case DataYear.Seven:
					return 2007;
				case DataYear.Eight:
					return 2008;
				case DataYear.Nine:
					return 2009;
				case DataYear.Ten:
					return 2010;
				case DataYear.Eleven:
					return 2011;
				case DataYear.Twelve:
					return 2012;
				default:
					throw new System.ArgumentOutOfRangeException();
			}
		}

		public static DataYear IndexToDataYear(this int year)
		{
			switch (year)
			{
				case 0:
					return DataYear.Five;
				case 1:
					return DataYear.Six;
				case 2:
					return DataYear.Seven;
				case 3:
					return DataYear.Eight;
				case 4:
					return DataYear.Nine;
				case 5:
					return DataYear.Ten;
				case 6:
					return DataYear.Eleven;
				case 7:
					return DataYear.Twelve;
				default:
					UnityEngine.Debug.LogError("IndexToDataYear() has failed.");
					return 0;
			}
		}
	}
}
