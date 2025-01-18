using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WitchFish
{
    public class Bubble : MonoBehaviour
    {
        public ItemEnum Type;
        public float speed = 1f;
       
        public float maxTop = 8.0f;
        public bool Enabled = false;
        public GameObject Item;
        public Vector3 TargetPos = new Vector3(-7.29f,3.58f,0);

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        void Explore()
        {
            var item = Instantiate(Item, TargetPos , Quaternion.identity);
            Destroy(gameObject);
        }

        private void Move()
        {
            if (!Enabled) return;
            if (transform.position.y > maxTop)
            {
                Destroy(gameObject);
            }

            var deltaTime = Time.deltaTime;
            var direction = Vector3.up;
            transform.Translate(deltaTime * speed * direction);
        }

        private void OnMouseDown()
        {
            Explore();
        }

    }

}
