namespace Enums
{
	[System.Flags]
	public enum FilterType : byte
	{
		Format = 1 << 0,
		Legality = 1 << 1,
		Year = 1 << 2
	}

	public static partial class CustomExtensions
	{
		public static FilterType Add(this FilterType currentFilter, FilterType newFilter)
		{
			return currentFilter | newFilter;
		}

		public static FilterType Remove(this FilterType currentFilter, FilterType newFilter)
		{
			return currentFilter ^ newFilter;
		}

		public static bool Contains(this FilterType currentFilter, FilterType newFilter)
		{
			return (currentFilter & newFilter) == newFilter;
		}
	}
}
