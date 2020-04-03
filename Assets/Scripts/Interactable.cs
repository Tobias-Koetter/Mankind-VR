using UnityEngine;

public class Interactable : MonoBehaviour
{
    MeshCollider ownCollider;
    MeshRenderer ownRenderer;
    Rigidbody ownRigidBody;
    void Start()
    {
        ownCollider = this.GetComponent<MeshCollider>();
        ownRenderer = this.GetComponent<MeshRenderer>();
        ownRigidBody = this.GetComponent<Rigidbody>();
    }
    public void interact()
    {
        
        print(">> i was forced to interact from someone else <<");
        selfDespawn();
    }


    private void selfDespawn()
    {
        ownCollider.enabled = false;
        ownRenderer.enabled = false;
        ownRigidBody.useGravity = false;
    }
}
