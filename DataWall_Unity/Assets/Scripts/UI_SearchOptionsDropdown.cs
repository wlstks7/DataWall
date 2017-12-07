using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(Dropdown))]
public class UI_SearchOptionsDropdown : MonoBehaviour
{
    [SerializeField]
    private SearchOption searchOption = SearchOption.FormatType;

    private Dropdown searchDropdown;

    private void Awake()
    {
        this.searchDropdown = GetComponent<Dropdown>();

        this.searchDropdown.ClearOptions();

        switch (this.searchOption)
        {
            case SearchOption.FormatType:
                this.searchDropdown.AddOptions(new List<string>(System.Enum.GetNames(typeof(FormatType))));
                break;
            case SearchOption.ImmigrationType:
                this.searchDropdown.AddOptions(new List<string>(System.Enum.GetNames(typeof(ImmigrationType))));
                break;
            case SearchOption.DataYear:
                this.searchDropdown.AddOptions(new List<string>(System.Enum.GetNames(typeof(DataYear))));
                break;
            default:
                Debug.LogError("An error occurred while updating the search filter dropdowns.");
                break;
        }

        this.searchDropdown.onValueChanged.AddListener(this.UpdateSearchFilters);
    }

    private void UpdateSearchFilters(int dropdownIndex)
    {
        switch (this.searchOption)
        {
            case SearchOption.FormatType:
                Data_SearchOptions.CurrentFilter_FormatType =
                    (FormatType)System.Enum.Parse(typeof(FormatType), this.searchDropdown.options[dropdownIndex].text);
                break;
            case SearchOption.ImmigrationType:
                Data_SearchOptions.CurrentFilter_ImmigrationType =
                    (ImmigrationType)System.Enum.Parse(typeof(ImmigrationType), this.searchDropdown.options[dropdownIndex].text);
                break;
            case SearchOption.DataYear:
                Data_SearchOptions.CurrentFilter_DataYear =
                    (DataYear)System.Enum.Parse(typeof(DataYear), this.searchDropdown.options[dropdownIndex].text);
                break;
            default:
                Debug.LogError("An error occurred while updating the search filter dropdowns.");
                break;
        }

        DataParser.Parser.StopCoroutine("ParseFileData");
        DataParser.Parser.StartCoroutine(DataParser.Parser.ParseFileData(
            Data_SearchOptions.CurrentFilter_FormatType,
            Data_SearchOptions.CurrentFilter_ImmigrationType,
            Data_SearchOptions.CurrentFilter_DataYear));
    }
}
