using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using WitchFish;
using Random = UnityEngine.Random;

namespace WitchFish
{
    public class Soap : MonoBehaviour
    {
        public  bool isCleaningFish = false;
        public Vector3 initPos = new Vector3(0,0,0);
        public Collider2D TargetFish = null;
        public string colliderName = string.Empty;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame  
        void Update()
        {

        }

        public int GetRandomExcluding(int min, int max, List<ItemEnum> exclude)
        {
            int randomValue = min;
            while (!exclude.Contains( (ItemEnum)randomValue) )
            {
                randomValue = Random.Range(min, max + 1);
            }
            return randomValue;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.name == "SoapCollider")
            {
                isCleaningFish = true;
                initPos = transform.position;
                TargetFish = other;
            }

  

        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.name != "SoapCollider" || TargetFish != collision)
            {
                return;
            }
            // 第二次检测到，且没变targetfish，就比较位置，变化够大就完成一次搓

            float verticalMovement = transform.position.y - initPos.y;
            var relavite = Mathf.Abs(verticalMovement);

            if (relavite > 0.3f)
            {
                Debug.LogError($"搓一次");
                initPos = transform.position;
                // todo fsx
                var fish = collision.transform.parent.GetComponent<Fish>();
                var lastneedList = new List<ItemEnum>();
                lastneedList.AddRange(fish.needList);
                for (int i = 0; i < lastneedList.Count; i++)
                {
                    fish.needList.Remove(lastneedList[i]);
                    fish.needList.Add((ItemEnum)GetRandomExcluding((int)ItemEnum.蟹黄堡, (int)ItemEnum.核弹, lastneedList));
                }
            }
        }

        void OnMouseDrag()
        {
            Vector3 mousePos = Input.mousePosition;
            //将当前物体位置转换为屏幕坐标并赋值给鼠标位置，保证物体深度不会发生变化
            mousePos.z = Global.cameraSystem.mainCamera.WorldToScreenPoint(transform.position).z;
            //将鼠标位置转化为世界坐标
            Vector3 objectPos = Global.cameraSystem.mainCamera.ScreenToWorldPoint(mousePos);
            //限制该世界坐标高度不能小于初始高度
            objectPos.y = Math.Clamp(objectPos.y, GameMgr.Singleton.minY, GameMgr.Singleton.maxY);
            //限制该世界坐标深度为物体初始深度
            objectPos.z = 0;
            //给物体赋值坐标
            transform.position = objectPos;


        }
    }

}