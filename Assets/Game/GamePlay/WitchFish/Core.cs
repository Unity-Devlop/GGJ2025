using UnityEngine;
using UnityToolkit;

namespace WitchFish
{
    public class Core : MonoSingleton<Core>
    {
        protected override bool DontDestroyOnLoad() => true;

        private static TypeEventSystem _event;

        public static TypeEventSystem Event
        {
            get
            {
                if (_event == null)
                {
                    _event = new TypeEventSystem();
                }

                return _event;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoadMethod()
        {
            _event = new TypeEventSystem();
        }


        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}