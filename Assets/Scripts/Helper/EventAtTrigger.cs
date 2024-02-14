using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventAtTrigger : MonoBehaviour
{
    public UnityEvent triggeredEvent;
    public LayerMask TriggerLayer;

    private void OnTriggerEnter(Collider other) {
        if (TriggerLayer == (TriggerLayer | (1 << other.gameObject.layer)))
        {
            triggeredEvent.Invoke();
        }
    }
}
