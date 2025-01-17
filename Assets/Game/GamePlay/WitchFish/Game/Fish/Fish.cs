using System;
using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class Fish : MonoBehaviour
    {
        public enum TargetStateEnum
        {
            Spawn,
            MoveToWait,
            Wait,
            MoveToJump,
            Jump,
            ReturnToLand,
        }

        public float moveSpeed = 1f;
        public StateMachine<Fish> stateMachine;
        public TargetStateEnum target = TargetStateEnum.Spawn;
        
        public const string MoveTarget = "MoveTarget";
        public const string MoveCompleteAction = "MoveComplete";
        
        private void Awake()
        {
            stateMachine = new StateMachine<Fish>(this);
            stateMachine.Add<FishIdleState>();
            stateMachine.Add<FishMoveState>();
            stateMachine.Add<FishJumpState>();
        }

        private void Update()
        {
            if (stateMachine.running)
            {
                stateMachine.OnUpdate();
            }
        }

        private void OnEnable()
        {
            stateMachine.Run<FishIdleState>();
        }

        private void OnDisable()
        {
            stateMachine.Stop();
        }
    }
}