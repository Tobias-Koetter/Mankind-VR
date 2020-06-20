using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;
using UnityEngine.XR;

public struct RotAndPos
{
    public Vector3 pos;
    public Quaternion rot;
}


public class PickupManager : MonoBehaviour
{
    public Transform fingerPointer;
    public Transform hand;
    public LayerMask hitLayers;
    public Material highlight;
    public float maxDist;
    [Range(1f,10f)]
    public float interpolationSpeed = 1f;

    private Material[] oldMat;
    private Spawned lastPickup;
    private Quaternion inHandRotation = Quaternion.Euler(-65f,150f,90f);

    private List<RotAndPos> wayPoints;
    [Range(0f,1f)]
    public float wayTraveled = 0;
    private Vector3 idleVec = new Vector3(0.487f, 0.796f, 0.322f);
    private Vector3 lookAtVec = new Vector3(0.567f, 0.969f, 0.45f);
    private Vector3 resetVector = Vector3.one * -10f;
    private Vector3 curStart;
    private Vector3 curEnd;
    private Quaternion lookAtRot = Quaternion.Euler(-25f, -45f, 35f);
    private Quaternion inPickUpRot = Quaternion.Euler(11f, 7.5f, 170f);
    private Quaternion idleRot = Quaternion.Euler(59f, -12f, 105f);
    private Quaternion curStartRot;
    private Quaternion curEndRot;

    private bool inPickup = false;
    private bool inDropDown = false;

    private Vector3 originalScale;

    void Start()
    {
        wayPoints = new List<RotAndPos>();
        curStart = resetVector;
        curEnd = resetVector;

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
                    lastPickup = hitInfo.collider.attachedRigidbody.gameObject.GetComponent<Spawned>();
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
                    RotAndPos temp = new RotAndPos();
                    temp.pos = hand.localPosition;
                    temp.rot = idleRot;
                    wayPoints.Add(temp);

                    temp.pos = hand.parent.InverseTransformPoint(lastPickup.transform.position);
                    temp.rot = inPickUpRot;
                    wayPoints.Add(temp);

                    temp.pos = lookAtVec;
                    temp.rot = lookAtRot;
                    wayPoints.Add(temp);
                    inPickup = true;
                    StartCoroutine(SetStage_PickupItem());
                    

                }
            }
            else if (!inPickup && lastPickup != null && !hit && inDropDown)
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
                inDropDown = false;
                StartCoroutine(setStage_LetObjectFall());
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


    public void SetStage_ObjectInHand()
    {
        originalScale = lastPickup.transform.localScale;
        lastPickup.getBody().isKinematic = true;
        lastPickup.transform.SetParent(hand, false);
        lastPickup.transform.SetPositionAndRotation(hand.position + hand.up * 0.1f + hand.forward * 0.1f, hand.rotation * inHandRotation);
        Vector3 scale = lastPickup.transform.localScale;
        lastPickup.transform.localScale = new Vector3(scale.x / hand.localScale.x, scale.y / hand.localScale.y, scale.z / hand.localScale.z);
    }

    public IEnumerator setStage_LetObjectFall()
    {
        while (wayTraveled < 1f)//!hand.transform.localRotation == idleRot )
        {
            wayTraveled += interpolationSpeed * Time.deltaTime;
            hand.transform.localPosition = Vector3.Lerp(curStart, curEnd, wayTraveled);
            hand.localRotation = Quaternion.Lerp(curStartRot, curEndRot, wayTraveled);
            yield return new WaitForSeconds(0.01f);
        }

        hand.transform.localRotation.Set(idleRot.x,idleRot.y,idleRot.z,idleRot.w);
        hand.transform.localPosition.Set(idleVec.x, idleVec.y, idleVec.z);
        curStartRot = Quaternion.identity;
        curEndRot = Quaternion.identity;
        curStart = resetVector;
        curEnd = resetVector;

        wayTraveled = 0f;

        lastPickup.transform.parent = null;
        lastPickup.getBody().isKinematic = false;
        lastPickup.transform.localScale = originalScale;
        lastPickup = null;
        yield return null;
    }

    public IEnumerator SetStage_PickupItem()
    {
        while (inPickup)
        {
            if (curStart.Equals(resetVector) && curEnd.Equals(resetVector))
            {
                if (wayPoints.Count >= 2)
                {
                    if (wayPoints.Count == 2)
                    {
                        SetStage_ObjectInHand();
                    }
                    curStart = wayPoints[0].pos;
                    curEnd = wayPoints[1].pos;
                    curStartRot = wayPoints[0].rot;
                    curEndRot = wayPoints[1].rot;
                    wayPoints.RemoveAt(0);

                }
                else
                {
                    curStart = wayPoints[0].pos;
                    curEnd = idleVec;
                    curStartRot = curEndRot;
                    curEndRot = idleRot;
                    wayPoints.Clear();
                    inPickup = false;
                    inDropDown = true;
                    wayTraveled = 0f;
                }
            }
            else if (wayTraveled < 1f)
            {
                wayTraveled += interpolationSpeed * Time.deltaTime;
                hand.transform.localPosition = Vector3.Lerp(curStart, curEnd, wayTraveled);
                hand.transform.localRotation = Quaternion.Lerp(curStartRot, curEndRot, wayTraveled);
            }
            else if (wayTraveled >= 1f)
            {
                curStart = resetVector;
                curEnd = resetVector;
                wayTraveled = 0f;
            }
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }
}
