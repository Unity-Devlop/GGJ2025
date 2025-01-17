using UnityToolkit;

namespace WitchFish
{
    public class FishIdleState : IState<Fish>
    {
        public void OnInit(Fish owner, IStateMachine<Fish> stateMachine)
        {
        }

        public void OnEnter(Fish owner, IStateMachine<Fish> stateMachine)
        {
        }

        public void Transition(Fish owner, IStateMachine<Fish> stateMachine)
        {
            if (owner.target == Fish.TargetStateEnum.MoveToWait)
            {
                stateMachine.SetParam(Fish.MoveTarget, GameMgr.Singleton.fishWaitingFoodPoint.position);
                stateMachine.Change<FishMoveState>();
            }
            else if (owner.target == Fish.TargetStateEnum.MoveToJump)
            {
                stateMachine.SetParam(Fish.MoveTarget,GameMgr.Singleton.fishJumpPoint.position);
                stateMachine.Change<FishMoveState>();
            }
        }

        public void OnUpdate(Fish owner, IStateMachine<Fish> stateMachine)
        {
        }

        public void OnExit(Fish owner, IStateMachine<Fish> stateMachine)
        {
            // throw new NotImplementedException();
        }
    }
}