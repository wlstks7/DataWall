using System.Collections.Generic;
using UnityEngine;

public class Graph_BarHandler : MonoBehaviour
{
    public static Graph_BarHandler GraphHandler;

    [SerializeField]
    internal List<Graph_StateBar> barList;

    private void Awake()
    {
        GraphHandler = this;
    }

    [ContextMenu("Test UpdateBarSize()")]
    private void TestUpdateBarSize()
    {
        foreach (Graph_StateBar bar in this.barList)
        {
            bar.UpdateBarSize(10);
        }
    }
}
