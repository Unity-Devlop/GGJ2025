using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FMOD;
using FMODUnity;
using Game;
using Game.Flow;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityToolkit;

namespace WitchFish
{
    public class GameMgr : MonoSingleton<GameMgr>
    {
        [SerializeField] private Transform fishSpawnPoint;
        [SerializeField] private Transform fishWaitingFoodPoint;

        [SerializeField] private Transform fishJumpPoint;

        [SerializeField] private Transform lakeFishSpawnPointParent;

        // private List<Transform> _fishJumpTrailLst;
        // public IReadOnlyList<Transform> fishJumpTrailLst => _fishJumpTrailLst;
        [SerializeField] private float fishSpawnInterval;

        [SerializeField] private List<GameObject> fishPrefabList;
        [SerializeField] private List<GameObject> lakeFishPrefabList;
        public float lakeFishSpawnInterval = 10f;

        float _lakeSpawnTimer;

        public float jumpDestroyTime = 70f;

        private List<Transform> _lakeFishSpawnPointList;

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private List<Fish> _currentLandFishWaitList = new List<Fish>();

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private List<Fish> _currentLakeFishWaitList = new List<Fish>();

        public List<GameObject> waterParticleList = new List<GameObject>();

        public GameObject GameOver;
        /// <summary>
        /// 湖里鱼的数量
        /// </summary>
        public BindableProperty<int> lakeFishCount { get; private set; }

        public int maxHp = 99;

        // public int PlayVideoIndex = 0;

        // 当前在岸上排队等食物的鱼
        // [SerializeField] private List<Fish> currentLandWaitingFishList = new List<Fish>();

        // 鱼的身位间隔
        [SerializeField] private Vector3 fishOffset;

        public float _spawnTimer;


        public float rayDistance = 2;

        public Vector3 direction = new Vector3(-1, 0, 0);

// #if UNITY_EDITOR
//         [SerializeField, Sirenix.OdinInspector.ReadOnly]
// #endif
        public float maxY { get; private set; } = 5;

// #if UNITY_EDITOR
//         [SerializeField, Sirenix.OdinInspector.ReadOnly]
// #endif

        public float minY { get; private set; } = -5;

        public int minFishCountToSpawnLakeFish = 6;

        public SerializableDictionary<ItemEnum, Sprite> id2Sprite = new SerializableDictionary<ItemEnum, Sprite>();


        public SerializableDictionary<ItemEnum, GameObject> id2ItemPrefab =
            new SerializableDictionary<ItemEnum, GameObject>();
        
        public List<GameObject> soupEffectList;

        public Button  closeButton;

        protected override void OnInit()
        {
            lakeFishCount = new BindableProperty<int>();
            var mainCamera = Global.cameraSystem.mainCamera;
            Rect rect = mainCamera.GetOrthographicCameraRect();
            minY = rect.yMin;
            maxY = rect.yMax;

            _lakeFishSpawnPointList = new List<Transform>();
            _lakeFishSpawnPointList = lakeFishSpawnPointParent.GetComponentsInChildren<Transform>().ToList();
            _lakeFishSpawnPointList.Remove(lakeFishSpawnPointParent);

            Core.Event.Listen<EventFishDiePush>(OnSendFishPush);

            Global.Get<AudioSystem>().PlayBGM(FMODName.Event.BGM_BackgroundMusic, out _);

            _spawnTimer = float.MaxValue;
            closeButton.onClick.AddListener(OnCloseToHall);
        }

        protected override void OnDispose()
        {
            if (Global.SingletonNullable != null)
            {
                Global.Get<AudioSystem>().DisposeBGM(FMODName.Event.BGM_BackgroundMusic);
                Global.Get<AudioSystem>().DisposeAllBGM();
            }
        }

        public void OnCloseToHall()
        {
            Global.Get<AudioSystem>().DisposeAllBGM();
            Global.Get<GameFlow>().Change<GameHomeState>();
        }

