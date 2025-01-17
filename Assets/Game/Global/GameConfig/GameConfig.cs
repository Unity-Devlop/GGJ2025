using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityToolkit;

namespace Game
{


    public class GameConfig : MonoBehaviour, ISystem, IOnInit
    {
        [SerializeField]
        private GameConfigItem configItem;
        public AssetReference homeScene => configItem.homeScene;
        public AssetReference playScene => configItem.playScene;
        public AssetReferenceT<GameObject> playMgrPrefab => configItem.playMgrPrefab;
        public AssetReferenceT<GameObject> homeMgrPrefab => configItem.homeMgrPrefab;
        
        public AssetReferenceT<GameObject> gameEntry=> configItem.gameEntry;

        public void OnInit()
        {
        }

        public void Dispose()
        {
        }
    }
}