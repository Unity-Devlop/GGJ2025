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
        [SerializeField ] private Button _maskBtn;
        [SerializeField ] private Button _exitBtn;
        [SerializeField ] private GameObject _devListPanel;
        [SerializeField ] private GameObject _tutorialPanel;
        private void Awake()
        {
            _maskBtn.gameObject.SetActive(false);
            _startGameButton.onClick.AddListener(OnStartGame);
            _devListBtn.onClick.AddListener(OnDevList);
            _exitBtn.onClick.AddListener(OnExit);
            _maskBtn.onClick.AddListener(OnMask);
        }

        private void OnStartGame()
        {
            Global.Get<GameFlow>().Change<GamePlayState>();
        }

        public void OnExit()
        {
            if (_tutorialPanel.activeInHierarchy)
            {
                _devListPanel.gameObject.SetActive(false);

            }
            else
            {
                _tutorialPanel.gameObject.SetActive(true);
                _maskBtn.gameObject.SetActive(true);
            }
            // Application.Quit();
        }

        public void OnMask()
        {
            _devListPanel.gameObject.SetActive(false);
            _tutorialPanel.gameObject.SetActive(false);
            _maskBtn.gameObject.SetActive(false);
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
                _maskBtn.gameObject.SetActive(true);
            }
        }
    }
}