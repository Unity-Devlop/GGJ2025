using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
namespace WitchFish
{
    public class Menu : MonoBehaviour
    {
        public GameObject Content;
        public GameObject Item;
        // Start is called before the first frame update
        void Start()
        {
            var curIdx = GameMgr.Singleton.PlayVideoIndex;
            var curCount = Content.transform.childCount ;
            for (int i = curCount; i < curIdx; i++)
            {
                var obj = Instantiate(Item, Content.transform);
                var bubbleItem = obj.GetComponent<uiBubbleItem>();
                bubbleItem.SetId(i);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnClickBack()
        {
            Time.timeScale = 1;
            Global.cameraSystem.SetToMainCamera();
            transform.parent.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            Time.timeScale = 0f; // Pause the game
        }
    }

}
