using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;

    public IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        Color c = fadeImage.color;

        while (time < duration)
        {
            float t = time / duration;
            c.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeImage.color = c;
            time += Time.deltaTime;
            yield return null;
        }

        c.a = endAlpha;
        fadeImage.color = c;
    }
}
