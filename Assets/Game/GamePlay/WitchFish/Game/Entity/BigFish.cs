using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace WitchFish
{
    public class BigFish : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private void Awake()
        {
            GameMgr.Singleton.lakeFishCount.Register(OnLakeFishCountChange);
        }

        private async void OnLakeFishCountChange(int obj)
        {
            _text.text = $"才{obj}条鱼";
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: destroyCancellationToken);
            _text.text = string.Empty;
        }

        private void OnDestroy()
        {
            if (GameMgr.SingletonNullable != null)
            {
                GameMgr.Singleton.lakeFishCount.UnRegister(OnLakeFishCountChange);
            }
        }
    }
}