using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrigger : MonoBehaviour
{
    private Pawn eventListener;

    public void Initialize(Pawn listener)
    {
        eventListener = gameObject.AddComponent<Pawn>();
            
        if (!listener) eventListener = listener;
    }

    // This method will be called by the AnimationEvent
    public void TriggerAnimationEvent(string eventData)
    {
        eventListener.OnAnimationEvent(eventData);
    }
}
