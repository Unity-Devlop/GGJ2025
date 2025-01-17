using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class FishMoveState : IState<Fish>
    {
        public Vector3 target;
        public void OnInit(Fish owner, IStateMachine<Fish> stateMachine)
        {
            
        }

        public void OnEnter(Fish owner, IStateMachine<Fish> stateMachine)
        {
            target = stateMachine.GetParam<Vector3>(Fish.MoveTarget);
            stateMachine.RemoveParam(Fish.MoveTarget);
        }

        public void Transition(Fish owner, IStateMachine<Fish> stateMachine)
        {
            float distance = Vector3.Distance(owner.transform.position, target);
            bool moveOver = distance < 0.1f;
            if (owner.target == Fish.TargetStateEnum.MoveToWait && moveOver)
            {
                owner.target = Fish.TargetStateEnum.Wait;
                stateMachine.Change<FishIdleState>();
            }
            else if (owner.target == Fish.TargetStateEnum.MoveToJump && moveOver)
            {
                owner.target = Fish.TargetStateEnum.Jump;
                stateMachine.Change<FishJumpState>();
            }
        }

        public void OnUpdate(Fish owner, IStateMachine<Fish> stateMachine)
        {
            Vector3 direction = (target - owner.transform.position).normalized;
            Vector3 vec = direction * (owner.moveSpeed * Time.deltaTime);
            owner.transform.Translate(vec);
        }

        public void OnExit(Fish owner, IStateMachine<Fish> stateMachine)
        {
        }
    }
}