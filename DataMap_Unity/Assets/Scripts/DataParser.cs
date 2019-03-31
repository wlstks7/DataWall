using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Networking = UnityEngine.Networking;
using Enums;

[RequireComponent(typeof(Camera))]
public class DataParser : MonoBehaviour
{
    private const string categoryBox_AssetName = "Timeline Data Container";

    internal static DataParser Instance { get; set; }

    [Header("UI Windows")]
    [SerializeField]
    private GameObject m_MapWindow;
    [SerializeField]
    private GameObject m_DataWindow;
    [SerializeField]
    private GameObject m_ContentContainer;

    [Header("Intro Window")]
    [SerializeField]
    private Image m_TitleText;
    [SerializeField]
    private Image m_DescriptionText;
    [SerializeField]
    private float m_FadeRate = 5f;

    [Header("Camera Defaults")]
    [SerializeField]
    private float m_DefaultOrthoSize = 5f;

    [Header("Camera Movement Settings")]
    [SerializeField]
    private float m_OrthoZoomSpeed = 0.1f;
    [SerializeField]
    private float m_CameraMoveSpeed = 0.1f;

    private List<GameObject> categoryBoxList = new List<GameObject>();

    // TODO: Move camera functions to separate script
    private Camera m_Camera;

    private void Awake()
    {
        Instance = this;

        Instance.m_Camera = Instance.GetComponent<Camera>();

        Instance.Test_ResetCamera_Hard();
        Instance.CameraToMapView();
    }

    private void Start()
    {
        StartCoroutine("FadeIntroWindow");
    }

    private Color tempColor;
    private float alphaDiff;
    private float targetAlpha;

    private System.Collections.IEnumerator FadeIntroWindow()
    {
        this.m_TitleText.enabled = true;
        this.m_DescriptionText.enabled = false;

        yield return new WaitForSecondsRealtime(3.0f);

        // Fade out title
        targetAlpha = 0f;
        while (true)
        {
            tempColor = this.m_TitleText.color;
            alphaDiff = Mathf.Abs(tempColor.a - targetAlpha);
            if (alphaDiff > 0.0001f)
            {
                tempColor.a = Mathf.Lerp(tempColor.a, targetAlpha, this.m_FadeRate * Time.deltaTime);
                this.m_TitleText.color = tempColor;
            }
            else
            {
                break;
            }
            yield return null;
        }

        Debug.Log("Phase2");

        this.m_DescriptionText.enabled = true;
        tempColor = this.m_DescriptionText.color;
        tempColor.a = 0;
        this.m_DescriptionText.color = tempColor;

        // Fade in description
        targetAlpha = 1f;
        while (true)
        {
            tempColor = this.m_DescriptionText.color;
            alphaDiff = Mathf.Abs(tempColor.a - targetAlpha);
            if (alphaDiff > 0.0001f)
            {
                tempColor.a = Mathf.Lerp(tempColor.a, targetAlpha, this.m_FadeRate * Time.deltaTime);
                this.m_DescriptionText.color = tempColor;
            }
            else
            {
                break;
            }
            yield return null;
        }
        Debug.Log("Exiting Coroutine");
    }

    public void ResetCamera_Smooth()
    {
        UpdateCamera(Vector3.zero, Instance.m_DefaultOrthoSize);
        CameraPoint.EnableAllPoints();
    }

    [ContextMenu("Reset Camera Position")]
    private void Test_ResetCamera_Hard()
    {
        this.transform.position = Vector3.zero;
        this.GetComponent<Camera>().orthographicSize = this.m_DefaultOrthoSize;
    }

    public void CameraToDataView()
    {
        if (FilterFlags.IsDirty)
        {
            ClearStoredData();
        }

        Instance.m_DataWindow.SetActive(true);
        Instance.m_MapWindow.SetActive(false);
    }

    public void CameraToMapView()
    {
        Instance.m_MapWindow.SetActive(true);
        Instance.m_DataWindow.SetActive(false);

        Instance.ResetCamera_Smooth();
    }

