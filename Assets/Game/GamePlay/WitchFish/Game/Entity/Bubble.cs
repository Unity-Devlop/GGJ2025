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
        public Transform target;

        private Sprite itemIcon;

        



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
            if (Type == ItemEnum.空白 )
            {

            }else if(Type == ItemEnum.语音)
            {

            }
            else
            {
                var item = Instantiate(GameMgr.Singleton.itemPrefab, target.transform.position , Quaternion.identity);
                // item.transform.SetParent(GameMgr.Singleton.basket);
                item.GetComponent<Item>().Bind(Type);
            }
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

        public void SetItemType(ItemEnum itemEnum)
        {
            Type = itemEnum;
            itemIcon = GameMgr.Singleton.id2Sprite[itemEnum];
            transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = itemIcon;
        }



        private void OnMouseDown()
        {
            Explore();
            if(Type == ItemEnum.语音)
            {
                PlayVideo();
            }
        }

        void PlayVideo()
        {
            // play
            GameMgr.Singleton.PlayVideoIndex++;
        }

    }

}
