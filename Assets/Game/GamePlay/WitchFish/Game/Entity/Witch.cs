using FMOD;
using FMODUnity;
using UnityEngine;

namespace WitchFish
{
    public class Witch : MonoBehaviour, ISoapTarget
    {
        public StudioEventEmitter eventEmitter;
        public void OnSoup()
        {
            if (!eventEmitter.IsPlaying())
            {
                eventEmitter.Play();
            }
        }
    }
}