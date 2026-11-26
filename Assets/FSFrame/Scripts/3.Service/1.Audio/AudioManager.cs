using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AudioManager : ManagerBase<AudioManager>
{
    [SerializeField] private AudioSource bgAudioSource;

    [SerializeField] [Range(0, 1)] [OnValueChanged("UpdateAllAudioPlay")]
    private float globalVolume;

    public float GlobalVolume
    {
        get => globalVolume;
        set
        {
            if(globalVolume == value) return;
            globalVolume = value;
            UpdateAllAudioPlay();
        }
    }

    [SerializeField] [Range(0, 1)] [OnValueChanged("UpdateBGAudioPlay")]
    private float bgVolume;

    public float BGVolume
    {
        get => bgVolume;
        set
        {
            if(bgVolume == value) return;
            bgVolume = value;
            UpdateBGAudioPlay();
        }
    }

    [SerializeField] [Range(0, 1)] [OnValueChanged("UpdateEffectAudioPlay")]
    private float effectVolume;

    public float EffectVolume
    {
        get => effectVolume;
        set
        {
            if(effectVolume == value) return;
            effectVolume = value;
            UpdateEffectAudioPlay();
        }
    }

    [SerializeField][OnValueChanged("UpdateMute")]
    private bool isMute = false;

    public bool IsMute
    {
        get => isMute;
        set
        {
            if(isMute == value) return;
            isMute = value;
            UpdateMute();
        }
    }
    
    [SerializeField][OnValueChanged("UpdateLoop")]    
    private bool isLoop = false;

    public bool IsLoop
    {
        get => isLoop;
        set
        {
            if(isLoop == value) return;
            isLoop = value;
            UpdateLoop();
        }
    }
    
    private bool isPause = false;

    public bool IsPause
    {
        get => isPause;
        set
        {
            if(isPause == value) return;
            isPause = value;
            if (isPause)
            {
                bgAudioSource.Pause();
            }
            else
            {
                bgAudioSource.UnPause();
            }
        }
    }

    private void UpdateAllAudioPlay()
    {
        UpdateBGAudioPlay();
        UpdateEffectAudioPlay();
    }

    private void UpdateBGAudioPlay()
    {
        bgAudioSource.volume = bgVolume * globalVolume;
    }

    private void UpdateEffectAudioPlay()
    {
        // TODO
    }

    private void UpdateMute()
    {
        bgAudioSource.mute = isMute;
        UpdateEffectAudioPlay();
    }

    private void UpdateLoop()
    {
        bgAudioSource.loop = isLoop;
    }
}