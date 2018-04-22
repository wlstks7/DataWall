using Enums;

public static class FilterFlags
{
	public struct FilterGroup
	{
		public StateName Filter_State;
		public FormatType Filter_Format;
		public LegalityType Filter_Legality;
		public DataYear Filter_Year;
	}

	public static bool IsDirty
	{
		get
		{
			return CurrentFilters.Filter_State != TempFilters.Filter_State
				|| CurrentFilters.Filter_Format != TempFilters.Filter_Format
				|| CurrentFilters.Filter_Legality != TempFilters.Filter_Legality
				|| CurrentFilters.Filter_Year != TempFilters.Filter_Year;
		}
	}

	

	public static FilterGroup CurrentFilters { get; private set; }

	private static FilterGroup TempFilters;

	public static void ApplyFilters()
	{
		CurrentFilters = TempFilters;
	}

	public static void UpdateFilter(StateName state)
	{
		TempFilters.Filter_State = state;
	}

	public static void UpdateFilter(FormatType format)
	{
		TempFilters.Filter_Format = format;
	}

	public static void UpdateFilter(LegalityType legality)
	{
		TempFilters.Filter_Legality = legality;
	}

	public static void UpdateFilter(DataYear year)
	{
		TempFilters.Filter_Year = year;
	}
}
