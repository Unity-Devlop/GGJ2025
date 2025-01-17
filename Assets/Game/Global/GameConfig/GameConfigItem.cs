using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    [CreateAssetMenu(fileName = "GameConfigItem", menuName = "Game/Global/GameConfigItem")]
    public class GameConfigItem : ScriptableObject
    {
        public AssetReference homeScene;
        public AssetReference playScene;
        public AssetReferenceT<GameObject> playMgrPrefab;
        public AssetReferenceT<GameObject> homeMgrPrefab;
        public AssetReferenceT<GameObject> gameEntry;
    }
}