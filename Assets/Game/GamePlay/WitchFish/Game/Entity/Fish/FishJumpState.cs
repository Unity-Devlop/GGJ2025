using System;
using System.Collections.Generic;
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
            await owner.transform.DOLocalMoveY(y + 1f, 0.5f)
                .ToUniTask(cancellationToken: owner.destroyCancellationToken);
            await owner.transform.DOMoveY(y, 0.5f).ToUniTask(cancellationToken: owner.destroyCancellationToken);

            owner.rb2D.isKinematic = false;
            owner.rb2D.AddForce(owner.jumpForceList.RandomTakeWithoutRemove(), ForceMode2D.Impulse);

            await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: owner.destroyCancellationToken);

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