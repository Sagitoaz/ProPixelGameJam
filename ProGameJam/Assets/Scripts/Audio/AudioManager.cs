using UnityEngine;

public class AudioManager : MonoBehaviour, IDataPersistence
{
    private bool isRunningLoop = false;
    [Header("-------- Audio Source --------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("-------- Audio Clip --------")]
    public AudioClip background;
    public AudioClip run;
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip jump;
    public AudioClip dash;
    public AudioClip death;
    public AudioClip coinCollect;
    public AudioClip HealingSound;
    public AudioClip hitSound;

    [Header("-------- Volume Settings --------")]
    [Range(0f, 1f)] public float musicVolume;
    [Range(0f, 1f)] public float sfxVolume;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();

        musicSource.volume = musicVolume;
        SFXSource.volume = sfxVolume;
    }

    private void Update()
    {
        musicSource.volume = musicVolume;
        SFXSource.volume = sfxVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void Run()
    {
        if (!isRunningLoop || SFXSource.clip != run)
        {
            SFXSource.clip = run;
            SFXSource.loop = true;
            SFXSource.Play();
            isRunningLoop = true;
        }
    }

    public void StopRun()
    {
        if (isRunningLoop)
        {
            SFXSource.Stop();
            SFXSource.loop = false;
            isRunningLoop = false;
        }
    }
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume = value;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSfxVolume()
    {
        return sfxVolume;
    }

    public void LoadData(GameData data)
    {
        this.musicVolume = data.musicVolume;
        this.sfxVolume = data.sfxVolume;
    }

    public void SaveData(ref GameData data)
    {
        data.musicVolume = this.musicVolume;
        data.sfxVolume = this.sfxVolume;
    }
}
