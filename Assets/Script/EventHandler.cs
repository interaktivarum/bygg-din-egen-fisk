using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FishBodyClickedEvent : UnityEvent<FishBody>{}
public class FishHeadClickedEvent : UnityEvent<FishHead>{}
public class FishPatternClickedEvent : UnityEvent<FishBody> {}

public static class EventHandler
{
    public static FishBodyClickedEvent fishBodyClickedEvent = new FishBodyClickedEvent();
    public static FishHeadClickedEvent fishHeadClickedEvent = new FishHeadClickedEvent();
    public static FishPatternClickedEvent fishPatternClickedEvent = new FishPatternClickedEvent();
}