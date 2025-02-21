using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class FishLakeDeadState : IState<Fish>
    {
        public void OnInit(Fish owner, IStateMachine<Fish> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public async void OnEnter(Fish owner, IStateMachine<Fish> stateMachine)
        {
            Vector3 vec = new Vector3(0, -5, 0);
            owner.transform.DOLocalMoveY(owner.transform.position.y + vec.y, .2f).SetEase(Ease.Linear);

            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            GameMgr.Singleton.DeSpawnLake(owner);
            Core.Event.Send<EventFishDiePush>();
            Core.Event.Send<EventFishDieInLakePush>();
        }

        public void Transition(Fish owner, IStateMachine<Fish> stateMachine)
        {
        }

        public void OnUpdate(Fish owner, IStateMachine<Fish> stateMachine)
        {
        }

        public void OnExit(Fish owner, IStateMachine<Fish> stateMachine)
        {
            // throw new System.NotImplementedException();
        }
    }
}