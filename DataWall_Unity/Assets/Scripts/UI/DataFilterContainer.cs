using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class DataFilterContainer : MonoBehaviour
{
	public Text FilterLabelText { get { return this.m_FilterLabelText; } }
	public Dropdown FilterDropdown { get { return this.m_FilterDropdown; } }

	[SerializeField]
	private FilterType m_InitialFilterType = FilterType.Format;
	[SerializeField]
	private Text m_FilterLabelText;
	[SerializeField]
	private Dropdown m_FilterDropdown;

	private void Awake()
	{
		this.m_FilterDropdown.ClearOptions();

		switch (this.m_InitialFilterType)
		{
			case FilterType.Format:
				this.m_FilterLabelText.text = "Format";
				this.m_FilterDropdown.AddOptions(new List<string>(System.Enum.GetNames(typeof(FormatType))));
				break;
			case FilterType.Legality:
				this.m_FilterLabelText.text = "Legality";
				this.m_FilterDropdown.AddOptions(new List<string>(System.Enum.GetNames(typeof(LegalityType))));
				break;
			case FilterType.Year:
				this.m_FilterLabelText.text = "Year";

				var infoList = new List<string>();

				foreach (DataYear element in (DataYear[])System.Enum.GetValues(typeof(DataYear)))
				{
					infoList.Add(element.ToInt().ToString());
				}

				this.m_FilterDropdown.AddOptions(infoList);
				break;
			default:
				Debug.LogError("An error occurred while updating the search filter dropdowns.");
				break;
		}

		this.m_FilterDropdown.onValueChanged.AddListener(this.UpdateSearchFilters);

		this.UpdateSearchFilters(this.m_FilterDropdown.value);
	}

	private void UpdateSearchFilters(int dropdownIndex)
	{
		// TODO: Change to bitmasking/simulated enum flag field
		switch (this.m_InitialFilterType)
		{
			case FilterType.Format:
				Data_FilterFlags.Filter_Format = dropdownIndex.IndexToFormatType();
				break;
			case FilterType.Legality:
				Data_FilterFlags.Filter_Legality = dropdownIndex.IndexToLegalityType();
				break;
			case FilterType.Year:
				Data_FilterFlags.Filter_Year = dropdownIndex.IndexToDataYear();
				break;
			default:
				Debug.LogError("An error occurred while updating the search filter dropdowns.");
				break;
		}

		return;
	}
}
