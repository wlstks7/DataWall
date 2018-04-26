using System.Collections.Generic;
using UnityEngine;
using Enums;

[RequireComponent(typeof(BoxCollider))]
public class CameraPoint : MonoBehaviour
{
	private static List<CameraPoint> CameraPoints = new List<CameraPoint>(10);

	private Vector3 CameraPosition
	{
		get
		{
			return new Vector3(this.transform.position.x, this.transform.position.y) +
				this.CameraOffset;
		}
	}

	private Vector3 CameraOffset
	{
		get
		{
			return this.GetComponent<BoxCollider>().center + (Vector3)this.m_CameraOffset;
		}
	}

	[SerializeField]
	private StateName m_State = StateName.Arizona;
	[SerializeField]
	private float m_OrthoSize = 5f;
	[SerializeField]
	private Vector2 m_CameraOffset;

	private void OnEnable()
	{
		CameraPoints.Add(this);
	}

	private void OnDestroy()
	{
		CameraPoints.Remove(this);
	}

	private void OnMouseDown()
	{
		FilterFlags.UpdateFilter(this.m_State);

		DataParser.UpdateCamera(this.CameraPosition, this.m_OrthoSize);
		DataParser.Instance.CameraToDataView();

		this.DisableOtherPoints();
	}

	[ContextMenu("Move Camera to Point")]
	private void Test_MoveCamera()
	{
		Camera.main.transform.position = this.CameraPosition;
		Camera.main.orthographicSize = this.m_OrthoSize;
	}

	private void DisableOtherPoints()
	{
		Material DisabledMaterial = Resources.Load<Material>("Materials/Disabled");

		foreach (CameraPoint point in CameraPoints)
		{
			if (point == this)
			{
				continue;
			}

			point.GetComponent<Renderer>().material = DisabledMaterial;
			point.GetComponent<BoxCollider>().enabled = false;
		}
	}

	public static void EnableAllPoints()
	{
		Material EnabledMaterial = Resources.Load<Material>("Materials/Enabled");

		foreach (CameraPoint point in CameraPoints)
		{
			point.GetComponent<Renderer>().material = EnabledMaterial;
			point.GetComponent<BoxCollider>().enabled = true;
		}
	}
}
