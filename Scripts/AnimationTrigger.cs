using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private Pawn eventListener;

    void Start()
    {
        eventListener = GetComponentInParent<Pawn>();
    }

    // This method will be called by the AnimationEvent
    public void TriggerAnimationEvent(string eventData)
    {
        eventListener.OnAnimationEvent(eventData);
    }
}
