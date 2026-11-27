using Sirenix.OdinInspector;
using UnityEngine;

public class LocalizationManager : ManagerBase<LocalizationManager>
{
        [SerializeField]
        private LocalizationSetting localizationSetting;
        
        [SerializeField]
        [OnValueChanged("UpdateLanguage")]
        private LanguageType languageType;

        public LanguageType LanguageType
        {
                get => languageType;
                set
                {
                        languageType = value;
                        UpdateLanguage();
                }
        }

        private void UpdateLanguage()
        {
                #if UNITY_EDITOR
                GameRoot.InitForEditor();
                #endif
                EventManager.EventTrigger("UpdateLanguage");
        }
        
        public T GetLocalizationData<T>(string workSpace, string key)  where T : class, ILocalizationResource
        {
                return localizationSetting.GetLocalizationData<T>(workSpace, key, languageType);
        }
}