using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PickupManager : MonoBehaviour
{
    public Transform fingerPointer;
    public Transform hand;
    public LayerMask hitLayers;
    public Material highlight;
    float maxDist;

    private Material[] oldMat;
    private Spawned lastPickup;

    void Start()
    {
        maxDist = Vector3.Distance(Camera.main.transform.position,fingerPointer.position);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool hit = Physics.Raycast(ray, out hitInfo, maxDist, hitLayers);
            if (lastPickup == null && hit)
            {
                if (hitInfo.collider.CompareTag("Small Trash"))
                {
                    lastPickup = hitInfo.collider.gameObject.GetComponent<Spawned>();
                    Debug.Log("Pickup Triggered with " + lastPickup);
                    //Debug.Log(lastPickup);
                    MeshRenderer mR = lastPickup.getRenderer();
                    Material[] mats = mR.materials;
                    oldMat = new Material[mats.Length];
                    for (int i = 0; i < mats.Length; i++)
                    {
                        oldMat[i] = mats[i];
                        mats[i] = highlight;
                    }
                    mR.materials = mats;

                    lastPickup.transform.parent = hand;
                    lastPickup.transform.localPosition = Vector3.zero;
                    lastPickup.getBody().isKinematic = true;

                }
            }
            else if (lastPickup != null && !hit)
            {
                Debug.Log("Do we get here?");
                MeshRenderer mR = lastPickup.getRenderer();
                Material[] mats = mR.materials;
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = oldMat[i];
                }
                mR.materials = mats;
                lastPickup.transform.parent = null;
                lastPickup.getBody().isKinematic = false;
                lastPickup = null;
            }
        }
    }
}
