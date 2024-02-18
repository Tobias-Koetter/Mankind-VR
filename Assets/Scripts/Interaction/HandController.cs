using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class HandController : MonoBehaviour
{
    public Transform objectCenter;
    public Transform cam;
    public float checkDistance = 0.4f;
    public LayerMask collisionMask;
    public Image clickCursor;

    //Temporarily used to give feedback for the player. Exchange for proper Feedback 
    public DecalProjector decalProjector;
    public Material interactMat;
    private Material normalMat;
    private Renderer ownRender;
    

    void Start()
    {

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
            if (Input.GetButton("Fire1"))
            {
                RaycastHit hitInfo;
                Vector3 lookDir = (cam.position + objectCenter.position).normalized;
                float distance = Vector3.Distance(cam.position, objectCenter.position);
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, distance, collisionMask))
                {
                    GameObject colliderObject = hitInfo.collider.gameObject;
                    Debug.Log(colliderObject);

                    if (colliderObject.activeSelf)
                    {
                        Interactable current = colliderObject.GetComponent<Interactable>();
                        Interactable parent = GlobalMethods.FindParentWithTag(colliderObject, "TreeLogic")?.GetComponent<Interactable>();
                        if (current is Spawned spawn)
                        {
                            int curID = spawn.PoolNumber;

                            spawn.SpawnMaster.DespawnObjectWithID(curID);
                        }
                        else if (parent is Trees tree /*&& colliderObject.name.EndsWith("0")*/)
                        {
                            decalProjector.transform.SetParent(this.transform);
                            decalProjector.transform.position = cam.transform.position + (hitInfo.point- cam.transform.position);
                            Vector3 worldLookAtPoint = new Vector3(tree.transform.position.x,hitInfo.point.y,tree.transform.position.z);
                            decalProjector.transform.LookAt(worldLookAtPoint);
                            decalProjector.transform.Translate(Vector3.back * 0.1f, Space.Self);

                            decalProjector.transform.SetParent(null);
                            //Debug.Log($"Got called because of {colliderObject}");
                            tree.Controller.handleTreeDestroy(tree);
                        }
                        else if (colliderObject.CompareTag("PointOfInterest"))
                        {
                            PoI p = colliderObject.GetComponent<PoI>();
                            p.Controller.handlePoIDestroy(p);
                        }
                        else
                        {

                        }

                    }


                }
                else
                {
                    print("Nothing there to interact with!");
                }
                ownRender.material = normalMat;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug Interaction stuff
        if (other.CompareTag("PointOfInterest"))
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
