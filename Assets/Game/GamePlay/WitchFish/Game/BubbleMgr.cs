using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMgr : MonoBehaviour
{
    public GameObject bubblePrefab;
    public BoxCollider2D RectLakeBottom;
    bool gameIsOn = true;
    public float BubbleCreatePerSec = 5;
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
            Bounds bounds = RectLakeBottom.bounds;

            // 在边界范围内随机选择一个位置
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            // 实例化气泡
            var item = Instantiate(bubblePrefab, transform.position+ new  Vector3(randomX, randomY, 0), Quaternion.identity);
            item.GetComponent<Bubble>().Enabled = true;

            // 等待指定的秒数
            yield return new WaitForSeconds(BubbleCreatePerSec);
        }
    }


}
