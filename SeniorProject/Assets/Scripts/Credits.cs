using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public Text credits, thanks;

    // Start is called before the first frame update
    void Start()
    {
        credits.color = new Color(credits.color.r, credits.color.g, credits.color.b, 0);
        thanks.color = new Color(thanks.color.r, thanks.color.g, thanks.color.b, 0);
        Invoke("FadeInCredits", 3f);
        Debug.Log("Fade Out Overworld BGM");
        AudioManager.instance.StartCoroutine(AudioManager.FadeOut(AudioManager.instance.mainBGM, 2f));
        Debug.Log("Fade in Battle Music");
        AudioManager.instance.StartCoroutine(AudioManager.FadeIn(AudioManager.instance.creditsMusic, 3f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeTextIn(float time, Text t)
    {
        t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
        while (t.color.a < 1.0f)
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a + (Time.deltaTime / time));
            yield return null;
        }
    }

    public IEnumerator FadeTextOut(float time, Text t)
    {
        t.color = new Color(t.color.r, t.color.g, t.color.b, 1);
        while (t.color.a > 0.0f)
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a - (Time.deltaTime / time));
            yield return null;
        }
    }

    public void FadeInCredits()
    {
        StartCoroutine(FadeTextIn(2f, credits));
        Invoke("FadeOutCredits", 10f);
    }

    public void FadeOutCredits()
    {
        StartCoroutine(FadeTextOut(2f, credits));
        Invoke("FadeInThanks", 4f);
    }

    public void FadeInThanks()
    {
        StartCoroutine(FadeTextIn(2f, thanks));
    }
}
