using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventAtTrigger : MonoBehaviour
{
    public UnityEvent triggeredEvent_Enter;
    public UnityEvent triggeredEvent_Exit;
    public UnityEvent triggeredEvent_Stay;
    public LayerMask TriggerLayer;
    public bool useEnter = true;
    public bool useExit = true;
    public bool useStay = false;
    private void OnTriggerEnter(Collider other) {
        if ( useEnter && (TriggerLayer == (TriggerLayer | (1 << other.gameObject.layer) ) ) )
        {
            triggeredEvent_Enter.Invoke();
        }
    }
    private void OnTriggerExit(Collider other) {
        if (useExit && (TriggerLayer == (TriggerLayer | (1 << other.gameObject.layer))))
        {
            triggeredEvent_Exit.Invoke();
        }
    }
    private void OnTriggerStay(Collider other) {
        if (useEnter && (TriggerLayer == (TriggerLayer | (1 << other.gameObject.layer))))
        {
            triggeredEvent_Stay.Invoke();
        }
    }
}



