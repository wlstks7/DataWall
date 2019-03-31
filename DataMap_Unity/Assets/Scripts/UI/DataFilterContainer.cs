using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using TMPro;

public class DataFilterContainer : MonoBehaviour
{
    [SerializeField]
    private FilterType m_FilterType = FilterType.Format;
    [SerializeField]
    private TextMeshProUGUI m_FilterLabelText;
    [SerializeField]
    private Dropdown m_FilterDropdown;

    private void Awake()
    {
        if (this.m_FilterDropdown == null)
        {
            Debug.LogWarning("Filter Dropdown component not set! Disabling filter container.");
            this.gameObject.SetActive(false);
        }
        else if (this.m_FilterLabelText == null)
        {
            Debug.LogWarning("Filter Text component not set! Disabling filter container.");
            this.gameObject.SetActive(false);
        }

        this.PopulateDropdown();
        this.UpdateSearchFilters(this.m_FilterDropdown.value);

        this.m_FilterDropdown.onValueChanged.AddListener(this.UpdateSearchFilters);

        FilterFlags.ApplyFilters();
    }

    private void OnEnable()
    {
        switch (this.m_FilterType)
        {
            case FilterType.Format:
                this.m_FilterDropdown.value = FilterFlags.CurrentFilters.Filter_Format.ToIndex();
                break;
            case FilterType.Legality:
                this.m_FilterDropdown.value = FilterFlags.CurrentFilters.Filter_Legality.ToIndex();
                break;
            default:
                break;
        }
    }

    private void Reset()
    {
        PopulateDropdown();
    }

    private void PopulateDropdown()
    {
        this.m_FilterDropdown.ClearOptions();

        switch (this.m_FilterType)
        {
            case FilterType.Format:
                this.m_FilterLabelText.text = "Format";
                this.m_FilterDropdown.AddOptions(new List<string>(System.Enum.GetNames(typeof(FormatType))));
                break;
            case FilterType.Legality:
                this.m_FilterLabelText.text = "Legality";
                this.m_FilterDropdown.AddOptions(new List<string>(System.Enum.GetNames(typeof(LegalityType))));
                break;
            //case FilterType.Year:
            //	this.m_FilterLabelText.text = "Year";
            //	var infoList = new List<string>();
            //	foreach (DataYear element in (DataYear[])System.Enum.GetValues(typeof(DataYear)))
            //	{
            //		infoList.Add(element.ToInt().ToString());
            //	}
            //	this.m_FilterDropdown.AddOptions(infoList);
            //	break;
            default:
                Debug.LogError("An error occurred while updating the search filter dropdowns.");
                break;
        }

        this.m_FilterDropdown.RefreshShownValue();
    }

    private void UpdateSearchFilters(int dropdownIndex)
    {
        switch (this.m_FilterType)
        {
            case FilterType.Format:
                FilterFlags.UpdateFilter(dropdownIndex.IndexToFormatType());
                break;
            case FilterType.Legality:
                FilterFlags.UpdateFilter(dropdownIndex.IndexToLegalityType());
                break;
            // TODO: Change to bitmasking/simulated enum flag field
            //case FilterType.Year:
            //	Data_FilterFlags.Filter_Year = dropdownIndex.IndexToDataYear();
            //	break;
            default:
                Debug.LogError("An error occurred while updating the search filter dropdowns.");
                break;
        }
    }
}
