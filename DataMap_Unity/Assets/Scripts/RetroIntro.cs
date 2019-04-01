using UnityEngine;
using UnityEngine.UI;

public class RetroIntro : MonoBehaviour
{
    [Header("Intro Window")]
    [SerializeField]
    private Image m_TitleText;
    [SerializeField]
    private Image m_DescriptionText;
    [SerializeField]
    private float m_FadeRate = 5f;

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
    }
}
