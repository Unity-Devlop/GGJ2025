using System.Collections;
using UnityEngine;
using UnityToolkit;
using WitchFish.UI;

namespace WitchFish
{
    public class HomeMgr : MonoSingleton<HomeMgr>
    {
        protected override void OnInit()
        {
            UIRoot.Singleton.OpenPanel<GameHomePanel>();
        }

        protected override void OnDispose()
        {
            UIRoot.Singleton.Dispose<GameHomePanel>();
        }
    }
}