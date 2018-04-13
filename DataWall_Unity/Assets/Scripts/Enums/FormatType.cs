namespace Enums
{
	//TODO: add more formats

	[System.Flags]
	public enum FormatType : byte
	{
		Population = 1 << 0
	}

	public static partial class CustomExtensions
	{
		public static FormatType IndexToFormatType(this int year)
		{
			switch (year)
			{
				case 0:
					return FormatType.Population;
				default:
					UnityEngine.Debug.LogError("IndexToLegalityType() has failed.");
					return 0;
			}
		}
	}
}
