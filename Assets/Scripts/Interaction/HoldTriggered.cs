using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldTriggered : MonoBehaviour
{
    public Area_Spawn spawnArea;
    public LayerMask filterMask;
    private void OnTriggerEnter(Collider other) {
        int layer = other.gameObject.layer;
        if (filterMask == (filterMask | 1 << layer))
        {
            Spawned spawned = other.attachedRigidbody ? other.attachedRigidbody.gameObject.GetComponent<Spawned>(): null;
            if(spawned)
            {
                spawnArea.MarkAsInsideCollider(spawned);
            }
        }
    }
}
