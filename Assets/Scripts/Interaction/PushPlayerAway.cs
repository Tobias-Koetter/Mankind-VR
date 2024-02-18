
using System.Collections.Generic;
using UnityEngine;

public class PushPlayerAway : MonoBehaviour
{
    public List<Transform> pushTargets;
    public LayerMask playerMask;

    private Transform currentTarget;
    private float currentDistance;

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        int layer = hit.collider.gameObject.layer;
        if(playerMask == ( playerMask | 1 << layer))
        {
            
            Vector3 contactPoint = hit.point;
            foreach(Transform t in pushTargets)
            {
                float dist = Vector3.Distance(t.position, contactPoint);
                if (currentTarget == null || dist < currentDistance)
                {
                    currentDistance = dist;
                    currentTarget = t;
                }
            }
            Vector3 target = currentTarget.position;
            target.y = 0f;
            
            hit.controller.transform.Translate(Vector3.up*1f,Space.World);
            Vector3 dir = (currentTarget.position - this.transform.position);
            dir.y = 0f;
            dir = dir.normalized;
            hit.controller.transform.Translate(dir * 0.5f, Space.World);
        }
    }
}
