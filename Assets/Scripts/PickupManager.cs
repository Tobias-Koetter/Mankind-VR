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
    private Quaternion inHandRotation = Quaternion.Euler(-65f,150f,90f);
    private Quaternion originalRotation;

    private Vector3 originalScale;

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
                    /*
                    MeshRenderer mR = lastPickup.getRenderer();
                    Material[] mats = mR.materials;
                    oldMat = new Material[mats.Length];
                    for (int i = 0; i < mats.Length; i++)
                    {
                        oldMat[i] = mats[i];
                        mats[i] = highlight;
                    }
                    mR.materials = mats;
                    */
                    originalScale = lastPickup.transform.localScale;
                    lastPickup.getBody().isKinematic = true;
                    lastPickup.transform.SetParent(hand, false);
                    lastPickup.transform.SetPositionAndRotation(hand.position + hand.up*0.15f + hand.forward*0.1f, hand.rotation*inHandRotation);
                    Vector3 scale = lastPickup.transform.localScale;
                    lastPickup.transform.localScale = new Vector3(scale.x/hand.localScale.x, scale.y/hand.localScale.y, scale.z/hand.localScale.z);
                    

                }
            }
            else if (lastPickup != null && !hit)
            {
                Debug.Log("Do we get here?");
                /*
                MeshRenderer mR = lastPickup.getRenderer();
                Material[] mats = mR.materials;
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = oldMat[i];
                }
                mR.materials = mats;
                */
                lastPickup.transform.parent = null;
                lastPickup.getBody().isKinematic = false;
                lastPickup.transform.localScale = originalScale;
                lastPickup = null;
            }
        }
/*
        if(lastPickup != null)
        {
            lastPickup.transform.position = hand.position + (hand.up * 0.1f + hand.forward*0.2f);
            lastPickup.transform.rotation = hand.rotation* inHandRotation;
        }
        */
    }
}