    internal static void UpdateCamera(Vector3 newPosition, float newOrthoSize)
    {
        Instance.StopCoroutine("UpdateCameraPosition");
        Instance.StartCoroutine("UpdateCameraPosition", newPosition);

        Instance.StopCoroutine("UpdateCameraOrthoSize");
        Instance.StartCoroutine("UpdateCameraOrthoSize", newOrthoSize);
    }

    private System.Collections.IEnumerator UpdateCameraPosition(Vector3 newPosition)
    {
        while (true)
        {
            if (Instance.m_Camera.transform.position == newPosition)
            {
                yield break;
            }

            Instance.m_Camera.transform.position = Vector3
                .Lerp(Instance.m_Camera.transform.position, newPosition, Instance.m_OrthoZoomSpeed);

            yield return null;
        }
    }

    private System.Collections.IEnumerator UpdateCameraOrthoSize(float newOrthoSize)
    {
        while (true)
        {
            if (Instance.m_Camera.orthographicSize == newOrthoSize)
            {
                yield break;
            }

            Instance.m_Camera.orthographicSize = Mathf
                .Lerp(Instance.m_Camera.orthographicSize, newOrthoSize, Instance.m_CameraMoveSpeed);

            yield return null;
        }
    }

    public void ParseData()
    {
        Instance.StopCoroutine("UpdateData");
        Instance.StartCoroutine("UpdateData");
    }

    private System.Collections.IEnumerator UpdateData()
    {
        Debug.Log(Time.time + ": PARSE START");

        string[] result;

        //TODO: Multi-category support

        FilterFlags.ApplyFilters();

        ClearStoredData();

        foreach (string numberFormat in Enum.GetNames(typeof(NumberFormat)))
        {
            string filePath = System.IO.Path.Combine(
                "https://raw.githubusercontent.com/RGRoland/DataWall/gh-pages/Files/DataWall-Files/DataMap/",
                FilterFlags.CurrentFilters.Filter_Format.ToString() +
                numberFormat +
                FilterFlags.CurrentFilters.Filter_Legality.ToString() +
                ".csv");

            Debug.Log("Filepath: " + filePath);

            var www = Networking.UnityWebRequest.Get(filePath);

            yield return www.SendWebRequest();

            result = www.downloadHandler.text.Split('\n');

            Debug.Log(Time.time + ": " + numberFormat + " data successfully retrieved.");
            Debug.Log(Time.time + ": Result length = " + result.Length);

            //TODO: Multi-year parsing

            // Result now equals comma-separated string array - each element equals timeline data for each line (a state)
            // > Sample Element: "1,2222,3,4444,55,666,77,888888,9999"

            result = result[FilterFlags.CurrentFilters.Filter_State.ToIndex()].Split(',');

            Debug.Log(Time.time + ": Adding UI Elements.");

            UI_AddCategoryBox(result, numberFormat);

            yield return null;
        }

        Debug.Log(Time.time + ": PARSE END");

        yield break;
    }

    private static void UI_AddCategoryBox(string[] result, string numberFormat)
    {
        StateDataPanel boxInfo = Instantiate(Resources.Load<GameObject>(categoryBox_AssetName), Instance.m_ContentContainer.transform)
            .GetComponent<StateDataPanel>();

        Instance.categoryBoxList.Add(boxInfo.gameObject);

        boxInfo.categoryLabel.text = FilterFlags.CurrentFilters.Filter_Format.ToString() + ": " + numberFormat;

        int length = Enum.GetNames(typeof(DataYear)).Length;

        for (int i = 0; i < length; i++)
        {
            switch ((NumberFormat)Enum.Parse(typeof(NumberFormat), numberFormat))
            {
                case NumberFormat.Nominal:
                    boxInfo.stateDataList[i].text = String.Format("{0:n0}", Int32.Parse(result[i]));
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

    private static void ClearStoredData()
    {
        foreach (GameObject box in Instance.categoryBoxList)
        {
            if (box != null)
            {
                Destroy(box);
            }
        }

        Instance.categoryBoxList.Clear();
    }
}
