using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FMOD;
using FMODUnity;
using Game;
using TMPro;
using UnityEngine;
using UnityToolkit;
using Random = System.Random;

namespace WitchFish
{
    public class BigFish : MonoBehaviour, ISoap
    {
        public GameObject panel;
        [SerializeField] private TMP_Text _text;
        private int IndexChat = 0;
        [SerializeField] private Animator _animator;

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

        private List<string> soapStringList = new List<string>()
        {
            "我很干净",

            "我很干净",
            "我很干净"
        };

        readonly Queue<string> _queue = new Queue<string>();

        private void Awake()
        {
            _text.text = "";
            GameMgr.Singleton.lakeFishCount.Register(OnLakeFishCountChange);
            StartCoroutine(AutoEnqueue());
            Core.Event.Listen<EventFishDieInLandPush>(OnSendFishDieInLand);
            Core.Event.Listen<EventFishDieInLakePush>(OnSendFishDieInLakePush);
            Core.Event.Listen<EventFishJumpInLakePush>(OnSendFishJumpInLakePush);
            ExecuteQueue().Forget();
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

        // public void OnMouseDown()
        // {
        //     Global.cameraSystem.SetToMenuCamera();
        //     panel.gameObject.SetActive(true);
        // }

        public void OnSoup()
        {
            float random = UnityEngine.Random.value;
            if (random < 2
                && _queue.Count == 0)
            {
                string str = soapStringList[UnityEngine.Random.Range(0, soapStringList.Count)];
                _queue.Enqueue(str);
                RuntimeManager.PlayOneShotAttached(FMODName.Event.SFX_SoundEffect_2___, gameObject);

                var prefab = GameMgr.Singleton.waterParticleList.RandomTakeWithoutRemove();
                var effect = Instantiate(prefab, transform.position, Quaternion.identity);
                Destroy(effect, 0.5f);
            }
        }


        private async UniTask ExecuteQueue()
        {
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                if (_queue.Count == 0)
                {
                    _animator.enabled = false;
                    await UniTask.Yield();
                    continue;
                }

                var str = _queue.Dequeue();
                _text.text = str;
                _animator.enabled = true;
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f * str.Length * UnityEngine.Random.value * 2));
                _text.text = "";
                _animator.enabled = false;
            }
        }

        IEnumerator AutoEnqueue()
        {
            while (true) // 无限循环
            {
                var time = UnityEngine.Random.Range(15, 20);
                yield return new WaitForSeconds(time);
                _queue.Enqueue(whenInTime[IndexChat % whenInTime.Count]);
                IndexChat++;
            }
        }


        void OnSendFishDieInLand(EventFishDieInLandPush push)
        {
            _queue.Enqueue(GetSpeakStr(whenRun, push.pa));
        }

        void OnSendFishDieInLakePush(EventFishDieInLakePush push)
        {
            _queue.Enqueue(GetSpeakStr(whenDie, push.pa));
        }

        void OnSendFishJumpInLakePush(EventFishJumpInLakePush push)
        {
            // Debug.LogError(push.pa);
            if (int.Parse(push.pa) % 10 == 0)
            {
                _queue.Enqueue(GetSpeakStr(whenJumpInLake, push.pa));
            }
        }


        string GetSpeakStr(List<string> list, string pa)
        {
            var index = UnityEngine.Random.Range(0, list.Count);
            return string.Format(list[index], pa);
        }
    }
}