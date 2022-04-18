using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource mainBGM, fireball, arrow, star, slap, battleMusic;
    private AudioSource[] allAudio;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        allAudio = FindObjectsOfType<AudioSource>();

        fireball.volume = PlayerPrefs.GetFloat("FX Levels");
        arrow.volume = PlayerPrefs.GetFloat("FX Levels");
        star.volume = PlayerPrefs.GetFloat("FX Levels");
        slap.volume = PlayerPrefs.GetFloat("FX Levels");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Pausing all");
            foreach (AudioSource aud in allAudio) {
                aud.Pause();
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Fade In from M");
            StartCoroutine(FadeIn(mainBGM, 5f));
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Fade Out from L");
            StartCoroutine(FadeOut(mainBGM, 5f));
        }
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float endVolume = audioSource.volume;
        audioSource.volume = 0;
        audioSource.Play();
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

    public static IEnumerator FadeOutAndStop(AudioSource audioSource, float FadeTime)
    {
        Debug.Log("Fading Out");
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        Debug.Log("Pausing");
        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public void PlayClip(AudioSource aud)
    {
        aud.Play();
    }
}
