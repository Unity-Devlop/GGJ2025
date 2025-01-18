using System;
using Game;
using UnityEngine;

namespace WitchFish
{
    public class Basket : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Item item))
            {
                item.state = ItemStateEnum.在框子里;
                // GameLogger.Log.Information("Enter Basket OnTriggerEnter2D {other}", other);
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Item item))
            {
                item.state = ItemStateEnum.在框子外;
                // GameLogger.Log.Information("Exit Basket {other}", other);
            }
        }
    }
}