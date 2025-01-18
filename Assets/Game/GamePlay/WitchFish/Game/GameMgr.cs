using System;
using System.Collections;
using System.Collections.Generic;
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

        [SerializeField] private float fishSpawnInterval;

        [SerializeField] private GameObject fishPrefab;
        [SerializeField] private List<Fish> currentLandFishList = new List<Fish>();

        [SerializeField] private List<Fish> currentLakeFishList = new List<Fish>();

        // 当前在岸上排队等食物的鱼
        [SerializeField] private List<Fish> currentLandWaitingFishList = new List<Fish>();

        // 鱼的身位间隔
        [SerializeField] private Vector3 fishOffset;

        public float _spawnTimer;

        
        public float rayDistance = 2;

        public Vector3 direction = new Vector3(-1, 0, 0);
        
        private void Update()
        {
            if (currentLandFishList.Count <6)
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
            var obj = GameObject.Instantiate(fishPrefab, fishSpawnPoint.position, Quaternion.identity);
            var fish = obj.GetComponent<Fish>();
            currentLandFishList.Add(fish);
            fish.stateMachine.Run<FishSpawnState>();
        }

        public Vector3 GetWaitPosition()
        {
            int count = currentLandWaitingFishList.Count;
            if (count == 0)
            {
                return fishWaitingFoodPoint.position;
            }
            // 往后排一个身位

            Vector3 pos = fishWaitingFoodPoint.position;
            pos += count * fishOffset;
            GameLogger.Log.Information("拿等待位置:{count}-{pos}", count, pos);
            return pos;
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
    }
}