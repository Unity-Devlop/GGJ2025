using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityToolkit;

namespace Game
{
    public class LocalizationSystem : MonoBehaviour, ISystem, IOnInit
    {
        public async void OnInit()
        {
            // 等待初始化完成
            await LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
        }


        public void Dispose()
        {
            LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
        }

        public string Get(string key)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString(key);
        }

        public async UniTask<string> GetAsync(string key)
        {
            return await LocalizationSettings.StringDatabase.GetLocalizedStringAsync(key);
        }

        public T Get<T>(string key) where T : Object
        {
            return LocalizationSettings.AssetDatabase.GetLocalizedAsset<T>(key);
        }

        public async UniTask<T> GetAsync<T>(string key) where T : Object
        {
            return await LocalizationSettings.AssetDatabase.GetLocalizedAssetAsync<T>(key);
        }


        private void OnSelectedLocaleChanged(Locale newLocale)
        {
        }
    }
}