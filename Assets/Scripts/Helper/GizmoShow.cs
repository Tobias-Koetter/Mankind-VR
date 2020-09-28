using UnityEngine;

public class GizmoShow : MonoBehaviour
{
    public Color gizmoColor;
    private void OnDrawGizmos() 
    {
        Gizmos.color = gizmoColor; 
        Gizmos.DrawSphere(transform.position, 2f); 
    }
}
