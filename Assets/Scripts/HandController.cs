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
                    GameObject colliderObject = hitColliders[i].gameObject;
                    if (colliderObject.activeSelf) {
                        Interactable current = colliderObject.GetComponent<Interactable>();
                        Interactable parent = colliderObject.GetComponentInParent<Interactable>();
                        if(current is Spawned spawn)
                        {
                            int curID = spawn.PoolNumber;

                            spawn.SpawnMaster.despawnObjectWithID(curID);
                        }
                        else if (parent is Trees tree)
                        {
                            tree.Controller.handleTreeDestroy(tree);
                        }
                        else if(colliderObject.tag.Equals("PointOfInterest"))
                        {
                            PoI p = colliderObject.GetComponent<PoI>();
                            p.Controller.handlePoIDestroy(p);
                        }
                        else
                        {
                            print("The current Object: \"" + colliderObject.name + "\" was not spawned by a spawnController");

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
