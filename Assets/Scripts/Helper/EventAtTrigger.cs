using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventAtTrigger : MonoBehaviour
{
    public UnityEvent triggeredEvent;

    private void OnTriggerEnter(Collider other) {
        triggeredEvent.Invoke();
    }
}
