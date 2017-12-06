using UnityEngine;

public class StateInfoDatabase : MonoBehaviour
{
	[System.Serializable]
	public struct StateInfo
	{
		public StateName stateName;
		public YearlyData[] timelineData;
	}

	[System.Serializable]
	public struct RuntimeStats
	{
		public int popRelativePercent;
	}

	[System.Serializable]
	public struct YearlyData
	{
		public DataYear year;
		//TODO: Automatic data retrieval
		[Header("Manually-Entered Stats")]
		public int popNetNominal;
		[Header("Runtime-Calculated Stats")]
		public RuntimeStats runtimeStats;
		//TODO: more stats
	}

	public StateInfo[] stateList;
	public YearlyData[] countryTotals;

	private void Start()
	{
		this.CalculateSubStats();
	}

	[ContextMenu("Populate Presets")]
	public void PopulatePresets()
	{
		//Initialize stateList with size = StateName enum names length
		this.stateList = new StateInfo[System.Enum.GetNames(typeof(StateName)).Length];

		//For each state via StateName enum
		for (int i = 0; i < System.Enum.GetNames(typeof(StateName)).Length; i++)
		{
			//Populate name with current StateName enum at index = i
			this.stateList[i].stateName = (StateName)System.Enum
				.Parse(typeof(StateName), System.Enum.GetNames(typeof(StateName))[i]);

			//Initialize timelineData with size = DataYear enum names length
			this.stateList[i].timelineData = new YearlyData[System.Enum.GetNames(typeof(DataYear)).Length];

			//For each data year via DataYear enum
			for (int j = 0; j < System.Enum.GetNames(typeof(DataYear)).Length; j++)
			{
				//Populate year with current DataYear enum at index = j 
				this.stateList[i].timelineData[j].year = (DataYear)System.Enum
					.Parse(typeof(DataYear), System.Enum.GetNames(typeof(DataYear))[j]);
			}
		}

		//Initialize contryTotals with size = DataYear enum names length
		this.countryTotals = new YearlyData[System.Enum.GetNames(typeof(DataYear)).Length];

		for (int i = 0; i < System.Enum.GetNames(typeof(DataYear)).Length; i++)
		{
			//
			this.countryTotals[i].year = (DataYear)System.Enum
				.Parse(typeof(DataYear), System.Enum.GetNames(typeof(DataYear))[i]);
			//
			this.countryTotals[i].runtimeStats.popRelativePercent = 100;
		}
	}

	[ContextMenu("Calculate Sub-Statistics")]
	private void CalculateSubStats()
	{
		if (this.countryTotals[0].popNetNominal == 0)
		{
			Debug.LogError("Nominal Population for first state equals zero, no sub-stats to calculate.");
			return;
		}

		foreach (var state in this.stateList)
		{
			for (int i = 0; i < state.timelineData.Length; i++)
			{
				//
				state.timelineData[i].runtimeStats.popRelativePercent =
					state.timelineData[i].popNetNominal / this.countryTotals[i].popNetNominal;
			}
		}
	}
}
