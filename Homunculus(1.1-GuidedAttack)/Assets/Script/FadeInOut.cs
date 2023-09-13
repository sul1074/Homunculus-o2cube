using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public Image panel;
    private float fadeDuration;
    float time = 0f;
    float f_time = 1f;
    public float GetFadeDurationTime() { return fadeDuration; }

    public void Fade()
    {
        fadeDuration = 0.5f;
        StartCoroutine(FadeFunc());
    }

    IEnumerator FadeFunc()
    {
        panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / f_time;
            alpha.a = Mathf.Lerp(0, 1, time*2f);
            panel.color = alpha;
            yield return null;
        }

        time = 0f;
        yield return new WaitForSeconds(fadeDuration);

        while(alpha.a > 0f)
        {
            time += Time.deltaTime / f_time;
            alpha.a = Mathf.Lerp(1, 0, time*2f);
            panel.color = alpha;
            yield return null;
        }
        panel.gameObject.SetActive(false);
        yield return null;
    }
}
