using System;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityToolkit;

namespace UnityToolkit
{
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    public class UICardContainer : MonoBehaviour
    {
        [Sirenix.OdinInspector.ReadOnly, Sirenix.OdinInspector.ShowInInspector]
        private List<UICard> _cards;

        [SerializeField] private bool autoSizing = true;
        [SerializeField] private float standardWidth = 152;
        [SerializeField] public int maxCount = 8;

        private RectTransform _rectTransform;
        private float _originHeight;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            _originHeight = _rectTransform.sizeDelta.y;
            // 不能有子物体
            _cards = new List<UICard>();
            Assert.IsTrue(transform.childCount == 0);
        }

        private void Update()
        {
            // 根据手牌的数量 动态调整自己的大小
            if (autoSizing)
            {
                int currentCount = _cards.Count;

                float maxWidth = standardWidth * maxCount;
                float width = standardWidth * currentCount;
                width = Mathf.Max(width, maxWidth);
                _rectTransform.sizeDelta = new Vector2(width, _originHeight);
            }
        }

        // public delegate T SpawnAction<T>(UICardContainer container) where T : UICard;


        public delegate void OnAdd(UICard card);

        public delegate void OnRemove(UICard card);

        public event OnAdd OnAddEvent;
        public event OnRemove OnRemoveEvent;

        public UICard Add(UICard card)
        {
            _cards.Add(card);
            OnAddEvent?.Invoke(card);
            return card;
        }

        public void Remove(UICard card)
        {
#if UNITY_EDITOR
            Assert.IsTrue(_cards.Contains(card));
#endif
            card.slot.SetParent(null);
            _cards.Remove(card);
            OnRemoveEvent?.Invoke(card);
        }

// #if UNITY_EDITOR
//
//         private void OnValidate()
//         {
//             _rectTransform = GetComponent<RectTransform>();
//             float targetStandardWidth = _rectTransform.sizeDelta.x / standardCount;
//             standardWidth = targetStandardWidth;
//             _rectTransform.sizeDelta = new Vector2(targetStandardWidth * standardCount, standardHeight);
//         }
//
// #endif
        //
        // public void Remove(UICard card)
        // {
        //     card.pivotPoint.SetParent(null);
        //     _cards.Remove(card);
        // }
// #if UNITY_EDITOR
//         private void OnValidate()
//         {
//             var rectTransform = GetComponent<RectTransform>();
//             var pivot = rectTransform.pivot;
//             if (pivot.x != 0)
//             {
//                 Debug.LogWarning("UICarContainer's pivot.x should be 0");
//             }
//
//             pivot.x = 0;
//             rectTransform.pivot = pivot;
//         }
// #endif
    }
}