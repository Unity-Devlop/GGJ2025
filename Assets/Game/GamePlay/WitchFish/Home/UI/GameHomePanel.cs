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
        [SerializeField ] private Button _devListBtn;
        [SerializeField ] private Button _exitBtn;
        [SerializeField ] private GameObject _devListPanel;
        private void Awake()
        {
            _startGameButton.onClick.AddListener(OnStartGame);
            _devListBtn.onClick.AddListener(OnDevList);
            _exitBtn.onClick.AddListener(OnExit);
        }

        private void OnStartGame()
        {
            Global.Get<GameFlow>().Change<GamePlayState>();
        }

        public void OnExit()
        {

           // Application.Quit();
        }



        public void OnDevList()
        {
            if (_devListPanel.activeInHierarchy)
            {
                _devListPanel.gameObject.SetActive(false);

            }
            else
            {
                _devListPanel.gameObject.SetActive(true);

            }
        }
    }
}