using System;
using System.Collections.Generic;
using System.Diagnostics;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityToolkit;
using WitchFish.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

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

        [SerializeField] private FishTypeEnum type = FishTypeEnum.普通鱼;

        [SerializeField] internal List<ItemEnum> needList;
        public event Action<ItemEnum> OnAdd = delegate { };
        public event Action<ItemEnum> OnRemove = delegate { };

        [NonSerialized] public RaycastHit2D[] hit2Ds;

        public List<Vector2> jumpForceList = new List<Vector2>()
        {
            new Vector2(-5, 5),
            new Vector2(-4, 8),
            new Vector2(-2, 8),
            new Vector2(-3, 6)
        };

        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();

            // var values = Enum.GetValues(typeof(LakeEnum));
            // // List<LakeEnum> lakeEnums = new List<LakeEnum>();
            // foreach (var value in values)
            // {
            //     lakeEnums.Add((LakeEnum)value);
            // }
            //
            // LakeEnum lakeEnum = lakeEnums.RandomTakeWithoutRemove();
            // rb2D.includeLayers = LayerMask.GetMask(lakeEnum.ToString());


            stateMachine = new StateMachine<Fish>(this);

            stateMachine.Add<FishSpawnState>();
            stateMachine.Add<FishMoveToWaitState>();
            stateMachine.Add<FishLandWaitState>();
            stateMachine.Add<FishLandReturnState>();
            stateMachine.Add<FishLakeWaitState>();
            stateMachine.Add<FishReturnLakeState>();
            stateMachine.Add<FishLakeDeadState>();
            stateMachine.Add<FishMoveToJumpState>();
            stateMachine.Add<FishJumpState>();
            GetComponentInChildren<FishNeedListUI>().Bind(this);
        }

        private void Start()
        {
            foreach (var itemEnum in needList)
            {
                OnAdd(itemEnum);
            }
        }


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

        public float waterEffectTime = 0.5f;
        
        [SerializeField]
        private SpriteRenderer _body;
        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Item item))
            {
                OnFeedItem(item);
            }

            if (other.TryGetComponent(out Lake lake))// && stateMachine.currentState is FishJumpState)
            {
                Color color = _body.color;
                color.a = 0.5f;
                _body.color = color;
                
                // GameLogger.Log.Information("{item}", item.ToString());
                // TODO 生成一个水花 
                var prefab = GameMgr.Singleton.waterParticleList.RandomTakeWithoutRemove();
                var effect = Instantiate(prefab, transform.position, Quaternion.identity);
                Destroy(effect, waterEffectTime);
            }
        }

        // public event Action OnFeedOver;

        private void OnFeedItem(Item item)
        {
            if (stateMachine.currentState is FishLandWaitState ||
                stateMachine.currentState is FishLakeWaitState)
            {
                if (needList.Contains(item.id))
                {
                    needList.Remove(item.id);
                    OnRemove?.Invoke(item.id);
                    GameMgr.Singleton.DeSpawn(item);
                }
            }
        }


        private void OnDisable()
        {
            if (stateMachine.running)
            {
                stateMachine.Stop();
            }
        }


#if UNITY_EDITOR


        [Conditional("UNITY_EDITOR")]
        [Sirenix.OdinInspector.Button]
        private void DebugRandomRemove()
        {
            int idx = UnityEngine.Random.Range(0, needList.Count);
            OnRemove?.Invoke(needList[idx]);
            needList.RemoveAt(idx);
        }

        [Conditional("UNITY_EDITOR")]
        [Sirenix.OdinInspector.Button]
        private void DebugFeedToJump()
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

#endif
}