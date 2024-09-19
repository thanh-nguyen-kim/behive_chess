using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
public class AudioController : Singleton<AudioController>
{
    private float originVol;
    private bool isSfxMute, isMusicMute;
    [SerializeField] private AudioSource sfxSource, musicSource;
    public bool Music
    {
        set
        {
            PlayerPrefs.SetInt("music_audio", value ? 1 : 0);
            isMusicMute = !value;
            if (isMusicMute)
            {
                Tween.Volume(musicSource, 0f, 1f, 0, Tween.EaseOut, Tween.LoopType.None, null, () => { musicSource.Stop(); });
            }
            else
            {
                musicSource.Play();
                Tween.Volume(musicSource, originVol, 1f, 0, Tween.EaseIn);
            }
            //fade in fade out music here
        }
        get { return PlayerPrefs.GetInt("music_audio", 1) == 1; }
    }
    public bool SFX
    {
        set
        {
            PlayerPrefs.SetInt("sfx_audio", value ? 1 : 0);
            isSfxMute = !value;
        }
        get { return PlayerPrefs.GetInt("sfx_audio", 1) == 1; }
    }
    protected override void OnRegistration()
    {
        originVol = musicSource.volume;
        isMusicMute = !Music;
        isSfxMute = !SFX;
    }
    public void PlaySfx(AudioClip clip, float volume = 1)
    {
        if (!isSfxMute)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }
    public void PlayMusic(AudioClip clip, bool isLoop)
    {
        musicSource.clip = clip;
        musicSource.loop = isLoop;
        if (!isMusicMute)
            musicSource.Play();
    }
}
