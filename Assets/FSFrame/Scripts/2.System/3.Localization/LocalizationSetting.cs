using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;

public enum LanguageType
{
    Chinese = 0,
    English = 1,
}

public interface ILocalizationResource { };

[Serializable]
public class L_Text : ILocalizationResource
{
    public string content;
}

[Serializable]
public class L_Image  : ILocalizationResource
{
    public Sprite content;
}

[Serializable]
public class L_Audio : ILocalizationResource 
{
    public AudioClip content;
}

[Serializable]
public class L_Video : ILocalizationResource 
{
    public VideoClip content;
}

[Serializable]
public class LocalizationData
{
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, KeyLabel = "语言类型", ValueLabel = "资源内容")]
    public Dictionary<LanguageType, ILocalizationResource> Content =  new Dictionary<LanguageType, ILocalizationResource>()
    {
        {LanguageType.Chinese, null},
        {LanguageType.English, null}
    };
}

[CreateAssetMenu(fileName = "LocalizationSetting",  menuName = "FSFrame/本地化配置")]
public class LocalizationSetting : ConfigBase
{
    [SerializeField]
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, KeyLabel = "情景空间", ValueLabel = "本地化数据")]
    private Dictionary<string, Dictionary<string, LocalizationData>> content  = new Dictionary<string, Dictionary<string, LocalizationData>>();

    public T GetLocalizationData<T>(string workSpace, string key, LanguageType language) where T : class, ILocalizationResource
    {
        return content[workSpace][key].Content[language] as T;
    }
}