using System.Collections.Generic;
using UnityEngine;
using Networking = UnityEngine.Networking;

public class DataParser : MonoBehaviour
{
    public const string categoryBox_AssetName = "CategoryBox";

    public static DataParser Parser;

    [SerializeField]
    private string[] result;

    private List<GameObject> categoryBoxList = new List<GameObject>();

    private void Awake()
    {
        Parser = this;

        Parser.StartCoroutine(Parser.ParseFileData(
            SearchOptions.CurrentFilter_FormatType,
            SearchOptions.CurrentFilter_ImmigrationType,
            SearchOptions.CurrentFilter_DataYear));
    }

    [ContextMenu("Test ParseData()")]
    private void TestParseData()
    {
        FormatType formatType = FormatType.Population;
        ImmigrationType immigrationType = ImmigrationType.Unauthorized;
        DataYear dataYear = DataYear.Five;

        StartCoroutine(ParseFileData(formatType, immigrationType, dataYear));
    }

    public System.Collections.IEnumerator ParseFileData(FormatType formatType, ImmigrationType immigrationType, DataYear dataYear)
    {
        Debug.Log("PARSE");

        //TODO: Data Persistence Validation (Eliminate parsing redundancy with query updates)
        //TODO: Multi-category support

        this.UI_ClearCategoryBoxes();

        #region Data Retrieval via File IO

        foreach (string numberFormat in System.Enum.GetNames(typeof(NumberFormat)))
        {
            string filePath = System.IO.Path
                .Combine(Application.streamingAssetsPath, formatType.ToString() + numberFormat + immigrationType.ToString())
                + ".csv";

            if (filePath.Contains("://"))
            {
                var www = Networking.UnityWebRequest.Get(filePath);
                yield return www.Send();
                this.result = www.downloadHandler.text.Split('\n');
            }
            else
            {
                try
                {
                    this.result = System.IO.File.ReadAllLines(filePath);
                }
                catch (System.IO.FileNotFoundException)
                {
                    Debug.Log("File could not be found. FilePath = " + filePath);
                    throw;
                }
            }

            yield return new WaitUntil(() => this.result != null);

            //TODO: Multi-year parsing

            //Result Data equals comma-separated string array - each element equals timeline data for each line (a state)
            // > Sample Element: "1,2222,3,4444,55,666,77,888888,9999"

            for (int i = 0; i < this.result.Length; i++)
            {
                this.result[i] = this.result[i].Split(',')[(int)dataYear - (int)DataYear.Five];
            }


            this.UI_AddCategoryBox(numberFormat);
        }

        #endregion

        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
    }

    private void UI_AddCategoryBox(string numberFormat)
    {
        var textBox = Instantiate(Resources.Load(categoryBox_AssetName, typeof(GameObject)), this.transform) as GameObject;

        this.categoryBoxList.Add(textBox.gameObject);

        UI_CategoryBoxInfo info = textBox.GetComponent<UI_CategoryBoxInfo>();

        info.categoryLabel.text = numberFormat;

        for (int i = 0; i < System.Enum.GetNames(typeof(StateName)).Length + 1; i++)
        {
            info.stateDataList[i].text = this.result[i];
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
