using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private Pawn pawn;

    void Start()
    {
        pawn = GetComponentInParent<Pawn>();
    }

    // This method will be called by the AnimationEvent
    public void TriggerAnimationEvent(string eventData)
    {
        pawn.OnAnimationEvent(eventData);
    }
}
