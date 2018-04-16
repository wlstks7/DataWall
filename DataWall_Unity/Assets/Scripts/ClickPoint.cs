using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ClickPoint : MonoBehaviour
{
	[SerializeField]
	private CameraPoint m_CameraPoint;

	private void OnMouseDown()
	{
		Data_FilterFlags.Filter_State = this.m_CameraPoint.m_State;

		DataParser.UpdateCamera(this.m_CameraPoint.Position, this.m_CameraPoint.m_OrthoSize);

		DataParser.Instance.CameraToDataView();
	}
}
