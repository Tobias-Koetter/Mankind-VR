using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class HandFeedback : MonoBehaviour
{
    public Transform cam;
    public Transform end;
    public LayerMask mask;

    private Vector3 initPoint;
    private Vector3 startPos;
    private Vector3 endPos;

    private bool isInterpolating = false;
    private bool isForward = false;
    private float interpolVal = 0.0f;
    private float stepStart = 1f;
    private float interpolStep = 3f;

    // Start is called before the first frame update
    void Start()
    {
        initPoint = transform.localPosition;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (!isInterpolating && !isForward)
        {
            if (Physics.Linecast(cam.position, end.position, out hit, mask))
            {
                Debug.Log(hit.transform.gameObject + "was hit.");
                if (hit.transform.tag.Equals("Tree"))
                {
                    isInterpolating = true;
                    startPos = transform.position;
                    InvokeRepeating("MoveHandForward", 0f, 0.02f);

                }
            }
        }
        if(!isInterpolating && isForward)
            if (!Physics.Linecast(cam.position, end.position, out hit, mask))
            {
                Debug.Log("hit was canceled.");
                isInterpolating = true;
                startPos = transform.position;
                InvokeRepeating("MoveHandBackward", 0f, 0.02f);
            }
    }

    public void MoveHandForward()
    {
        Debug.Log("Invoking with val"+ interpolVal);
        endPos = end.position;
        if (interpolVal < 0.1f || interpolVal > 0.2f){ interpolVal += stepStart * Time.deltaTime; }
        else { interpolVal += interpolStep * Time.deltaTime; }

        if(interpolVal > 0.3f)
        {
            interpolVal = 0.3f;
            isInterpolating = false;
            isForward = true;
            CancelInvoke();
        }
        
        transform.position = Vector3.Lerp(startPos, endPos, interpolVal);
    }

    public void MoveHandBackward()
    {
        Debug.Log("Invoking with val" + interpolVal);
        
        endPos = cam.position + initPoint;
        if (interpolVal < 0.1f || interpolVal > 0.2f) { interpolVal += stepStart * Time.deltaTime; }
        else { interpolVal += interpolStep * Time.deltaTime; }

        if (interpolVal < 0f)
        {
            interpolVal = 0f;
            isInterpolating = false;
            isForward = false;
            CancelInvoke();
        }

        transform.position = Vector3.Lerp(startPos, endPos, interpolVal);
    }
}
