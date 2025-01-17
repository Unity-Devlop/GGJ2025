using UnityToolkit;

namespace WitchFish
{
    public class Core : MonoSingleton<Core>
    {
        protected override bool DontDestroyOnLoad() => true;

        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}