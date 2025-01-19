using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

namespace WitchFish.UI
{
    [RequireComponent(typeof(Canvas))]
    public class FishNeedListUI : MonoBehaviour
    {
        public GameObject iconPrefab;

        [Sirenix.OdinInspector.ReadOnly, Sirenix.OdinInspector.ShowInInspector]
        private List<ItemIconUI> _itemIconUis;

        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _itemIconUis = new List<ItemIconUI>();
        }

        private Fish _fish;

        public void Bind(Fish fish)
        {
            this._fish = fish;
            // fish.OnAdd += OnAdd;
            // fish.OnRemove += OnRemove;
        }

        private void Update()
        {
            if (_fish.stateMachine.currentState is not FishLakeWaitState &&
                _fish.stateMachine.currentState is not FishLandWaitState)
            {
                _canvas.enabled = false;
            }
            else
            {
                if (_fish.needList.Count == 0)
                {
                    _canvas.enabled = false;
                }
                else
                {
                    _canvas.enabled = true;
                }
            }

            while (_fish.needList.Count > _itemIconUis.Count)
            {
                var ui = Instantiate(iconPrefab, transform).GetComponent<ItemIconUI>();
                _itemIconUis.Add(ui);
            }

            while (_itemIconUis.Count > _fish.needList.Count)
            {
                var last = _itemIconUis.Count - 1;
                var lastUI = _itemIconUis[last];
                _itemIconUis.Remove(lastUI);
                Destroy(lastUI.gameObject);
            }


            for (int i = 0; i < _fish.needList.Count; i++)
            {
                var need = _fish.needList[i];
                var ui = _itemIconUis[i];
                ui.Bind(need);
            }
        }

        // private void OnAdd(ItemEnum obj)
        // {
        //     var icon = Instantiate(iconPrefab, transform).GetComponent<ItemIconUI>();
        //     icon.Bind(obj);
        //     _itemIconUis.Add(icon);
        // }
        //
        // private void OnRemove(ItemEnum obj)
        // {
        //     int index = _itemIconUis.FindIndex(icon => icon.id == obj);
        //     if (index == -1)
        //     {
        //         return;
        //     }
        //     GameObject.Destroy(_itemIconUis[index].gameObject);
        //     _itemIconUis.RemoveAt(index);
        // }
    }
}