using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class BubbleMgr : MonoSingleton<BubbleMgr>
    {
        public GameObject bubblePrefab;
        public BoxCollider2D RectLakeBottom;
        bool gameIsOn = true;
        [Header("气泡生成")]
        public float BubbleCreateInterval = 5;
        public float ChatBubbleCreateInterval = 4;
        public int BubbleCreateCount = 1;
        [Header("气泡上升")]
        public float bubbleSpeed = 1f;

        public float maxTop = 8.0f;

        public float jackpotChance = 20;


        // 使用列表来存储所有的加权值
        List<WeightedValue> weightedValues = new List<WeightedValue>();

        // Start is called before the first frame update
        void Start()
        {
            InitRandomMap();
            StartCoroutine(CreateBubble(BubbleCreateInterval));
            StartCoroutine(CreateChatBubble(ChatBubbleCreateInterval));
        }

        // Update is called once per frame
        void Update()
        {

        }
        IEnumerator CreateBubble(float BubbleCreateInterval)
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

        IEnumerator CreateChatBubble(float ChatBubbleInterval)
        {
            while (gameIsOn) // 无限循环
            {

                // 实例化气泡

                InstantiateChatBubble();
               

                // 等待指定的秒数
                yield return new WaitForSeconds(ChatBubbleInterval);
            }
        }

        void InstantiateChatBubble()
        {
            var bubbleItem = RandomPosCreateBubbleItem();
            int randomValue = Random.Range(0,100);
            if (randomValue < jackpotChance)
            {
                bubbleItem.SetItemType(ItemEnum.语音);
                jackpotChance = 20;
            }
            else
            {
                jackpotChance += 5;
                bubbleItem.SetItemType(ItemEnum.空白);
            }
        }

        void InstantiateBubble()
        {
            var bubbleItem = RandomPosCreateBubbleItem();
            // 获取一个随机值
            ItemEnum randomValue = GetRandomWeightedValue();
            bubbleItem.SetItemType(randomValue);
        }

        Bubble RandomPosCreateBubbleItem()
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
            return bubbleItem;
        }

        void InitRandomMap()
        {
            // 示例：添加一些加权值
            weightedValues.Add(new WeightedValue { value = ItemEnum.蟹黄堡, weight = 25 });
            weightedValues.Add(new WeightedValue { value = ItemEnum.破皮鞋, weight = 25 });
            weightedValues.Add(new WeightedValue { value = ItemEnum.章鱼, weight = 20 });
            weightedValues.Add(new WeightedValue { value = ItemEnum.向日葵, weight = 20 });
            weightedValues.Add(new WeightedValue { value = ItemEnum.核弹, weight = 10 });


        }

        public ItemEnum GetRandomWeightedValue()
        {
            // 计算总权重
            float totalWeight = 0f;
            foreach (var item in weightedValues)
            {
                totalWeight += item.weight;
            }

            // 生成一个0到总权重之间的随机数
            float randomWeight = Random.Range(0, totalWeight);

            // 遍历加权值，找到随机数落入的区间
            float cumulativeWeight = 0f;
            foreach (var item in weightedValues)
            {
                cumulativeWeight += item.weight;
                if (randomWeight <= cumulativeWeight)
                {
                    return item.value;
                }
            }

            // 如果没有找到，返回默认值（理论上不应该发生）
            return ItemEnum.蟹黄堡;
        }

    }

    public struct WeightedValue
    {
        public ItemEnum value;
        public float weight;
    }

}


