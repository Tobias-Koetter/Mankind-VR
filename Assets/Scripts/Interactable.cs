using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable: MonoBehaviour
{
    protected Collider ownCollider;
    protected MeshRenderer ownRenderer;
    protected Rigidbody ownRigidBody;

    void Start()
    {
        ownCollider = this.GetComponent<Collider>();
        //ownRenderer = this.GetComponent<MeshRenderer>();
        ownRigidBody = this.GetComponent<Rigidbody>();
    }
    public virtual void Interact() { throw new System.NotImplementedException("Was not implemented in the current child class."); }

    public virtual void Spawn(bool value)
    {
        /*
        ownCollider.enabled = value;
        //ownRenderer.enabled = value;
        ownRigidBody.useGravity = value;
        ownRigidBody.isKinematic = !value;
        */
    }
}
