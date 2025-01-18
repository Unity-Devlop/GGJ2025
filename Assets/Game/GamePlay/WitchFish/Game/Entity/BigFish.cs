using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace WitchFish
{
    public class BigFish : MonoBehaviour
    {
        public GameObject panel;
        [SerializeField] private TMP_Text _text;
        private int IndexChat = 0;
        private List<string> whenJumpInLake = new List<string>()
        {
            "真是太逊了，你的鱼塘里才{0}只鱼。",
            "你养了{0}只鱼，就你这样还想成为海王？",
            "你才养了{0}只鱼，我奶奶玩得都比你好。",
            "{0}条鱼，你到底行不行啊牢弟？",
            "现在才{0}条鱼，把鼠标放下，让那块肥皂玩。",

        };

        private List<string> whenInTime = new List<string>()
        {

            "把核弹捡起来，把核弹捡起来！",
            "你要不先去玩会三消练练手速吧？",
            "收手吧牢弟，手机里全都是咸鱼。",
            "就这？",
            "终于有点鱼了，烧火准备吃火锅吧！",

        };

        private List<string> whenDie = new List<string>()
        {

            "又有一条鱼在你的鱼塘里永远沉眠了。",
            "你的鱼淹死了。什么？你说是饿死的？那你不赶紧喂鱼！",

        };

        private List<string> whenRun = new List<string>()
        {
            "不配合的鱼抓回来电疗！",
            "你吃菌子中毒了吧？哪有鱼走？",
        };
        private void Awake()
        {
            GameMgr.Singleton.lakeFishCount.Register(OnLakeFishCountChange);
            StartCoroutine(SpeakInterval());
            Core.Event.Listen<EventFishDieInLandPush>(OnSendFishDieInLand);
            Core.Event.Listen<EventFishDieInLakePush>(OnSendFishDieInLakePush);
            Core.Event.Listen<EventFishJumpInLakePush>(OnSendFishJumpInLakePush);
        }

        private async void OnLakeFishCountChange(int obj)
        {
            // 结束才有
            //_text.text = $"才{obj}条鱼";
            //await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: destroyCancellationToken);
            //_text.text = string.Empty;
        }

        private void OnDestroy()
        {
            Core.Event.UnListen<EventFishDieInLandPush>(OnSendFishDieInLand);
            Core.Event.UnListen<EventFishDieInLakePush>(OnSendFishDieInLakePush);
            Core.Event.UnListen<EventFishJumpInLakePush>(OnSendFishJumpInLakePush);
            if (GameMgr.SingletonNullable != null)
            {
                GameMgr.Singleton.lakeFishCount.UnRegister(OnLakeFishCountChange);
            }
        }

        public void OnMouseDown()
        {
            Global.cameraSystem.SetToMenuCamera();
            panel.gameObject.SetActive(true);

        }

        IEnumerator SpeakInterval()
        {
            while (true) // 无限循环
            {

                var time = UnityEngine.Random.Range(15,20);
                yield return new WaitForSeconds(time);
                _text.text = whenInTime[IndexChat% whenInTime.Count];
                IndexChat++;
                yield return new WaitForSeconds(3);
                _text.text = string.Empty;
                // 等待指定的秒数
            }
        }


        void OnSendFishDieInLand(EventFishDieInLandPush push)
        {
            StartCoroutine( SpeakByPush(whenRun,push.pa));
        }
        void OnSendFishDieInLakePush(EventFishDieInLakePush push)
        {
            StartCoroutine(SpeakByPush(whenDie,push.pa));
        }
        void OnSendFishJumpInLakePush(EventFishJumpInLakePush push)
        {
            Debug.LogError(push.pa);
            StartCoroutine(SpeakByPush(whenJumpInLake,push.pa));
        }


        IEnumerator SpeakByPush(List<string> list,string pa)
        {
            var index = UnityEngine.Random.Range(0, list.Count);
            _text.text = string.Format( list[index],pa);
            yield return new WaitForSeconds(3);
            _text.text = string.Empty;
        }
    }
}