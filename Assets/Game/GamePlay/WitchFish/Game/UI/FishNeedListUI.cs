using System;
using System.Collections.Generic;
using UnityEngine;

namespace WitchFish.UI
{
    public class FishNeedListUI : MonoBehaviour
    {
        public GameObject iconPrefab;
        private List<ItemIconUI> _itemIconUis;

        private void Awake()
        {
            _itemIconUis = new List<ItemIconUI>();
        }

        public void Bind(Fish fish)
        {
            
            fish.OnAdd += OnAdd;
            fish.OnRemove += OnRemove;
        }

        private void OnAdd(ItemEnum obj)
        {
            var icon = Instantiate(iconPrefab, transform).GetComponent<ItemIconUI>();
            icon.id = obj;
            _itemIconUis.Add(icon);
        }

        private void OnRemove(ItemEnum obj)
        {
            int index = _itemIconUis.FindIndex(icon => icon.id == obj);
            _itemIconUis.RemoveAt(index);
            GameObject.Destroy(_itemIconUis[index].gameObject);
        }
    }
}