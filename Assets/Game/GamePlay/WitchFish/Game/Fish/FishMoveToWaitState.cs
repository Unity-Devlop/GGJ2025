using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class FishMoveToWaitState : IState<Fish>
    {
        Vector3 _targetPosition;

        public void OnInit(Fish owner, IStateMachine<Fish> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnEnter(Fish owner, IStateMachine<Fish> stateMachine)
        {
            GameMgr.Singleton.EnqueueWait(owner);
            _targetPosition = GameMgr.Singleton.GetWaitPosition();
        }

        public void Transition(Fish owner, IStateMachine<Fish> stateMachine)
        {
            if(GameMgr.Singleton.CloseToWaitPoint(owner))
            {
                stateMachine.Change<FishLandWaitState>();
            }
        }

        public void OnUpdate(Fish owner, IStateMachine<Fish> stateMachine)
        {
            Vector3 direction = _targetPosition - owner.transform.position;
            Vector3 vec = direction.normalized * (owner.moveSpeed * Time.deltaTime);
            owner.transform.Translate(vec, Space.World);
        }

        public void OnExit(Fish owner, IStateMachine<Fish> stateMachine)
        {
            GameMgr.Singleton.DequeueWait(owner);
        }
    }
}