using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraPoint : MonoBehaviour
{
	private static List<CameraPoint> CameraPoints = new List<CameraPoint>(10);

	public Vector3 Position { get { return this.transform.position; } }

	public Enums.StateName m_State = Enums.StateName.Arizona;
	public float m_OrthoSize = 1.5f;

	public void OnEnable()
	{
		CameraPoints.Add(this);
	}

	public void OnDestroy()
	{
		CameraPoints.Remove(this);
	}

	public static CameraPoint GetCameraPoint(Enums.StateName stateName)
	{
		return CameraPoints.First((point) => point.m_State == stateName);
	}
}
