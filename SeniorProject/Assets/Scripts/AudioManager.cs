using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource mainBGM;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float endVolume = audioSource.volume;
        audioSource.volume = 0;
        Debug.Log("Fading in");

        while (audioSource.volume < endVolume)
        {
            audioSource.volume += endVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        Debug.Log("Back to full");
        audioSource.volume = endVolume;
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        Debug.Log("Fading Out");
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        Debug.Log("Pausing");
        audioSource.Pause();
        audioSource.volume = startVolume;
    }
}
