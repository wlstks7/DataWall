using UnityEngine;

public class UI_StateInfoHandler : MonoBehaviour
{
	public GameObject stateInfoWindow;

	public void OnMouseEnter()
	{
		if (this.stateInfoWindow != null)
		{
			this.stateInfoWindow.SetActive(true);
		}
	}

	public void OnMouseExit()
	{
		if (this.stateInfoWindow != null)
		{
			this.stateInfoWindow.SetActive(false);
		}
	}
}
