using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class FishLandReturnState : IState<Fish>
    {
        public void OnInit(Fish owner, IStateMachine<Fish> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnEnter(Fish owner, IStateMachine<Fish> stateMachine)
        {
            // throw new System.NotImplementedException();
            
        }

        public void Transition(Fish owner, IStateMachine<Fish> stateMachine)
        {
            Vector3 target = GameMgr.Singleton.GetReturnPosition();
            float dis = Vector3.Distance(owner.transform.position, target);
            if (dis < 0.1f)
            {
                GameMgr.Singleton.DeSpawn(owner);
                Core.Event.Send<EventFishDiePush>();
                Core.Event.Send<EventFishDieInLandPush>();
            }
        }

        public void OnUpdate(Fish owner, IStateMachine<Fish> stateMachine)
        {
            Vector3 target = GameMgr.Singleton.GetReturnPosition();
            Vector3 vec = target - owner.transform.position;
            vec.Normalize();
            owner.transform.position += vec * (owner.moveSpeed * Time.deltaTime);
        }

        public void OnExit(Fish owner, IStateMachine<Fish> stateMachine)
        {
            // throw new System.NotImplementedException();
        }
    }
}