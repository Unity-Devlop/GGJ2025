using System;
using System.Collections.Generic;
using UnityEngine;

namespace WitchFish.UI
{
    [RequireComponent(typeof(Canvas))]
    public class FishNeedListUI : MonoBehaviour
    {
        public GameObject iconPrefab;
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
            fish.OnAdd += OnAdd;
            fish.OnRemove += OnRemove;
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
                _canvas.enabled = true;
            }
        }

        private void OnAdd(ItemEnum obj)
        {
            var icon = Instantiate(iconPrefab, transform).GetComponent<ItemIconUI>();
            icon.Bind(obj);
            _itemIconUis.Add(icon);
        }

        private void OnRemove(ItemEnum obj)
        {
            int index = _itemIconUis.FindIndex(icon => icon.id == obj);
            GameObject.Destroy(_itemIconUis[index].gameObject);
            _itemIconUis.RemoveAt(index);
        }
    }
}