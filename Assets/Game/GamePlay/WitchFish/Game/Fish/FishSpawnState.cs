using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class FishSpawnState : IState<Fish>
    {
        private float _timer;
        public void OnInit(Fish owner, IStateMachine<Fish> stateMachine)
        {
        }

        public void OnEnter(Fish owner, IStateMachine<Fish> stateMachine)
        {
            _timer = 0f;
        }

        public void Transition(Fish owner, IStateMachine<Fish> stateMachine)
        {
            // 等几秒钟
            _timer += Time.deltaTime;
            if (_timer > owner.spawnWaitTime)
            {
                _timer = 0f;
                stateMachine.Change<FishMoveToWaitState>();
            }

        }

        public void OnUpdate(Fish owner, IStateMachine<Fish> stateMachine)
        {
        }

        public void OnExit(Fish owner, IStateMachine<Fish> stateMachine)
        {
        }
    }
}