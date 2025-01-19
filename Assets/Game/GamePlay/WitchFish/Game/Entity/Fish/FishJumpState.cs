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
            try
            {
                owner.animator.Play("FishIdle");
                owner.rb2D.excludeLayers |= LayerMask.GetMask("Item");
                _animOver = false;
                float y = owner.transform.position.y;
                 owner.transform.DOLocalMoveY(y + 1f, 0.5f);
                 await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                 owner.transform.DOMoveY(y, 0.5f);
                 await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

                owner.rb2D.isKinematic = false;
                owner.rb2D.AddForce(owner.jumpForceList.RandomTakeWithoutRemove(), ForceMode2D.Impulse);
                
                GameMgr.Singleton.EnterLake(owner);
                
            }
            catch (OperationCanceledException)
            {
            }

            _animOver = true;
        }

        public void Transition(Fish owner, IStateMachine<Fish> stateMachine)
        {
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