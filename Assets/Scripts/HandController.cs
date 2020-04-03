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
        if (Input.GetMouseButtonDown(0)) { 
            Collider[] hitColliders = Physics.OverlapSphere(objectCenter.position, checkDistance, collisionMask);

            isCollision = hitColliders.Length > 0;

            //if left mouse button is clicked and there is a colliding object
            if (isCollision) {

                for (int i = 0; i < hitColliders.Length; i++)
                {
                    print("collider: " + hitColliders[i].ToString());

                    if (hitColliders[i].gameObject.activeSelf) {
                        Interactable current = hitColliders[i].gameObject.GetComponent<Interactable>();
                        if (current)
                        {
                            int curID = current.PoolNumber;

                            current.SpawnMaster.despawnObjectWithID(curID);
                        }
                        else
                        {
                            print("The current Object: \""+ hitColliders[i].gameObject.name +"\" was not spawned by a spawnController");
                        }
                        
                    }

                }


            }
            else
            {
                print("Nothing there to interact with!");
            }
            isCollision = false;
        }
    }


}
