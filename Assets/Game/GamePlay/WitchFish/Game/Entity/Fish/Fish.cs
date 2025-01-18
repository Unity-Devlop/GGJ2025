using System;
using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityToolkit;

namespace WitchFish
{
    public class Fish : MonoBehaviour
    {
        public float spawnWaitTime = 0.5f;
        public float moveSpeed = 1f;
        public float beginAngryWaitTime = 0.5f;
        public float maxWaitTime = 3f;
        public TMP_Text debugText;
        public StateMachine<Fish> stateMachine { get; private set; }
        public Rigidbody2D rb2D { get; private set; }

        public List<ItemType> needList;
        
        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
            
            stateMachine = new StateMachine<Fish>(this);

            stateMachine.Add<FishSpawnState>();
            stateMachine.Add<FishMoveToWaitState>();
            stateMachine.Add<FishLandWaitState>();
            stateMachine.Add<FishLandReturnState>();
            stateMachine.Add<FishMoveToJumpState>();
            stateMachine.Add<FishJumpState>();
            stateMachine.Add<FishLakeReturnState>();
        }

        public RaycastHit2D[] hit2Ds;
        public Vector2 jumpForce = new Vector2(-5, 5);


        private void Update()
        {
            // hit2D = Physics2D.Raycast(transform.position, new Vector2(-1, 0), 1);
            hit2Ds = Physics2D.RaycastAll(transform.position, GameMgr.Singleton.direction,
                GameMgr.Singleton.rayDistance);
#if UNITY_EDITOR
            Debug.DrawRay(transform.position, GameMgr.Singleton.direction, Color.red);
#endif
            if (stateMachine.running)
            {
                debugText.text = stateMachine.currentState.GetType().Name.Replace("State", "");
                stateMachine.OnUpdate();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent(out Item item)){
                GameLogger.Log.Information("{item}", item.ToString());
            }
        }
        
        

        private void OnDisable()
        {
            if (stateMachine.running)
            {
                stateMachine.Stop();
            }
        }

        [Sirenix.OdinInspector.Button]
        public void OnFeed()
        {
            // TODO 喂东西给我
            if (stateMachine.currentState is FishLandWaitState)
            {
#if UNITY_EDITOR
                transform.SetAsLastSibling();
#endif
                stateMachine.Change<FishMoveToJumpState>();
            }
        }
    }
}