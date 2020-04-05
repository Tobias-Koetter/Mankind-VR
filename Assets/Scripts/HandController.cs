using UnityEngine;

public class HandController : MonoBehaviour
{
    public Transform objectCenter;
    public float checkDistance = 0.4f;
    public LayerMask collisionMask;

    //Temporarily used to give feedback for the player. Exchange for proper Feedback 
    public Material interactMat;
    private Material normalMat;
    private Renderer ownRender;

    bool isCollision;

    void Start()
    {
        isCollision = false;

        //Debug Interaction stuff
        ownRender = objectCenter.GetComponent<Renderer>();
        normalMat = ownRender.material;

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
                        if (current && current.SpawnMaster)
                        {
                            int curID = current.PoolNumber;

                            current.SpawnMaster.despawnObjectWithID(curID);
                        }
                        else if(current && current.tag == "PointOfInterest")
                        {
                            current.Spawn(false);
                            current.Interact();
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
            ownRender.material = normalMat;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(ownRender.material);
        //Debug Interaction stuff
        if (other.tag == "PointOfInterest")
        {
            ownRender.material = interactMat;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //Debug Interaction stuff
        ownRender.material = normalMat;
    }


}
