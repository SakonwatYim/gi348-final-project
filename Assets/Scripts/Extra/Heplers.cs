using System.Collections;
using UnityEngine;


public static class Heplers 
{
    public static IEnumerator IEFade(CanvasGroup canvas, float desiredValue, float fadeTime)
    {
        float timer = 0f;
        float initialValur = canvas.alpha;
        while (timer < fadeTime)
        {
            canvas.alpha = Mathf.Lerp(initialValur, desiredValue, timer / fadeTime);
            timer += Time.deltaTime;
            yield return null;
        }

        canvas.alpha = desiredValue;
    }
   
}
