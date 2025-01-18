using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game;
using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class FishReturnLakeState : IState<Fish>
    {
        private bool _animOver;

        public void OnInit(Fish owner, IStateMachine<Fish> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public async void OnEnter(Fish owner, IStateMachine<Fish> stateMachine)
        {
            _animOver = false;
            // 跳一下
            owner.rb2D.isKinematic = false;
            owner.rb2D.AddForce(owner.jumpForceList.RandomTakeWithoutRemove(), ForceMode2D.Impulse);

            await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: owner.destroyCancellationToken);

            _animOver = true;
        }

        public void Transition(Fish owner, IStateMachine<Fish> stateMachine)
        {
            if (_animOver)
            {
                GameMgr.Singleton.DeSpawnLake(owner);
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