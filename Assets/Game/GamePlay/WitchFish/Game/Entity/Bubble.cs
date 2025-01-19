using System.Collections;
using System.Collections.Generic;
using FMODUnity;
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

        Animator animator;
        private void Awake()
        {
            animator = gameObject.GetComponent<Animator>();
        }



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            itemIcon = GameMgr.Singleton.id2Sprite[Type];
            transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = itemIcon;
            Move();
        }

        void Explore()
        {
            if (Type == ItemEnum.空白 )
            {

            }else if(Type == ItemEnum.语音)
            {
                string path = "event:/SFX/生气";
                var instance =  RuntimeManager.CreateInstance(path);
                if (instance.isValid())
                {
                    instance.start();
                }
            }
            else
            {
                var prefab = GameMgr.Singleton.id2ItemPrefab[Type];
                var item = Instantiate(prefab, target.transform.position , Quaternion.identity);
                // item.transform.SetParent(GameMgr.Singleton.basket);
                item.GetComponent<Item>().Bind(Type);
                animator.Play("booow");
               
            }
            Destroy(gameObject,0.2f);
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
