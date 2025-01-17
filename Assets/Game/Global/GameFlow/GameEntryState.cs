using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityToolkit;
using Object = UnityEngine.Object;

namespace Game.Flow
{
    [Serializable]
    public class GameEntryState : IState<GameFlow>
    {
        private CancellationTokenSource _cancellationTokenSource;
        private bool _loginSuccess;
        private GameObject _bindObject;
#if UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
#endif
        private IGameEntry _gameEntry;

        public void OnInit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            // throw new System.NotImplementedException();
        }


        public void OnEnter(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            _loginSuccess = true;
            // _loginSuccess = false;
            _cancellationTokenSource = new CancellationTokenSource();
            // LoginTask().Forget();
            var prefab = Addressables.LoadAssetAsync<GameObject>(Global.Config.gameEntry).WaitForCompletion();
            _bindObject = Object.Instantiate(prefab);
            bool assert = _bindObject.TryGetComponent(out _gameEntry);
            Assert.IsTrue(assert);
            _gameEntry.OnInit();
        }

        // private async UniTask LoginTask()
        // {
        //     try
        //     {
        //         await UniTask.WaitUntil(() => Global.RPC.connected, cancellationToken: _cancellationTokenSource.Token);
        //         Error error = await Global.RPC.globalHub.Login(Global.DeviceAddress);
        //         _loginSuccess = error.code == StatusCode.Success;
        //     }
        //     catch (ObjectDisposedException e)
        //     {
        //         Global.Warning($"登陆任务被Dispose了{e}");
        //     }
        // }

        public void Transition(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            if (_gameEntry == null) return;
            if (!_gameEntry.initialized) return;
            if (_loginSuccess)
            {
                stateMachine.Change<GameHomeState>();
            }
        }

        public void OnUpdate(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
        }

        public void OnExit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _gameEntry = null;
            Object.Destroy(_bindObject);
        }
    }
}