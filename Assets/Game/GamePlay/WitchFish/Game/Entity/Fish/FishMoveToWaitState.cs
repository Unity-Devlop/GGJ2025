using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class FishMoveToWaitState : IState<Fish>
    {
        public void OnInit(Fish owner, IStateMachine<Fish> stateMachine)
        {
        }

        public void OnEnter(Fish owner, IStateMachine<Fish> stateMachine)
        {
            
            owner.animator.Play("FishIdle");
        }

        public void Transition(Fish owner, IStateMachine<Fish> stateMachine)
        {
            // 前面有正在等待的鱼 自己就停
            if (GameMgr.Singleton.CloseToFinalWaitPoint(owner))
            {
                stateMachine.Change<FishLandWaitState>();
            }
        }

        public void OnUpdate(Fish owner, IStateMachine<Fish> stateMachine)
        {
            bool frontHasOtherFish = false;
            foreach (var raycastHit2D in owner.hit2Ds)
            {
                if (raycastHit2D.collider != null
                    && raycastHit2D.collider.TryGetComponent(out Fish fish)
                    && fish != owner
                    && (fish.stateMachine.currentState is FishLandWaitState ||
                        fish.stateMachine.currentState is FishMoveToWaitState ||
                        fish.stateMachine.currentState is FishMoveToJumpState
                    )
                   )
                {
                    frontHasOtherFish = true;
                    break;
                }
            }

            if (!frontHasOtherFish)
            {
                Move(owner);
            }
        }

        private void Move(Fish owner)
        {
            Vector3 target = GameMgr.Singleton.GetWaitPosition();
            Vector3 direction = (target - owner.transform.position).normalized;
            owner.transform.position += direction * (Time.deltaTime * owner.moveSpeed);
        }

        public void OnExit(Fish owner, IStateMachine<Fish> stateMachine)
        {
        }
    }
}