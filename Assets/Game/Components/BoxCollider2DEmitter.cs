using System;
using UnityEngine;

namespace Game.GamePlay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxCollider2DEmitter : MonoBehaviour
    {
        public event Action<Collider2D> TriggerEnter2DEvent = delegate { };
        public event Action<Collider2D> TriggerExit2DEvent = delegate { };
        public event Action<Collider2D> TriggerStay2DEvent = delegate { };

        [field: SerializeField] public LayerMask collideMask { get; private set; }

        public BoxCollider2D boxCollider2D { get; private set; }

        private void Awake()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (collideMask == (collideMask | (1 << other.gameObject.layer)))
                TriggerEnter2DEvent(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (collideMask == (collideMask | (1 << other.gameObject.layer)))
                TriggerExit2DEvent(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (collideMask == (collideMask | (1 << other.gameObject.layer)))
                TriggerStay2DEvent(other);
        }
    }
}