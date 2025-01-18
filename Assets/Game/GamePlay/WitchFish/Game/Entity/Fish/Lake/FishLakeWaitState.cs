using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class FishLakeWaitState : IState<Fish>
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
            if (owner.needList.Count == 0)
            {
                stateMachine.Change<FishReturnLakeState>();
            }
            else if (_timer > owner.beginAngryWaitTime && _timer < owner.maxWaitTime)
            {
                // TODO 生气
            }
            else if (_timer > owner.maxWaitTime)
            {
                stateMachine.Change<FishLakeDeadState>();
            }
        }

        public void OnUpdate(Fish owner, IStateMachine<Fish> stateMachine)
        {
            _timer += Time.deltaTime;
        }

        public void OnExit(Fish owner, IStateMachine<Fish> stateMachine)
        {
            // throw new System.NotImplementedException();
        }
    }
}