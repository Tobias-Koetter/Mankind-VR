using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoShow : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 2f);
    }
}
