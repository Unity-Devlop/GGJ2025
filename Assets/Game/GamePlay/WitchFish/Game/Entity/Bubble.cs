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

        private Sprite itemIcon;


        [Header("图")]
        public Sprite _向日葵;
        public Sprite _核弹;
        public Sprite _破皮鞋;
        public Sprite _章鱼;
        public Sprite _蟹黄堡;
        public Sprite _语音;



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

        public void SetItemType(ItemEnum itemEnum)
        {
            Type = itemEnum;
            itemIcon = itemEnum switch  
            {
                ItemEnum.向日葵=>_向日葵,
                ItemEnum.核弹=> _核弹,
                ItemEnum.破皮鞋=>_破皮鞋,
                ItemEnum.章鱼=> _章鱼,
                ItemEnum.蟹黄堡=> _蟹黄堡,
                ItemEnum.语音=> _语音,
            };
            transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = itemIcon;
        }



        private void OnMouseDown()
        {
            Explore();
        }

    }

}
