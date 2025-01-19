using System;
using Cysharp.Threading.Tasks;
using FMOD;
using FMODUnity;
using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class FishLandWaitState : IState<Fish>
    {
        private float _timer;

        private bool _animating;
        private bool _animatingOver;

        public void OnInit(Fish owner, IStateMachine<Fish> stateMachine)
        {
        }

        public void OnEnter(Fish owner, IStateMachine<Fish> stateMachine)
        {
            _animating = false;
            _animatingOver = false;
            _timer = 0;
        }

        public void Transition(Fish owner, IStateMachine<Fish> stateMachine)
        {
            _timer += Time.deltaTime;
            if (_animating)
            {
                if (_animatingOver)
                {
                    stateMachine.Change<FishMoveToJumpState>();
                }

                return;
            }

            if (owner.needList.Count == 0)
            {
                
                owner.angry.SetActive(false);
                owner.eye.sprite = owner.normalEyeSprite;
                _animating = true;
                _animatingOver = false;
                owner.animator.Play("yuzuiAnim");
                UniTask.Delay(TimeSpan.FromSeconds(0.6f)).ContinueWith(() => { _animatingOver = true; });
            }
            else if (_timer > owner.maxWaitTime)
            {
                RuntimeManager.PlayOneShotAttached(FMODName.Event.SFX_SoundEffect_3___, owner.gameObject);
                stateMachine.Change<FishLandReturnState>();
            }
            else if (_timer >= owner.beginAngryWaitTime)
            {
                if (_timer >= owner.beginAngryWaitTime + (owner.maxWaitTime - owner.beginAngryWaitTime) * 0.5)
                {
                    owner.angry.SetActive(true);
                }
                // TODO 生气
                owner.eye.sprite = owner.angryEyeSprite;
            }
        }

        public void OnUpdate(Fish owner, IStateMachine<Fish> stateMachine)
        {
            // 没有到最后的目的地
        }

        public void OnExit(Fish owner, IStateMachine<Fish> stateMachine)
        {
            if (GameMgr.SingletonNullable != null)
            {
            }
        }
    }
}