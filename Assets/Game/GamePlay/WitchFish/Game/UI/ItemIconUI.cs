using System;
using UnityEngine;
using UnityEngine.UI;

namespace WitchFish.UI
{
    public class ItemIconUI : MonoBehaviour
    {
        [Sirenix.OdinInspector.ReadOnly, Sirenix.OdinInspector.ShowInInspector]
        public ItemEnum id { get; private set; }

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void Bind(ItemEnum itemEnum)
        {
            _image.sprite = GameMgr.Singleton.id2Sprite[itemEnum];
        }
    }
}