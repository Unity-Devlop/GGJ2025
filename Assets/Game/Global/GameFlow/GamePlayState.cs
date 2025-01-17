using System;
using Cysharp.Threading.Tasks;
using Game.GamePlay;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityToolkit;
using Object = UnityEngine.Object;

namespace Game.Flow
{
    [Serializable]
    public class GamePlayState : IState<GameFlow>
    {
        [SerializeField] private GameObject bindedGameObject;
        public bool entered { get; private set; } = false;

        public void OnInit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
        }

        public void OnEnter(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            entered = false;
            Addressables.LoadSceneAsync(Global.Get<GameConfig>().playScene).Completed += operation =>
            {
                // Global.Get<AudioSystem>().PlayBGM(FMODName.Event.BGM_game_home, out _);
                Addressables.LoadAssetAsync<GameObject>(Global.Get<GameConfig>().playMgrPrefab).Completed +=
                    operation =>
                    {
                        GameLogger.Log.Information("GamePlayState OnEnter");

                        bindedGameObject = Object.Instantiate(operation.Result);
                        entered = true;
                    };
            };
        }

        public void Transition(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
        }

        public void OnUpdate(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            if (!entered) return;
            Assert.IsNotNull(bindedGameObject);
        }

        public void OnExit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            GameLogger.Log.Information("GamePlayState OnExit");
            if (bindedGameObject != null)
            {
                Object.Destroy(bindedGameObject);
            }
        }
    }
}