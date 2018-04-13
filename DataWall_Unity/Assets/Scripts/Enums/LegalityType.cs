namespace Enums
{
	[System.Flags]
	public enum LegalityType :byte
	{
		Unauthorized,
		Lawful
	}

	public static partial class CustomExtensions
	{
		public static LegalityType IndexToLegalityType(this int year)
		{
			switch (year)
			{
				case 0:
					return LegalityType.Unauthorized;
				case 1:
					return LegalityType.Lawful;
				default:
					UnityEngine.Debug.LogError("IndexToLegalityType() has failed.");
					return 0;
			}
		}
	}
}
