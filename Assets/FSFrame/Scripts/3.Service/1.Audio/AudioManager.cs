using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : ManagerBase<AudioManager>
{
    [SerializeField] private AudioSource bgAudioSource;
    [SerializeField] private GameObject prefab_AudioPlay;
    private List<AudioSource> audioPlayList = new List<AudioSource>();
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
        // 倒序遍历 防止载体删除 AudioSource 造成的影响
        for (int i = audioPlayList.Count - 1; i >= 0; i--)
        {
            if (audioPlayList[i] == null)
            {
                audioPlayList.RemoveAt(i);
            }
            else
            {
                SetEffectAudioPlay(audioPlayList[i]);
            }
        }
    }

    private void SetEffectAudioPlay(AudioSource audioPlay, float spatial = -1)
    {
        audioPlay.mute = isMute;
        audioPlay.volume = effectVolume * globalVolume;
        if (spatial != -1)
        {
            audioPlay.spatialBlend = spatial;
        }

        if (isPause)
        {
            audioPlay.Pause();
        }
        else
        {
            audioPlay.UnPause();
        }
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

    #region 背景音乐

    public void PlayBGAudio(AudioClip audioClip, bool loop = true, float volume = -1)
    {
        bgAudioSource.clip = audioClip;
        IsLoop = loop;
        if (volume != -1) BGVolume = volume;
        bgAudioSource.Play();
    }

    public void PlayBGAudio(string clipPath, bool loop = true, float volume = -1)
    {
        AudioClip clip = ResManager.Instance.LoadAsset<AudioClip>(clipPath);
        PlayBGAudio(clipPath, loop, volume);
    }

    #endregion

    #region 特效音乐

    private Transform audioPlayRoot = null;
    
    private AudioSource GetAudioPlay(bool is3D = true)
    {
        if (audioPlayRoot == null)
        {
            audioPlayRoot = new GameObject("AudioPlayRoot").transform;
        }
        AudioSource audioSource = PoolManager.Instance.GetGameObject<AudioSource>(prefab_AudioPlay, audioPlayRoot);
        SetEffectAudioPlay(audioSource, is3D ? 1f : 0f);
        audioPlayList.Add(audioSource);
        return audioSource;
    }

    private void RecycleAudioPlay(AudioSource audioSource, AudioClip audioClip, UnityAction callback, float time)
    {
        StartCoroutine(DoRecycleAudioPlay(audioSource, audioClip, callback, time));
    }

    private IEnumerator DoRecycleAudioPlay(AudioSource audioSource, AudioClip audioClip, UnityAction callback, float time)
    {
        // 延迟 clip 的长度（秒）
        yield return new WaitForSeconds(audioClip.length);
        if (audioClip != null)
        {
            audioSource.FSGameObjectPushPool();
            // 延迟参数（秒）
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }
    }

    /// <summary>
    /// 播放一次特效音乐（资源组件版）
    /// </summary>
    /// <param name="audioClip">音效片段</param>
    /// <param name="component">挂载组件</param>
    /// <param name="volumeScale">音量 0-1</param>
    /// <param name="is3D">是否 3D</param>
    /// <param name="callback">结束播放后的回调</param>
    /// <param name="delay">回调要在结束播放后延迟多久执行</param>
    public void PlayOneShot(AudioClip audioClip,Component component, float volumeScale = 1, bool is3D = true, UnityAction callback = null, float delay = 0)
    {
        AudioSource audioSource = GetAudioPlay(is3D);
        audioSource.transform.SetParent(component.transform);
        audioSource.transform.localPosition = Vector3.zero;
        
        audioSource.PlayOneShot(audioClip, volumeScale);
        RecycleAudioPlay(audioSource, audioClip, callback, delay);
    }
    
    /// <summary>
    /// 播放一次特效音乐（路径组件版）
    /// </summary>
    /// <param name="audioClipPath">音效片段资源路径</param>
    /// <param name="component">挂载组件</param>
    /// <param name="volumeScale">音量 0-1</param>
    /// <param name="is3D">是否 3D</param>
    /// <param name="callback">结束播放后的回调</param>
    /// <param name="delay">回调要在结束播放后延迟多久执行</param>
    public void PlayOneShot(string audioClipPath,Component component, float volumeScale = 1, bool is3D = true, UnityAction callback = null, float delay = 0)
    {
        AudioClip audioClip = ResManager.Instance.LoadAsset<AudioClip>(audioClipPath);
        if (audioClip != null) PlayOneShot(audioClip, component, volumeScale, is3D, callback, delay);
    }

    /// <summary>
    /// 播放一次特效音乐（资源零散版）
    /// </summary>
    /// <param name="audioClip">音效片段</param>
    /// <param name="position"></param>
    /// <param name="volumeScale">音量 0-1</param>
    /// <param name="is3D">是否 3D</param>
    /// <param name="callback">结束播放后的回调</param>
    /// <param name="delay">回调要在结束播放后延迟多久执行</param>
    public void PlayOneShot(AudioClip audioClip,Vector3 position, float volumeScale = 1, bool is3D = true, UnityAction callback = null, float delay = 0)
    {
        AudioSource audioSource = GetAudioPlay(is3D);
        audioSource.transform.localPosition = position;
        
        audioSource.PlayOneShot(audioClip, volumeScale);
        RecycleAudioPlay(audioSource, audioClip, callback, delay);
    }
    
    /// <summary>
    /// 播放一次特效音乐（路径零散版）
    /// </summary>
    /// <param name="audioClipPath">音效片段资源路径</param>
    /// <param name="position"></param>
    /// <param name="volumeScale">音量 0-1</param>
    /// <param name="is3D">是否 3D</param>
    /// <param name="callback">结束播放后的回调</param>
    /// <param name="delay">回调要在结束播放后延迟多久执行</param>
    public void PlayOneShot(string audioClipPath,Vector3 position, float volumeScale = 1, bool is3D = true, UnityAction callback = null, float delay = 0)
    {
        AudioClip audioClip = ResManager.Instance.LoadAsset<AudioClip>(audioClipPath);
        if (audioClip != null) PlayOneShot(audioClip, position, volumeScale, is3D, callback, delay);
    }

    #endregion
}