        private void Update()
        {
            // 鼠标左键发射线
            Vector3 mousePos2D = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Global.cameraSystem.mainCamera.ScreenToWorldPoint(mousePos2D);
                var hit2Ds = Physics2D.RaycastAll(mousePos, Vector2.zero);
                foreach (var raycastHit2D in hit2Ds)
                {
                    var hit = raycastHit2D;
                    if (hit.collider.TryGetComponent(out Item item))
                    {
                        item.OnMouseDrag();
                    }

                    if (hit.collider.TryGetComponent(out Soap soap))
                    {
                        soap.OnMouseDrag();
                    }
                }
            }

            //=================================================
            if (lakeFishCount.Value < 6)
            {
                fishSpawnInterval = 8;
            }
            else if (lakeFishCount.Value < 16)
            {
                fishSpawnInterval = 7;
            }
            else if (lakeFishCount.Value < 31)
            {
                fishSpawnInterval = 6;
            }
            else if (lakeFishCount.Value < 51)
            {
                fishSpawnInterval = 5;
            }

            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= fishSpawnInterval)
            {
                _spawnTimer = 0;
                SpawnFish();
            }
            //=================================================

            
            
            //=================================================
            if (lakeFishCount.Value < 6)
            {
                lakeFishSpawnInterval = 28;
            }
            else if (lakeFishCount.Value < 16)
            {
                lakeFishSpawnInterval = 24;
            }
            else if (lakeFishCount.Value < 31)
            {
                lakeFishSpawnInterval = 24;
            }
            else if (lakeFishCount.Value < 51)
            {
                lakeFishSpawnInterval = 20;
            }

