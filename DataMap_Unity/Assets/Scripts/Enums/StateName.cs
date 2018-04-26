namespace Enums
{
	[System.Flags]
	public enum StateName
	{
		Arizona = 1 << 0,
		California = 1 << 1,
		Florida = 1 << 2,
		Georgia = 1 << 3,
		Illinois = 1 << 4,
		NewJersey = 1 << 5,
		NewYork = 1 << 6,
		NorthCarolina = 1 << 7,
		Texas = 1 << 8,
		Washington = 1 << 9
	}

	public static partial class CustomExtensions
	{
		public static int ToIndex(this StateName stateName)
		{
			switch (stateName)
			{
				case StateName.Arizona:
					return 0;
				case StateName.California:
					return 1;
				case StateName.Florida:
					return 2;
				case StateName.Georgia:
					return 3;
				case StateName.Illinois:
					return 4;
				case StateName.NewJersey:
					return 5;
				case StateName.NewYork:
					return 6;
				case StateName.NorthCarolina:
					return 7;
				case StateName.Texas:
					return 8;
				case StateName.Washington:
					return 9;
				default:
					throw new System.ArgumentOutOfRangeException();
			}
		}
	}
}
