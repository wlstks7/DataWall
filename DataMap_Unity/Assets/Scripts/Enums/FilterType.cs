namespace Enums
{
	[System.Flags]
	public enum FilterType : byte
	{
		Format = 1 << 0,
		Legality = 1 << 1,
	}
}
