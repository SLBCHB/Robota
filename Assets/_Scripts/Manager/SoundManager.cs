using UnityEngine;
using System.Collections.Generic;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> musicClips;
    [SerializeField] private List<AudioClip> sfxClips;

    private Dictionary<string, AudioClip> musicDict;
    private Dictionary<string, AudioClip> sfxDict;

    protected override void Awake()
    {
        base.Awake();

        musicDict = new Dictionary<string, AudioClip>();
        sfxDict = new Dictionary<string, AudioClip>();

        foreach (var clip in musicClips)
        {
            if (clip != null && !musicDict.ContainsKey(clip.name))
                musicDict.Add(clip.name, clip);
        }

        foreach (var clip in sfxClips)
        {
            if (clip != null && !sfxDict.ContainsKey(clip.name))
                sfxDict.Add(clip.name, clip);
        }
    }

    public void PlayMusic(string clipName, bool loop = true)
    {
        if (musicDict.TryGetValue(clipName, out var clip))
        {
            if (musicSource.clip == clip && musicSource.isPlaying) return;

            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"[SoundManager] Music clip '{clipName}' not found!");
        }
    }
    
    public void StopMusic() => musicSource.Stop();


    public void PlaySFX(string clipName)
    {
        if (sfxDict.TryGetValue(clipName, out var clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"[SoundManager] SFX clip '{clipName}' not found!");
        }
    }

    public void PlaySFX(string clipName, AudioSource source)
    {
        if (sfxDict.TryGetValue(clipName, out var clip))
        {
            source.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"[SoundManager] SFX clip '{clipName}' not found!");
        }
    }

    public void StopSfx() => sfxSource.Stop();
    public void SfxPitch(float pitch) => sfxSource.pitch = pitch;

    public void SetMusicVolume(float volume) => musicSource.volume = Mathf.Clamp01(volume);
    public void SetSFXVolume(float volume) => sfxSource.volume = Mathf.Clamp01(volume);
}

