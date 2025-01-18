using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityToolkit;

namespace Game.Flow
{
    public class GameFlow : MonoBehaviour, ISystem, IOnInit
    {
        private StateMachine<GameFlow> _stateMachine;
        private Blackboard _blackboard;

#if UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly] [NonSerialized]
        private string _currentState;
#endif
        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        [field: NonSerialized]
        public IState<GameFlow> currentState { get; private set; }

        public bool running => _stateMachine.running;


        public void OnInit()
        {
            _blackboard = new Blackboard();
            _stateMachine = new StateMachine<GameFlow>(this, _blackboard);

            _stateMachine.Add<DummyState>();
            _stateMachine.Add<GameEntryState>();
            _stateMachine.Add<GameHomeState>();
            _stateMachine.Add<GamePlayState>();
        }

        public void Run()
        {
            _stateMachine.Run<GameEntryState>();
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }

        private void Update()
        {
            if (!running) return;
#if UNITY_EDITOR
            currentState = _stateMachine.currentState;
            _currentState = _stateMachine.currentState.GetType().Name;
#endif
            _stateMachine.OnUpdate();
        }

        [Sirenix.OdinInspector.Button]
        private void ToGameHome()
        {
            _stateMachine.Change<GameHomeState>();
        }

        public void Change<T>() where T : IState<GameFlow>
        {
            if(_stateMachine.currentState is T )return;
            _stateMachine.Change<T>();
        }

        #region Fast Access

        public static void To<T>() where T : IState<GameFlow>
        {
            if (Global.SingletonNullable == null) return;
            Global.Get<GameFlow>()._stateMachine.Change<T>();
        }

        public static void SetParameter<T>(string key, T value)
        {
            if (Global.SingletonNullable == null) return;
            Global.Get<GameFlow>()._blackboard.Set(key, value);
        }

        public static void GetParameter<T>(string key, out T value)
        {
            if (Global.SingletonNullable == null)
            {
                value = default;
                return;
            }

            value = Global.Get<GameFlow>()._blackboard.Get<T>(key);
        }

        public static void ClearParameter(string key)
        {
            if (Global.SingletonNullable == null) return;
            Global.Get<GameFlow>()._blackboard.Remove(key);
        }

        public static bool ContainsParameter(string key)
        {
            if (Global.SingletonNullable == null) return false;
            return Global.Get<GameFlow>()._blackboard.Contains(key);
        }

        #endregion
    }
}