using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace WitchFish
{
    public class BigFish : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private int IndexChat = 0;
        private List<string> list = new List<string>()
        {
            "aaaaaaaaa",
            "bbbbbbbb",
            "cccc",
            "ddd",
        };
        private void Awake()
        {
            GameMgr.Singleton.lakeFishCount.Register(OnLakeFishCountChange);
            StartCoroutine(SpeakInterval());
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

        IEnumerator SpeakInterval()
        {
            while (true) // 无限循环
            {

                var time = UnityEngine.Random.Range(6,8);
                _text.text = list[IndexChat%list.Count];
                IndexChat++;
                yield return new WaitForSeconds(1);
                _text.text = string.Empty;
                // 等待指定的秒数
                yield return new WaitForSeconds(time);
            }
        }
    }
}