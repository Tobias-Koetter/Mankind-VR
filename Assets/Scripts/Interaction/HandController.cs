using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class HandController : MonoBehaviour
{
    public Transform objectCenter;
    public Transform cam;
    public float checkDistance = 0.4f;
    public LayerMask collisionMask;
    public Image clickCursor;

    //Temporarily used to give feedback for the player. Exchange for proper Feedback 
    //public DecalProjector decalProjector;
    public GameObject decalProjector;
    public Material interactMat;
    private Material normalMat;
    private Renderer ownRender;

    private WaitForSeconds wait3Sec = new WaitForSeconds(3f);



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
            if (Input.GetButtonDown("Fire1"))
            {
                RaycastHit hitInfo;
                Vector3 lookDir = (cam.position + objectCenter.position).normalized;
                float distance = Vector3.Distance(cam.position, objectCenter.position);
                if (Physics.BoxCast(cam.position, objectCenter.transform.localScale * 0.5f, cam.transform.forward, out hitInfo, Quaternion.identity, distance, collisionMask))
                //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, distance, collisionMask))
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
                            GameObject newDecal = Instantiate<GameObject>(decalProjector);
                            newDecal.transform.SetParent(this.transform);
                            newDecal.transform.position = cam.transform.position + (hitInfo.point- cam.transform.position);
                            Vector3 worldLookAtPoint = new Vector3(tree.transform.position.x,hitInfo.point.y,tree.transform.position.z);
                            newDecal.transform.LookAt(worldLookAtPoint);
                            newDecal.transform.Translate(Vector3.back * 0.1f, Space.Self);
                            newDecal.transform.SetParent(null);
                            newDecal.gameObject.SetActive(true);
                            ResetBumbFeedback(newDecal);
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
                    //print("Nothing there to interact with!");
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

    private IEnumerator ResetBumbFeedback(GameObject decal) {
        yield return wait3Sec;
        DestroyImmediate(decal);
    }
}
