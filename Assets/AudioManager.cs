using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip backgroungMusic;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if(musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        if(sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

    }

    private void Start()
    {
        PlayMusic(backgroungMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if(clip != null)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.volume = 0.1f;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