            if (lakeFishCount.Value > minFishCountToSpawnLakeFish)
            {
                _lakeSpawnTimer += Time.deltaTime;
                if (_lakeSpawnTimer >= lakeFishSpawnInterval)
                {
                    _lakeSpawnTimer = 0;
                    SpawnLakeFish();
                }
            }
            //=================================================
        }

        // public Transform basket;

        // 湖里生成了一个鱼 很饿
        [Sirenix.OdinInspector.Button]
        private void SpawnLakeFish()
        {
            var lakeFishPrefab = lakeFishPrefabList.RandomTakeWithoutRemove();
            foreach (var transform1 in _lakeFishSpawnPointList)
            {
                if (transform1.childCount == 0)
                {
                    var obj = Instantiate(lakeFishPrefab, transform1);
                    var fish = obj.GetComponent<Fish>();
                    SetupLakeFish(fish);
                    _currentLakeFishWaitList.Add(fish);
                    fish.stateMachine.Run<FishLakeWaitState>();
                    break;
                }
            }

            lakeFishCount.Value -= 1;
        }

        [Sirenix.OdinInspector.Button]
        private void SpawnFish()
        {
            var fishPrefab = fishPrefabList.RandomTakeWithoutRemove();
            var obj = GameObject.Instantiate(fishPrefab, fishSpawnPoint.position, Quaternion.identity);
            var fish = obj.GetComponent<Fish>();
            SetupFish(fish);
            _currentLandFishWaitList.Add(fish);
            fish.stateMachine.Run<FishSpawnState>();
        }

        private int CalNeedNumber()
        {
            // return 4;
            if (lakeFishCount.Value is >= 0 and < 6)
            {
                return 1;
            }

            if (lakeFishCount.Value is >= 6 and < 16)
            {
                return UnityEngine.Random.value < 0.5 ? 1 : 2;
            }

            if (lakeFishCount.Value is >= 16 and < 31)
            {
                return UnityEngine.Random.value < 0.5 ? 2 : 3;
            }

            if (lakeFishCount.Value is >= 31 and < 51)
            {
                return UnityEngine.Random.Range(2, 5);
            }

            if (lakeFishCount.Value is >= 51)
            {
                return UnityEngine.Random.Range(2, 5);
            }

            return 4;
        }


        private void SetupLakeFish(Fish fish)
        {
            float beginAngryWaitTime = 14;
            float maxWaitTime = 14 + 8;
            fish.beginAngryWaitTime = beginAngryWaitTime;
            fish.maxWaitTime = maxWaitTime;


            FillNeedList(fish);
        }

        private void SetupFish(Fish fish)
        {
            float beginAngryWaitTime = 14;
            float maxWaitTime = 14 + 8;
            fish.beginAngryWaitTime = beginAngryWaitTime;
            fish.maxWaitTime = maxWaitTime;

            FillNeedList(fish);
        }

        private void FillNeedList(Fish fish)
        {
            int needNumber = CalNeedNumber();
            // fish.InvokeRemoveEvent();
            fish.needList = new List<ItemEnum>(needNumber);
            for (int i = 0; i < needNumber; i++)
            {
                float value = UnityEngine.Random.value;
                if (value < 0.5)
                {
                    fish.needList.Add(UnityEngine.Random.value < 0.5 ? ItemEnum.蟹黄堡 : ItemEnum.破皮鞋);
                }
                else if (value < 0.9)
                {
                    fish.needList.Add(UnityEngine.Random.value < 0.5 ? ItemEnum.向日葵 : ItemEnum.章鱼);
                }
                else
                {
                    fish.needList.Add(ItemEnum.核弹);
                }
            }

            // fish.InvokeAddEvent();
        }

        public Vector3 GetWaitPosition()
        {
            return fishWaitingFoodPoint.position;
        }

        public Vector3 GetJumpPosition()
        {
            // TODO 
            return fishJumpPoint.position;
        }

        public bool CloseToJumpPoint(Fish fish)
        {
            return Vector3.Distance(fish.transform.position, fishJumpPoint.position) <= 0.1f;
        }

        public bool CloseToFinalWaitPoint(Fish owner)
        {
            return Vector3.Distance(owner.transform.position, fishWaitingFoodPoint.position) <= 0.1f;
        }

        public void DeSpawn(Item item)
        {
            GameObject.Destroy(item.gameObject, Time.deltaTime);
        }

        public void DeSpawnLake(Fish owner)
        {
            _currentLakeFishWaitList.Remove(owner);
            GameObject.Destroy(owner.gameObject, Time.deltaTime);
        }

        public void DeSpawn(Fish owner)
        {
            _currentLandFishWaitList.Remove(owner);
            GameObject.Destroy(owner.gameObject, Time.deltaTime);
        }

        public Vector3 GetReturnPosition()
        {
            return fishSpawnPoint.position;
        }

        public async void EnterLake(Fish fish)
        {
            lakeFishCount.Value += 1;

            GameLogger.Log.Information("鱼数量+1".Color(Color.yellow));
            Core.Event.Send(new EventFishJumpInLakePush
                { pa = lakeFishCount.ToString() });
            
            
            await UniTask.Delay(TimeSpan.FromSeconds(5f));
            fish.stateMachine.Change<FishWaitDestroyState>();
            
            await UniTask.Delay(TimeSpan.FromSeconds(jumpDestroyTime), cancellationToken: destroyCancellationToken);
            DeSpawn(fish);
        }

        async void OnSendFishPush(EventFishDiePush push)
        {
            await UniTask.DelayFrame(1);
            if (maxHp > 1) maxHp--;
            else
            {
                Global.Get<AudioSystem>().DisposeAllBGM();
                RuntimeManager.PlayOneShotAttached(FMODName.Event.SFX_SoundEffect_8_____, gameObject);


                Time.timeScale = 0;
                GameOver.SetActive(true);
                StartCoroutine(waitForSecond());
            }
        }

        IEnumerator waitForSecond()
        {
            yield return new WaitForSecondsRealtime(3);
            Time.timeScale = 1;
  
        }
    }
}