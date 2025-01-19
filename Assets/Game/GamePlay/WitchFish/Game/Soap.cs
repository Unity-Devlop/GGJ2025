using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMODUnity;
using Game;
using UnityEngine;
using UnityEngine.Serialization;
using WitchFish;
using Random = UnityEngine.Random;

namespace WitchFish
{
    public class Soap : MonoBehaviour
    {
        [SerializeField] private Transform soapTarget;

        // public bool isCleaningFish = false;
        // 上一次检测到鱼的位置
        private Vector3 _prevHitFishPos = new Vector3(0, 0, 0);

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private Collider2D _prevTarget = null;

        public const string ColliderName = "SoapCollider";
        private int _cuoCount = 0;

        public Vector3 maxInPath;
        public Vector3 minInPath;


        public float radius = 0.5f;
        private bool _dragging = false;

        // private Vector3 _prevHitBigFishPos = new Vector3(0, 0, 0);
        public float returnSpeed = 5f;
        private int sortingOrder = 0;
        private SpriteRenderer _renderer;
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
        }

        private void Update()
        {
            if (_dragging)
            {
                _renderer.sortingOrder = 88;
                var collider2Ds = Physics2D.OverlapCircleAll(transform.position, radius);
                bool hit = false;
                foreach (var collider2D1 in collider2Ds)
                {
                    if (collider2D1.gameObject.name == ColliderName && _prevTarget == null)
                    {
                        _prevTarget = collider2D1;
                        _prevHitFishPos = collider2D1.transform.position;
                        hit = true;
                        break;
                    }

                    if (collider2D1.gameObject.name == ColliderName && _prevTarget != null)
                    {
                        if (collider2D1 != _prevTarget)
                        {
                            _prevTarget = collider2D1;
                            _prevHitFishPos = collider2D1.transform.position;
                            hit = true;
                            break;
                        }

                        hit = true;
                        // 相同表示还是同一只鱼
                        maxInPath.x = MathF.Max(maxInPath.x, transform.position.x);
                        maxInPath.y = MathF.Max(maxInPath.y, transform.position.y);
                        minInPath.x = MathF.Min(minInPath.x, transform.position.x);
                        minInPath.y = MathF.Min(minInPath.y, transform.position.y);
                        if (!(maxInPath.x - minInPath.x > 0.3f) && !(maxInPath.y - minInPath.y > 0.3f)) continue;
                        _prevHitFishPos = transform.position;
                        maxInPath = _prevHitFishPos;
                        minInPath = _prevHitFishPos;
                        _cuoCount++;
                        if (_cuoCount <= 3) continue;
                        var soapTarget = _prevTarget.transform.parent.GetComponent<ISoapTarget>();
                        soapTarget.OnSoup();

                        _cuoCount = 0;
                    }
                }


                if (!hit)
                {
                    _prevTarget = null;
                    _cuoCount = 0;
                    maxInPath = transform.position;
                    _prevHitFishPos = transform.position;
                }
            }
            else
            {
                _renderer.sortingOrder = sortingOrder;
                transform.position = Vector3.Lerp(transform.position, soapTarget.transform.position,
                    Time.deltaTime * returnSpeed);
            }
        }

        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     if (other.gameObject.name == "SoapCollider")
        //     {
        //         initPos = transform.position;
        //         TargetFish = other;
        //     }
        // }

        // private void OnTriggerExit2D(Collider2D collision)
        // {
        //     if (collision.gameObject.name == "SoapCollider")
        //     {
        //         TargetFish = null;
        //         isCleaningFish = false;
        //         cuoCount = 0;
        //         maxInPath = transform.position;
        //         initPos = transform.position;
        //     }
        // }

        // private void OnTriggerStay2D(Collider2D collision)
        // {
        //     if (collision.gameObject.name != "SoapCollider" || fish_soap_collider != collision)
        //     {
        //         return;
        //     }
        //     // 第二次检测到，且没变targetfish，就比较位置，变化够大就完成一次搓
        //
        //
        //     maxInPath.x = MathF.Max(maxInPath.x, transform.position.x);
        //     maxInPath.y = MathF.Max(maxInPath.y, transform.position.y);
        //     minInPath.x = MathF.Min(minInPath.x, transform.position.x);
        //     minInPath.y = MathF.Min(minInPath.y, transform.position.y);
        //     if (maxInPath.x - minInPath.x > 0.4f || maxInPath.y - minInPath.y > 0.4f)
        //     {
        //         initPos = transform.position;
        //         maxInPath = initPos;
        //         minInPath = initPos;
        //         cuoCount++;
        //         if (cuoCount > 5)
        //         {
        //             // todo fsx
        //             var fish = collision.transform.parent.GetComponent<Fish>();
        //             var lastneedList = new List<ItemEnum>();
        //             lastneedList.AddRange(fish.needList);
        //             fish.needList.Remove(lastneedList[0]);
        //             var newitem = (ItemEnum)GetRandomExcluding((int)ItemEnum.蟹黄堡, (int)ItemEnum.核弹, lastneedList);
        //             fish.needList.Add(newitem);
        //             cuoCount = 0;
        //         }
        //     }
        // }

        private void OnMouseDown()
        {
            _dragging = true;
            RuntimeManager.PlayOneShotAttached(FMODName.Event.SFX_SoundEffect_7____, gameObject);
        }

        private void OnMouseUp()
        {
            _dragging = false;
        }

        public void OnMouseDrag()
        {
            _dragging = true;
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