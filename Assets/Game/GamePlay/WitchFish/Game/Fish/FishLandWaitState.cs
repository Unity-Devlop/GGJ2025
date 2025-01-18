using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class FishLandWaitState : IState<Fish>
    {
        private float _timer;

        public void OnInit(Fish owner, IStateMachine<Fish> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnEnter(Fish owner, IStateMachine<Fish> stateMachine)
        {
            _timer = 0;
        }

        public void Transition(Fish owner, IStateMachine<Fish> stateMachine)
        {
            _timer += Time.deltaTime;
            if (_timer > owner.maxWaitTime)
            {
                stateMachine.Change<FishLandReturnState>();
            }
            else if (_timer >= owner.beginAngryWaitTime)
            {
                // TODO 生气
            }
        }

        public void OnUpdate(Fish owner, IStateMachine<Fish> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnExit(Fish owner, IStateMachine<Fish> stateMachine)
        {
            // throw new System.NotImplementedException();
        }
    }
}