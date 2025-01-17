using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityToolkit;

namespace UnityToolkit
{
    [AddComponentMenu("")]
    public class UICard : Selectable, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private const float MoveSpeedLimit = 100f;
        public event Action<UICard> OnEndDragEvent = delegate { };

        public event Action<UICard> OnBeginDragEvent = delegate { };

        // 需要锚定到的位置
        public RectTransform slot { get; protected set; }
        private RectTransform _transform;
        private Canvas _canvas;

        // Config
        [SerializeField] private Vector3 offset;

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public bool dragging { get; private set; }

        public RectTransform rectTransform { get; private set; }

        private bool _autoReset = true;


        protected override void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            base.Awake();
            _canvas = GetComponentInParent<Canvas>();

            _transform = GetComponent<RectTransform>();
        }

        public void SetSlot(RectTransform slot)
        {
            this.slot = slot;
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        private void Update()
        {
            if (dragging)
            {
                Vector3 mousePosition = Input.mousePosition;
                Vector2 targetPosition = UIRoot.Singleton.UICamera.ScreenToWorldPoint(mousePosition) - offset;
                Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
                Vector2 velocity = direction * Mathf.Min(MoveSpeedLimit,
                    Vector2.Distance(transform.position, targetPosition) / Time.deltaTime);
                transform.Translate(velocity * Time.deltaTime);
                ClampPosition(); // 限制位置 不能超出屏幕
                return;
            }

            Assert.IsFalse(dragging);

            if (_autoReset)
            {
                _transform.anchoredPosition = Vector2.zero;
            }
        }

        //
        private void ClampPosition()
        {
            Vector2 screenBounds =
                UIRoot.Singleton.UICamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x, screenBounds.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y, screenBounds.y);
            float z = transform.position.z;
            transform.position = new Vector3(clampedPosition.x, clampedPosition.y, z);
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDragEvent(this);
            _canvas.overrideSorting = true;
            dragging = true;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragEvent(this);
            _canvas.overrideSorting = false;
            dragging = false;
        }
    }
}