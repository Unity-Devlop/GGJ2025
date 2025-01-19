using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class BubbleMgr : MonoSingleton<BubbleMgr>
    {
        public GameObject bubblePrefab;
        public GameObject chatBubblePrefab;
        public Transform rect;
        bool gameIsOn = true;
        [Header("气泡生成")] public float BubbleCreateInterval = 5;
        public float ChatBubbleCreateInterval = 4;
        public int BubbleCreateCount = 1;
        [Header("气泡上升")] public float bubbleSpeed = 1f;

        public float maxTop = 8.0f;

        public float jackpotChance = 100;

        [SerializeField] private BubbleConfig config;

        private List<BoxCollider2D> _boxColliders = new List<BoxCollider2D>();

        // 使用列表来存储所有的加权值
        // List<WeightedValue> weightedValues = new List<WeightedValue>();
        protected override void OnInit()
        {
            _boxColliders = rect.GetComponentsInChildren<BoxCollider2D>().ToList();
        }

        // Start is called before the first frame update
        void Start()
        {
            InitRandomMap();
            StartCoroutine(CreateBubble());
            StartCoroutine(CreateChatBubble());
        }


        void SetBubbleSettinsByFishCountInLake()
        {
            var fishCount = GameMgr.Singleton.lakeFishCount;
            if (fishCount < 6)
            {
                BubbleCreateCount = 1;
                BubbleCreateInterval = 2;
            }
            else if (fishCount < 16)
            {
                BubbleCreateCount = 1;
                BubbleCreateInterval = 1;
            }
            else if (fishCount < 31)
            {
                BubbleCreateCount = 2;
            }
            else if (fishCount < 51)
            {
                BubbleCreateCount = 3;
            }
            else
            {
                BubbleCreateCount = 4;
            }
        }

        IEnumerator CreateBubble()
        {
            while (gameIsOn) // 无限循环
            {
                SetBubbleSettinsByFishCountInLake();
                // 实例化气泡
                for (int i = 0; i < BubbleCreateCount; i++)
                {
                    InstantiateBubble();
                }


                // 等待指定的秒数
                yield return new WaitForSeconds(BubbleCreateInterval);
            }
        }

        IEnumerator CreateChatBubble()
        {
            while (gameIsOn) // 无限循环
            {
                // 等待指定的秒数
                yield return new WaitForSeconds(ChatBubbleCreateInterval);
                // 实例化气泡
                if (UnityEngine.Random.value < 0.3)
                {
                    InstantiateChatBubble();
                }
            }
        }

        void InstantiateChatBubble()
        {
            GameLogger.Log.Information("聊天气泡");
            var bubbleItem = RandomPosCreateChatBubbleItem();
            Debug.Log(bubbleItem.gameObject);
            bubbleItem.SetItemType(ItemEnum.语音);
            int randomValue = Random.Range(0, 100);
            //if (randomValue < jackpotChance)
            //{
            //    jackpotChance = 20;
            //}
            // else
            // {
            // jackpotChance += 5;
            // bubbleItem.SetItemType(ItemEnum.空白);
            // }
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
            var boxCollider2D = _boxColliders.RandomTakeWithoutRemove();

            Bounds bounds = boxCollider2D.bounds;

            // 在边界范围内随机选择一个位置
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);

            var item = Instantiate(bubblePrefab, transform.position + new Vector3(randomX, randomY, 0),
                Quaternion.identity);
            var bubbleItem = item.GetComponent<Bubble>();
            bubbleItem.speed = bubbleSpeed;
            bubbleItem.maxTop = maxTop;
            bubbleItem.Enabled = true;
            return bubbleItem;
        }


        Bubble RandomPosCreateChatBubbleItem()
        {
            var boxCollider2D = _boxColliders.RandomTakeWithoutRemove();

            Bounds bounds = boxCollider2D.bounds;

            // 在边界范围内随机选择一个位置
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);

            var item = Instantiate(chatBubblePrefab, transform.position + new Vector3(randomX, randomY, 0),
                Quaternion.identity);
            var bubbleItem = item.GetComponent<Bubble>();
            bubbleItem.speed = bubbleSpeed;
            bubbleItem.maxTop = maxTop;
            bubbleItem.Enabled = true;
            return bubbleItem;
        }

        void InitRandomMap()
        {
            // 示例：添加一些加权值
            // weightedValues.Add(new WeightedValue { value = ItemEnum.蟹黄堡, weight = 25 });
            // weightedValues.Add(new WeightedValue { value = ItemEnum.破皮鞋, weight = 25 });
            // weightedValues.Add(new WeightedValue { value = ItemEnum.章鱼, weight = 20 });
            // weightedValues.Add(new WeightedValue { value = ItemEnum.向日葵, weight = 20 });
            // weightedValues.Add(new WeightedValue { value = ItemEnum.核弹, weight = 10 });
        }

        public ItemEnum GetRandomWeightedValue()
        {
            return config.randomList.RandomTakeWithoutRemove();

            // // 计算总权重
            // float totalWeight = 0f;
            // foreach (var item in weightedValues)
            // {
            //     totalWeight += item.weight;
            // }
            //
            // // 生成一个0到总权重之间的随机数
            // float randomWeight = Random.Range(0, totalWeight);
            //
            // // 遍历加权值，找到随机数落入的区间
            // float cumulativeWeight = 0f;
            // foreach (var item in weightedValues)
            // {
            //     cumulativeWeight += item.weight;
            //     if (randomWeight <= cumulativeWeight)
            //     {
            //         return item.value;
            //     }
            // }
            //
            // // 如果没有找到，返回默认值（理论上不应该发生）
            // return ItemEnum.蟹黄堡;
        }
    }

    // public struct WeightedValue
    // {
    //     public ItemEnum value;
    //     public float weight;
    // }
}