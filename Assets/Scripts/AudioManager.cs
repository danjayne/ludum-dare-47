using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region "Singleton"

    private static AudioManager _instance;

    public static AudioManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        _audioSource = GetComponent<AudioSource>();
    }

    #endregion

    public AudioClip BackgroundMusic;
    public AudioClip WitchMusic;

    private AudioSource _audioSource;

    public void SetPitch(float pitch)
    {
        _audioSource.pitch = pitch;
    }

    public void PlayMusic(MusicEnum music)
    {
        AudioClip audioClip = null;

        try
        {
            if (music == MusicEnum.WitchMusic)
            {
                audioClip = WitchMusic;
            } else if (music == MusicEnum.BackgroundMusic)
            {
                audioClip = BackgroundMusic;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to play sound effect '{music}' due to error '{ex.Message}'");
        }

        if (audioClip != null)
        {
            // what if something is currently playing? delay?
            if (_audioSource.clip != audioClip)
            {
                _audioSource.clip = audioClip;
                _audioSource.Play();
            }
        }
    }

    public void PlaySoundEffect(SoundEffectEnum soundEffect, float? volume = null)
    {
        AudioClip audioClip = null;

        try
        {
            var file = GetSoundEffectFileName(soundEffect);

            if (!string.IsNullOrWhiteSpace(file))
                audioClip = Resources.Load(file) as AudioClip;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to play sound effect '{soundEffect}' due to error '{ex.Message}'");
        }

        if (audioClip != null) // what if something is currently playing? delay?
            _audioSource.PlayOneShot(audioClip, volume ?? _audioSource.volume);
    }

    private string GetMusicFileName(MusicEnum music)
    {
        switch (music)
        {
            case MusicEnum.BackgroundMusic: return "8-bit-halloween";
            case MusicEnum.WitchMusic: return "level-4-witch-music";
        }

        return "nosound";
    }

    private string GetSoundEffectFileName(SoundEffectEnum soundEffect)
    {
        switch (soundEffect)
        {
            case SoundEffectEnum.ArthurHurt: return "arthur-burnt-by-fire";
            case SoundEffectEnum.ArthurHurtByFire: return "arthur-burnt-by-fire";
            case SoundEffectEnum.ArthurHelmetClosing: return "helmet-closing";
            case SoundEffectEnum.Cauldron: return "magic-cauldron";
            case SoundEffectEnum.CurseIntro: return "Curse-intro-sound";
            case SoundEffectEnum.Dash: return "dash";
            case SoundEffectEnum.Footsteps: return "footstep";
            case SoundEffectEnum.FrankensteinHurt: return "frankenstein-hurt";
            case SoundEffectEnum.GhostHurt: return "ghost-hurt";
            case SoundEffectEnum.GravestoneMoving: return "gravestone-moving";
            case SoundEffectEnum.HealthPotion: return "gravestone-moving";
            case SoundEffectEnum.Hurt: return "hurt-sound";
            case SoundEffectEnum.Jump: return "jump-sound";
            case SoundEffectEnum.Landing: return "landing-sound";
            case SoundEffectEnum.SmashCauldron: return "smashing-cauldron";
            case SoundEffectEnum.Slash1: return "slash-sound";
            case SoundEffectEnum.Slash2: return "slash-2-sound";
            case SoundEffectEnum.SpiderHurt: return "spider-hurt";
            case SoundEffectEnum.Stab: return "stab";
            case SoundEffectEnum.WitchCackle: return "witch-cackle";
            case SoundEffectEnum.WitchHurt:  return "witch-hurt";
        }

        return "nosound";
    }
}

public enum SoundEffectEnum
{
    ArthurHurt,
    ArthurHurtByFire,
    ArthurHelmetClosing,
    CurseIntro,
    Dash,
    Footsteps,
    FrankensteinHurt,
    GhostHurt,
    GravestoneMoving,
    HealthPotion,
    Hurt,
    Jump,
    Landing,
    Cauldron,
    SmashCauldron,
    Slash1,
    Slash2,
    SpiderHurt,
    Stab,
    WitchCackle,
    WitchHurt
}

public enum MusicEnum
{
    WitchMusic,
    BackgroundMusic
}