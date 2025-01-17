using Game;
using UnityEngine;

namespace WitchFish
{
    public class GameEntry : MonoBehaviour, IGameEntry
    {
        public GameObject corePrefab;
        public bool initialized { get; private set; }

        public void OnInit()
        {
            Instantiate(corePrefab);
            initialized = true;
        }
    }
}