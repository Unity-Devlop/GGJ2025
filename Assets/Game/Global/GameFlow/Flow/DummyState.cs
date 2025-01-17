using UnityToolkit;

namespace Game.Flow
{
    public class DummyState: IState<GameFlow>
    {
        public void OnInit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
        }

        public void OnEnter(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
        }

        public void Transition(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
        }

        public void OnUpdate(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
        }

        public void OnExit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
        }
    }
}