using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.Serialization;
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


        private List<Transform> _lakeFishSpawnPointList;

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private List<Fish> _currentLandFishWaitList = new List<Fish>();

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private List<Fish> _currentLakeFishWaitList = new List<Fish>();


        public BindableProperty<int> lakeFishCount { get; private set; }

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
                }
            }

            if (_currentLandFishWaitList.Count < 6)
            {
                _spawnTimer += Time.deltaTime;
                if (_spawnTimer >= fishSpawnInterval)
                {
                    _spawnTimer = 0;
                    SpawnFish();
                }
            }

            if (lakeFishCount.Value > minFishCountToSpawnLakeFish)
            {
                _lakeSpawnTimer += Time.deltaTime;
                if (_lakeSpawnTimer >= fishSpawnInterval)
                {
                    _lakeSpawnTimer = 0;
                    SpawnLakeFish();
                }
            }
        }

        public float lakeFishSpawnInterval = 10f;

        float _lakeSpawnTimer;

        [Sirenix.OdinInspector.Button]
        private void SpawnLakeFish()
        {
            var lakeFishPrefab = lakeFishPrefabList.RandomTakeWithoutRemove();
            foreach (var transform1 in _lakeFishSpawnPointList)
            {
                if (transform1.childCount == 0)
                {
                    var obj = GameObject.Instantiate(lakeFishPrefab, transform1);
                    var fish = obj.GetComponent<Fish>();
                    SetupLakeFish(fish);
                    _currentLakeFishWaitList.Add(fish);
                    fish.stateMachine.Run<FishLakeWaitState>();
                    break;
                }
            }

            lakeFishCount.Value -= 1;
        }


        public List<GameObject> lakeFishPrefabList;

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


        private void SetupLakeFish(Fish fish)
        {
        }

        private void SetupFish(Fish fish)
        {
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
            lakeFishCount.Value += 1;
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

        public void EnterLake(Fish fish)
        {
            lakeFishCount.Value += 1;
            DeSpawn(fish);
        }
    }
}