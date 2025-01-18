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
