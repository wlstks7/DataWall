using System.Collections.Generic;
using UnityEngine;
using Networking = UnityEngine.Networking;

public class DataParser : MonoBehaviour
{
    public const string categoryBox_AssetName = "CategoryBox";

    public static DataParser Parser;

    private List<GameObject> categoryBoxList = new List<GameObject>();

    private void Awake()
    {
        Parser = this;
    }

    private void Start()
    {
        StopAllCoroutines();
        Parser.StartCoroutine(Parser.ParseFileData(
            Data_SearchOptions.CurrentFilter_FormatType,
            Data_SearchOptions.CurrentFilter_ImmigrationType,
            Data_SearchOptions.CurrentFilter_DataYear));
    }

    [ContextMenu("Test ParseData()")]
    private void TestParseData()
    {
        FormatType formatType = FormatType.Population;
        ImmigrationType immigrationType = ImmigrationType.Unauthorized;
        DataYear dataYear = DataYear.Five;

        StopAllCoroutines();
        StartCoroutine(ParseFileData(formatType, immigrationType, dataYear));
    }

    public System.Collections.IEnumerator ParseFileData(FormatType formatType, ImmigrationType immigrationType, DataYear dataYear)
    {
        Debug.Log(Time.time + ": PARSE");
        string[] result;

        //TODO: Data Persistence Validation (Eliminate parsing redundancy with query updates)
        //TODO: Multi-category support

        this.UI_ClearCategoryBoxes();

        #region Data Retrieval via File IO

        foreach (string numberFormat in System.Enum.GetNames(typeof(NumberFormat)))
        {
            string filePath = System.IO.Path
                .Combine(Application.streamingAssetsPath, formatType.ToString() + numberFormat + immigrationType.ToString())
                + ".csv";

            Debug.Log(Time.time + ": Filepath = " + filePath);

            if (filePath.Contains("://"))
            {
                var www = Networking.UnityWebRequest.Get(filePath);
                yield return www.SendWebRequest();
                result = www.downloadHandler.text.Split('\n');
            }
            else
            {
                try
                {
                    result = System.IO.File.ReadAllLines(filePath);
                }
                catch (System.IO.FileNotFoundException)
                {
                    Debug.Log(Time.time + ": File could not be found. FilePath = " + filePath);
                    throw;
                }
            }

            Debug.Log(Time.time + ": " + numberFormat + " data successfully retrieved.");

            Debug.Log(Time.time + ": Result length = " + result.Length);

            //TODO: Multi-year parsing

            //Result now equals comma-separated string array - each element equals timeline data for each line (a state)
            // > Sample Element: "1,2222,3,4444,55,666,77,888888,9999"

            for (int i = 0; i < result.Length; i++)
            {
                string[] stateInfo = result[i].Split(',');
                //Debug.Log(Time.time + ": StateInfo data (Timeline) || i = " + i);

                try
                {
                    result[i] = stateInfo[((int)dataYear) - 2005];
                    //Debug.Log(Time.time + ": StateInfo data (" + dataYear.ToString() + ") = \"" + result[i] + "\" || i = " + i);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(Time.time + ": " + e.Message);
                }
            }

            Debug.Log(Time.time + ": Adding UI Elements.");

            this.UI_AddCategoryBox(result, numberFormat);
        }

        #endregion

        Debug.Log(Time.time + ": Force Rebuilding UI Layouts.");

        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());

        yield break;
    }

    private void UI_AddCategoryBox(string[] result, string numberFormat)
    {
        var textBox = Instantiate(Resources.Load(categoryBox_AssetName, typeof(GameObject)), this.transform) as GameObject;

        this.categoryBoxList.Add(textBox.gameObject);

        UI_CategoryBoxInfo info = textBox.GetComponent<UI_CategoryBoxInfo>();

        info.categoryLabel.text = Data_SearchOptions.CurrentFilter_FormatType.ToString() +
            " " + numberFormat +
            " " + ((int)Data_SearchOptions.CurrentFilter_DataYear).ToString();

        int length = System.Enum.GetNames(typeof(StateName)).Length + 1;

        for (int i = 0; i < length; i++)
        {
            switch ((NumberFormat)System.Enum.Parse(typeof(NumberFormat), numberFormat))
            {
                case NumberFormat.Nominal:
                    info.stateDataList[i].text = result[i];
                    break;
                case NumberFormat.Percentage:
                    info.stateDataList[i].text = result[i] + '%';

                    if (i < length - 1)
                    {
                        Graph_BarHandler.GraphHandler.barList[i].UpdateBarSize(float.Parse(result[i],
                            System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
                    }
                    break;
                default:
                    Debug.LogError(Time.time + ": An error occurred while updating the search filter dropdowns.");
                    break;
            }
        }
    }

    private void UI_ClearCategoryBoxes()
    {
        if (this.categoryBoxList == null)
        {
            return;
        }

        foreach (GameObject box in this.categoryBoxList)
        {
            Destroy(box);
        }

        this.categoryBoxList.Clear();
    }
}
