using System;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace WitchFish
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Item : MonoBehaviour
    {
        private Collider2D[] _colliders;

        private Rigidbody2D _rigidbody2D;
        public ItemStateEnum state = ItemStateEnum.在框子外;
        public ItemEnum id;

        [SerializeField] private SpriteRenderer _renderer;
        private float _timer;
        public float deSpawnTime = 5f;
private int sortingOrder;
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _colliders = GetComponents<Collider2D>();
            _timer = 0;

            sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
            Bind(id);
        }

        private void Update()
        {
            if (state == ItemStateEnum.在框子外)
            {
                _timer += Time.deltaTime;
            }
            else if (state == ItemStateEnum.在框子里)
            {
                _timer = 0;
            }

            if (_timer > deSpawnTime)
            {
                Destroy(gameObject);
            }
        }

        public void OnMouseDown()
        {
            // GameLogger.Log.Information("{collider} isTrigger=true", _collider);
            _rigidbody2D.isKinematic = true;
            foreach (var collider2D1 in _colliders)
            {
                collider2D1.isTrigger = true;
            }
            _renderer.sortingOrder = 10;
        }

        private void OnMouseUp()
        {
            // GameLogger.Log.Information("{collider} isTrigger=false", _collider);
            _rigidbody2D.isKinematic = false;
            
            foreach (var collider2D1 in _colliders)
            {
                collider2D1.isTrigger = false;
            }
            
            // Vector3 pos = transform.position;
            // float z = transform.parent.position.z;
            // transform.position = new Vector3(pos.x, pos.y, z);
            _renderer.sortingOrder = sortingOrder;
        }

        public void OnMouseDrag()
        {
           
            // Vector3 pos = transform.position;
            // pos.z = 0;
            // transform.position = pos;
            
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


        public void Bind(ItemEnum type)
        {
            _renderer.sprite = GameMgr.Singleton.id2Sprite[type];
            id = type;
        }
    }
}