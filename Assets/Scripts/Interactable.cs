using UnityEngine;

public class Interactable : MonoBehaviour
{
    Collider ownCollider;
    MeshRenderer ownRenderer;
    Rigidbody ownRigidBody;

    private SpawnController spawnMaster;
    public SpawnController SpawnMaster { get => spawnMaster; set => spawnMaster = value; }

    private int poolNumber = 0;
    public int PoolNumber { get => poolNumber; set => poolNumber = value; }


    void Start()
    {
        ownCollider = this.GetComponent<Collider>();
        ownRenderer = this.GetComponent<MeshRenderer>();
        ownRigidBody = this.GetComponent<Rigidbody>();
    }
    public void Interact()
    {
        
        print(">> i "+ this.name+" was forced to interact from someone else <<");
    }


    public void Spawn(bool value)
    {
        ownCollider.enabled = value;
        ownRenderer.enabled = value;
        ownRigidBody.useGravity = value;
    }
}
