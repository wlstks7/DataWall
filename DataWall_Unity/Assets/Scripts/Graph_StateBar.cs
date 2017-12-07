using System.Collections;
using UnityEngine;

public class Graph_StateBar : MonoBehaviour
{
    public void UpdateBarSize(float percentage)
    {
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, 0, this.transform.localPosition.z);
        this.transform.localScale.Set(this.transform.localScale.x, 0, this.transform.localScale.z);

        this.StopCoroutine("LerpScale");
        this.StartCoroutine(LerpScale(percentage));
    }

    private IEnumerator LerpScale(float percentage)
    {
        var initialPosition = new Vector3(this.transform.localPosition.x, 0, this.transform.localPosition.z);
        var initialScale = new Vector3(this.transform.localScale.x, 0, this.transform.localScale.z);
        var targetPosition = new Vector3(this.transform.localPosition.x, percentage / 2, this.transform.localPosition.z);
        var targetScale = new Vector3(this.transform.localScale.x, percentage, this.transform.localScale.z);
        float lerpSpeed = 0.8f;
        float timeRemaining = lerpSpeed;

        while (timeRemaining > 0.0f)
        {
            timeRemaining -= Time.deltaTime;

            this.transform.localScale = Vector3.Lerp(targetScale, initialScale, timeRemaining / lerpSpeed);
            this.transform.localPosition = Vector3.Lerp(targetPosition, initialPosition, timeRemaining / lerpSpeed);
            yield return null;
        }

        yield break;
    }
}
