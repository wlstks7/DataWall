namespace Enums
{
	//TODO: add more formats

	[System.Flags]
	public enum FormatType : byte
	{
		Total = 1 << 0
	}

	public static partial class CustomExtensions
	{
		public static FormatType IndexToFormatType(this int year)
		{
			switch (year)
			{
				case 0:
					return FormatType.Total;
				default:
					UnityEngine.Debug.LogError("IndexToLegalityType() has failed.");
					return 0;
			}
		}

		public static int ToIndex(this FormatType filterType)
		{
			switch (filterType)
			{
				case FormatType.Total:
					return 0;
				default:
					throw new System.ArgumentOutOfRangeException();
			}
		}
	}
}
