using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class FishJumpState : IState<Fish>
    {
        private bool _animOver;

        public void OnInit(Fish owner, IStateMachine<Fish> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public async void OnEnter(Fish owner, IStateMachine<Fish> stateMachine)
        {
            _animOver = false;
            float y = owner.transform.position.y;
            await owner.transform.DOLocalMoveY(y + 1f, 0.5f);
            await owner.transform.DOMoveY(y, 0.5f);

            // 沿着轨迹移动
            var list = GameMgr.Singleton.fishJumpTrailLst;
            for (int i = 0; i < list.Count; i++)
            {
                Vector3 pos = list[i].transform.position;
                Vector3 dir = pos - owner.transform.position;
                while (Vector3.Distance(owner.transform.position, pos) > 0.1f)
                {
                    owner.transform.position += dir * Time.deltaTime;
                    await UniTask.DelayFrame(1, cancellationToken: owner.destroyCancellationToken);
                }

                await UniTask.DelayFrame(1, cancellationToken: owner.destroyCancellationToken);
            }

            _animOver = true;
        }

        public void Transition(Fish owner, IStateMachine<Fish> stateMachine)
        {
            if (_animOver)
            {
                GameMgr.Singleton.DeSpawn(owner);
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