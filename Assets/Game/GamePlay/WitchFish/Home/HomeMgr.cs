using System.Collections;
using FMOD;
using Game;
using UnityEngine;
using UnityToolkit;
using WitchFish.UI;

namespace WitchFish
{
    public class HomeMgr : MonoSingleton<HomeMgr>
    {
        protected override void OnInit()
        {
            UIRoot.Singleton.OpenPanelAsync<GameHomePanel>();
        }

        protected override void OnDispose()
        {
            UIRoot.Singleton.Dispose<GameHomePanel>();
        }
    }
}