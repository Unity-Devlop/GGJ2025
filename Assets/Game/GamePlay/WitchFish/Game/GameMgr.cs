using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class GameMgr : MonoSingleton<GameMgr>
    {
        [SerializeField] private Transform fishSpawnPoint;
        [SerializeField] private Transform fishWaitingFoodPoint;

        [SerializeField] private Transform fishJumpPoint;

        // private List<Transform> _fishJumpTrailLst;
        // public IReadOnlyList<Transform> fishJumpTrailLst => _fishJumpTrailLst;
        [SerializeField] private float fishSpawnInterval;

        [SerializeField] private List<GameObject> fishPrefabList;

        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private List<Fish> currentLandFishList = new List<Fish>();

        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private List<Fish> currentLakeFishList = new List<Fish>();

        // 当前在岸上排队等食物的鱼
        // [SerializeField] private List<Fish> currentLandWaitingFishList = new List<Fish>();

        // 鱼的身位间隔
        [SerializeField] private Vector3 fishOffset;

        public float _spawnTimer;


        public float rayDistance = 2;

        public Vector3 direction = new Vector3(-1, 0, 0);

        public float maxY = 10;
        public float minY = -10;

        protected override void OnInit()
        {
            // _fishJumpTrailLst = fishJumpPoint.GetComponentsInChildren<Transform>().ToList();
            // _fishJumpTrailLst.Remove(fishJumpPoint);
        }

        private void Update()
        {
            if (currentLandFishList.Count < 6)
            {
                _spawnTimer += Time.deltaTime;
                if (_spawnTimer >= fishSpawnInterval)
                {
                    _spawnTimer = 0;
                    SpawnFish();
                }
            }
        }

        [Sirenix.OdinInspector.Button]
        private void SpawnFish()
        {
            var fishPrefab = fishPrefabList.RandomTakeWithoutRemove();
            var obj = GameObject.Instantiate(fishPrefab, fishSpawnPoint.position, Quaternion.identity);
            var fish = obj.GetComponent<Fish>();
            SetupFish(fish);
            currentLandFishList.Add(fish);
            fish.stateMachine.Run<FishSpawnState>();
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

        public void DeSpawn(Fish owner)
        {
            currentLandFishList.Remove(owner);
#if UNITY_EDITOR
            owner.transform.SetAsLastSibling();
#endif
            GameObject.Destroy(owner.gameObject, Time.deltaTime);
        }

        public Vector3 GetReturnPosition()
        {
            return fishSpawnPoint.position;
        }
    }
}