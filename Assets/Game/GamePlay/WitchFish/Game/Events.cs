using UnityToolkit;
namespace WitchFish
{
    public struct EventFishDiePush : IEvent
    {
        public string pa;
    }

    public struct EventFishDieInLakePush : IEvent
    {
        public string pa;
    }

    public struct EventFishJumpInLakePush : IEvent
    {
        public string pa;
    }

    public struct EventFishPush : IEvent
    {
        public string pa;
    }

    public struct EventFishDieInLandPush : IEvent
    {
        public string pa;
    }

}