using UnityEngine;
using UnityEngine.UI;

public class HandController : MonoBehaviour
{
    public Transform objectCenter;
    public Transform cam;
    public float checkDistance = 0.4f;
    public LayerMask collisionMask;
    public Image clickCursor;

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

        if (!GlobalSettingsManager.clickActionActive)
        {
            clickCursor.gameObject.SetActive(false);
        }

    }

    void Update()
    {
        if (GlobalSettingsManager.clickActionActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo;
                Vector3 lookDir = (cam.position + objectCenter.position).normalized;
                float distance = Vector3.Distance(cam.position, objectCenter.position);
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, distance, collisionMask))
                {
                    GameObject colliderObject = hitInfo.collider.gameObject;
                    Debug.Log(colliderObject);

                    //Collider[] hitColliders = Physics.OverlapSphere(objectCenter.position, checkDistance, collisionMask);


                    //isCollision = hitColliders.Length > 0;

                    //if left mouse button is clicked and there is a colliding object
                    //if (isCollision) {

                    //for (int i = 0; i < hitColliders.Length; i++)
                    //{
                    //GameObject colliderObject = hitColliders[i].gameObject;
                    if (colliderObject.activeSelf)
                    {
                        Interactable current = colliderObject.GetComponent<Interactable>();
                        Interactable parent = GlobalMethods.FindParentWithTag(colliderObject, "TreeLogic")?.GetComponent<Interactable>();
                        if (current is Spawned spawn)
                        {
                            int curID = spawn.PoolNumber;

                            spawn.SpawnMaster.despawnObjectWithID(curID);
                        }
                        else if (parent is Trees tree /*&& colliderObject.name.EndsWith("0")*/)
                        {
                            //Debug.Log($"Got called because of {colliderObject}");
                            tree.Controller.handleTreeDestroy(tree);
                        }
                        else if (colliderObject.tag.Equals("PointOfInterest"))
                        {
                            PoI p = colliderObject.GetComponent<PoI>();
                            p.Controller.handlePoIDestroy(p);
                        }
                        else
                        {
                            //print("The current Object: \"" + colliderObject.name + "\" was not spawned by a spawnController");

                        }

                        //}

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
    }

    private void OnTriggerEnter(Collider other)
    {
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
