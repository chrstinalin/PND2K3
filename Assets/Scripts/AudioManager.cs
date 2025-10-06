using UnityEngine;
using System.Collections;

// Manages the current overworld theme.
// Manages an AudioSource to play sound effects.
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [HideInInspector] public AudioSource currentlyPlaying;
    [HideInInspector] public AudioSource MainTheme;

    [SerializeField] private AudioClip MainThemeAudio;

    private float FADE_IN_TRANSITION = 2f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if(!MainThemeAudio) Debug.LogWarning("Main Theme Audio not assigned in AudioManager.");

        // Main Theme
        AudioSource MainTheme = gameObject.AddComponent<AudioSource>();
        MainTheme.clip = MainThemeAudio;
        MainTheme.loop = true;
        MainTheme.Play();

        currentlyPlaying = MainTheme;
    }

    public void PlaySFX(AudioClip audioFile)
    {
        if(!audioFile) Debug.LogWarning("No audio file was provided to PlaySFX().");

        GameObject SFX = new GameObject("SFX");
        AudioSource audioSource = SFX.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = audioFile;
        audioSource.Play();
        Destroy(SFX, audioFile.length);
    }

    public void SwitchTheme(AudioSource newTheme)
    {
        if(newTheme == currentlyPlaying) return;

        newTheme.time = currentlyPlaying.time;
        newTheme.volume = 0f;
        newTheme.Play();
        
        StartCoroutine(CrossfadeTheme(currentlyPlaying, newTheme));
        currentlyPlaying = newTheme;
    }

    private IEnumerator CrossfadeTheme(AudioSource from, AudioSource to)
    {
        float timer = 0f;
        while (timer < FADE_IN_TRANSITION)
        {
            float t = timer / FADE_IN_TRANSITION;
            from.volume = Mathf.Lerp(1f, 0f, t);
            to.volume = Mathf.Lerp(0f, 1f, t);
            timer += Time.deltaTime;
            yield return null;
        }
        from.volume = 0f;
        to.volume = 1f;
        from.Stop();
    }
}
