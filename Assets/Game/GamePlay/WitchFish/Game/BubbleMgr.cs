using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WitchFish
{
    public class BubbleMgr : MonoBehaviour
    {
        public GameObject bubblePrefab;
        public BoxCollider2D RectLakeBottom;
        bool gameIsOn = true;
        [Header("气泡生成")]
        public float BubbleCreateInterval = 5;
        public int BubbleCreateCount = 1;
        [Header("气泡上升")]
        public float bubbleSpeed = 1f;

        public float maxTop = 8.0f;


        // Start is called before the first frame update
        void Start()
        {

            StartCoroutine(CreateBubble());
        }

        // Update is called once per frame
        void Update()
        {

        }
        IEnumerator CreateBubble()
        {
            while (gameIsOn) // 无限循环
            {

                // 实例化气泡
                for(int i =0; i< BubbleCreateCount; i++)
                {
                    InstantiateBubble();
                }


                // 等待指定的秒数
                yield return new WaitForSeconds(BubbleCreateInterval);
            }
        }

        void InstantiateBubble()
        {
            Bounds bounds = RectLakeBottom.bounds;

            // 在边界范围内随机选择一个位置
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);

            var item = Instantiate(bubblePrefab, transform.position + new Vector3(randomX, randomY, 0), Quaternion.identity);
            var bubbleItem = item.GetComponent<Bubble>();
            bubbleItem.speed = bubbleSpeed;
            bubbleItem.maxTop = maxTop;
            bubbleItem.Enabled = true;
        }

    }

}


