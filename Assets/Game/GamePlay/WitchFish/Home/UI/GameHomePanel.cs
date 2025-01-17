using System;
using Game;
using Game.Flow;
using UnityEngine;
using UnityEngine.UI;
using UnityToolkit;

namespace WitchFish.UI
{
    public class GameHomePanel : UIPanel
    {
        [SerializeField ] private Button _startGameButton;

        private void Awake()
        {
            _startGameButton.onClick.AddListener(OnStartGame);
        }

        private void OnStartGame()
        {
            Global.Get<GameFlow>().Change<GamePlayState>();
        }
    }
}