using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class FishMoveToJumpState: IState<Fish>
    {
        Vector3 _targetPos;
        public void OnInit(Fish owner, IStateMachine<Fish> stateMachine)
        {
        }

        public void OnEnter(Fish owner, IStateMachine<Fish> stateMachine)
        {
            owner.moveSFX.Play();
            owner.animator.Play("FishIdle");
            _targetPos = GameMgr.Singleton.GetJumpPosition();
        }

        public void Transition(Fish owner, IStateMachine<Fish> stateMachine)
        {
            if (GameMgr.Singleton.CloseToJumpPoint(owner))
            {
                stateMachine.Change<FishJumpState>();
            }
        }

        public void OnUpdate(Fish owner, IStateMachine<Fish> stateMachine)
        {
            if (!owner.moveSFX.IsPlaying())
            {
                owner.moveSFX.Play();
            }
            Vector3 diff = _targetPos - owner.transform.position;
            Vector3 vec = diff.normalized * (owner.moveSpeed * Time.deltaTime);
            owner.transform.Translate(vec, Space.World);
        }

        public void OnExit(Fish owner, IStateMachine<Fish> stateMachine)
        {
            
            owner.moveSFX.Stop();
        }
    }
}