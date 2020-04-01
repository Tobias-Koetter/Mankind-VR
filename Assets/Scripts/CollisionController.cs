using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{

    public Transform objectCenter;
    public float checkDistance = 0.4f;
    public LayerMask collisionMask;


    bool isCollision;


    // Start is called before the first frame update
    void Start()
    {
        isCollision = false;
    }

    // Update is called once per frame
    void Update()
    {
        isCollision = Physics.CheckSphere(objectCenter.position, checkDistance, collisionMask);
        if (isCollision) { 
            print("collision");
        }
        isCollision = false;
    }
}
