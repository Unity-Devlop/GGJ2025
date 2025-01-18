using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityToolkit;

namespace WitchFish
{
    public class Fish : MonoBehaviour
    {
        public float spawnWaitTime = 0.5f;
        public float moveSpeed = 1f;
        public float beginAngryWaitTime = 0.5f;
        public float maxWaitTime = 3f;
        public StateMachine<Fish> stateMachine { get; private set; }

        private void Awake()
        {
            stateMachine = new StateMachine<Fish>(this);

            stateMachine.Add<FishSpawnState>();
            stateMachine.Add<FishMoveToWaitState>();
            stateMachine.Add<FishLandWaitState>();
            stateMachine.Add<FishLandReturnState>();
            stateMachine.Add<FishMoveToJumpState>();
            stateMachine.Add<FishJumpState>();
            stateMachine.Add<FishLakeReturnState>();
        }

        private void Update()
        {
            if (stateMachine.running)
            {
                stateMachine.OnUpdate();
            }
        }

        private void OnDisable()
        {
            if (stateMachine.running)
            {
                stateMachine.Stop();
            }
        }

        public void OnFeed()
        {
            // TODO 喂东西给我
        }
    }
}