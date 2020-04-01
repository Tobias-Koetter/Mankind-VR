using UnityEngine;

public class HandController : MonoBehaviour
{

    public Transform objectCenter;
    public float checkDistance = 0.4f;
    public LayerMask collisionMask;


    bool isCollision;

    void Start()
    {
        isCollision = false;
    }

    void Update()
    {
        isCollision = Physics.CheckSphere(objectCenter.position, checkDistance, collisionMask);
        if (isCollision && Input.GetMouseButtonDown(0)) { 
            print("collision");
        }
        isCollision = false;
    }


}
