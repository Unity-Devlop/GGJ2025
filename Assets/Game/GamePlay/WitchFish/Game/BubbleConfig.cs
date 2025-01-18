using System.Collections.Generic;
using UnityEngine;

namespace WitchFish
{
    [CreateAssetMenu(menuName = "WitchFish/Game/BubbleConfig")]
    public class BubbleConfig : ScriptableObject
    {
        public List<ItemEnum> randomList = new List<ItemEnum>();
    }
}