using System.Collections.Generic;
using UnityEngine;
using Networking = UnityEngine.Networking;
using Enums;
using UnityEngine.UI;

public class DataParser : MonoBehaviour
{
	public const string categoryBox_AssetName = "Timeline Data Container";

	[SerializeField]
	private GameObject m_FiltersTab;
	[SerializeField]
	private Button m_ParserButton;
	[SerializeField]
	private GameObject m_ContentContainer;

	private List<GameObject> categoryBoxList = new List<GameObject>();

	private void Awake()
	{
		this.m_ParserButton.onClick.AddListener(() =>
		{
			StopCoroutine("ParseFileData");
			StartCoroutine("ParseFileData");
		});
	}

	private void Start()
	{
		Data_FilterFlags.Filter_State = StateName.Arizona;
	}

	private System.Collections.IEnumerator ParseFileData()
	{
		Debug.Log(Time.time + ": PARSE");
		string[] result;

		//TODO: Multi-category support

		this.ClearStoredData();

		foreach (string numberFormat in System.Enum.GetNames(typeof(NumberFormat)))
		{
			string filePath = System.IO.Path.Combine("https://raw.githubusercontent.com/RGRoland/DataWall/gh-pages/Files/DataWall-Files/Raw/",
				Data_FilterFlags.Filter_Format.ToString() + numberFormat + Data_FilterFlags.Filter_Legality.ToString() + ".csv");

			Debug.Log(Time.time + ": Filepath = " + filePath);

			var www = Networking.UnityWebRequest.Get(filePath);

			yield return www.SendWebRequest();

			result = www.downloadHandler.text.Split('\n');

			Debug.Log(Time.time + ": " + numberFormat + " data successfully retrieved.");
			Debug.Log(Time.time + ": Result length = " + result.Length);

			//TODO: Multi-year parsing

			// Result now equals comma-separated string array - each element equals timeline data for each line (a state)
			// > Sample Element: "1,2222,3,4444,55,666,77,888888,9999"

			result = result[Data_FilterFlags.Filter_State.ToIndex()].Split(',');

			Debug.Log(Time.time + ": Adding UI Elements.");

			this.UI_AddCategoryBox(result, numberFormat);

			yield return null;
		}

		yield break;
	}

	private void UI_AddCategoryBox(string[] result, string numberFormat)
	{
		StateDataPanel boxInfo = Instantiate(Resources.Load<GameObject>(categoryBox_AssetName), this.m_ContentContainer.transform)
			.GetComponent<StateDataPanel>();

		this.categoryBoxList.Add(boxInfo.gameObject);

		boxInfo.categoryLabel.text = Data_FilterFlags.Filter_Format.ToString() + ": " + numberFormat;

		int length = System.Enum.GetNames(typeof(DataYear)).Length;

		for (int i = 0; i < length; i++)
		{
			switch ((NumberFormat)System.Enum.Parse(typeof(NumberFormat), numberFormat))
			{
				case NumberFormat.Nominal:
					boxInfo.stateDataList[i].text = result[i];
					break;
				case NumberFormat.Percentage:
					boxInfo.stateDataList[i].text = result[i] + '%';
					break;
				default:
					Debug.LogError(Time.time + ": An error occurred while updating the search filter dropdowns.");
					break;
			}
		}
	}

	private void ClearStoredData()
	{
		foreach (GameObject box in this.categoryBoxList)
		{
			if (box != null)
			{
				Destroy(box);
			}
		}

		this.categoryBoxList.Clear();
	}
}
