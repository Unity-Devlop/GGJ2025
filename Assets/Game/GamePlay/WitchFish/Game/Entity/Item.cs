using System;
using Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace WitchFish
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Item : MonoBehaviour
    {
        private Collider2D _collider;
        private Rigidbody2D _rigidbody2D;
        public ItemStateEnum state = ItemStateEnum.在框子外;
        public ItemEnum id;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        public void OnMouseDown()
        {
            // GameLogger.Log.Information("{collider} isTrigger=true", _collider);
            _rigidbody2D.isKinematic = true;
            _collider.isTrigger = true;
        }

        private void OnMouseUp()
        {
            // GameLogger.Log.Information("{collider} isTrigger=false", _collider);
            _rigidbody2D.isKinematic = false;
            _collider.isTrigger = false;
        }

        public void OnMouseDrag()
        {
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