using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityToolkit;

namespace Game.Flow
{
    public class GameHomeState : IState<GameFlow>
    {
        public GameObject bindedGameObject;
        public bool entered { get; private set; } = false;
        private bool loading = false;

        public void OnInit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public async void OnEnter(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            entered = false;

            if (loading) return;
            await UniTask.DelayFrame(1);
            loading = true;
            Addressables.LoadSceneAsync(Global.Get<GameConfig>().homeScene).Completed += operation =>
            {
                // Global.Get<AudioSystem>().PlayBGM(FMODName.Event.BGM_game_home, out _);
                Addressables.LoadAssetAsync<GameObject>(Global.Get<GameConfig>().homeMgrPrefab).Completed +=
                    operation =>
                    {
                        GameLogger.Log.Debug("GameHomeState OnEnter");
                        bindedGameObject = Object.Instantiate(operation.Result);
                        entered = true;
                        loading = false;
                    };
            };
        }

        public void Transition(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            // stateMachine.Change<GamePlayState>();
        }

        public void OnUpdate(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            if (!entered) return;
            // throw new System.NotImplementedException();
            Assert.IsNotNull(bindedGameObject);
        }

        public void OnExit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            if (!entered) return;
            GameLogger.Log.Debug("GameHomeState OnExit");
            if (bindedGameObject != null)
            {
                Object.Destroy(bindedGameObject);
            }
        }
    }
}