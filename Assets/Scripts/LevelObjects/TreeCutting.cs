using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCutting : MonoBehaviour
{
    public Rigidbody[] midParts;
    public Rigidbody topPart;
    public GameObject bottomPart;

    [Range(0.1f,10f)]
    public float totalTime_MidExplosion = 0.3f;
    [Range(0.1f, 10f)]
    public float totalTime_inBetween = 0.1f;
    [Range(0.1f, 10f)]
    public float totalTime_final = 10f;
    [Range(0f, 10f)]
    public float totalTime_dissolve = 4f;
    public float strength_Force = 200f; 

    private float timer = 0f;

    private Vector3 force;
    Vector3[] start;
    Vector3 end;

    private bool inMidExpl = true;
    private bool inBetween = false;
    private bool inFinal = false;
    public bool isDead = false;
    public bool isGrounded = false;

    public bool IsDead { get { return isDead; } }

    public void SetGrounded() { isGrounded = true; }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Rigidbody r in midParts) r.isKinematic = false;
        timer = 0f;
        int dir = Random.Range(0, 4);   // 0,1,2,3 (Range(int inclusive ,int exclusive))
        force = Vector3.zero; 
        switch (dir)                    // Use x and y axis because the model has z as the height axis
        {                               // We only want to create a force along the horizontal plane
            case 0:
                force = Vector3.left * strength_Force;
                break;
            case 1:
                force = Vector3.right * strength_Force;
                break;
            case 2:
                force = Vector3.up * strength_Force;
                break;
            case 3:
                force = Vector3.down * strength_Force;
                break;
        }


        
    }

    void Update() {
        if (inMidExpl)
        {
            timer += Time.deltaTime;

            if (timer >= totalTime_MidExplosion)
            {

                topPart.isKinematic = false;
                timer = 0f;
                inMidExpl = false;
                inBetween = true;
            }
        }
        else if (inBetween)
        {
            timer += Time.deltaTime;
            //topPart.AddForceAtPosition(force, topPart.transform.position + topPart.transform.forward * 1.5f);
            topPart.AddRelativeTorque(force, ForceMode.Force);
            if (timer >= totalTime_inBetween)
            {

                timer = 0f;
                inBetween = false;
                inFinal = true;
            }

        }
        else if (inFinal)
        {
            if (isGrounded)
            {
                //topPart.isKinematic = true;
                isDead = true;
                inFinal = false;
                foreach (Rigidbody r in midParts)
                {
                    r.isKinematic = true;
                    r.gameObject.GetComponent<MeshCollider>().enabled = false;
                }
                //StopAllCoroutines();
                //StartCoroutine(DissolveMidparts());
                start = new Vector3[midParts.Length];

                for (int i = 0; i < midParts.Length; i++)
                {
                    Vector3 pos = midParts[i].transform.position;
                    start[i] = pos;
                }
                end = bottomPart.transform.position + Vector3.down * 0.5f;
                timer = 0f;
            }
            else
            {
                if (timer <= totalTime_final)
                {
                    timer += Time.deltaTime;
                    //topPart.AddForceAtPosition(force, topPart.transform.position + topPart.transform.forward * 1.5f);
                    topPart.AddRelativeTorque(force, ForceMode.Force);
                }
                else
                {
                    isGrounded = true;
                    timer = 0f;
                }
            }
        }
        else if (IsDead)
        {
            if (timer < totalTime_dissolve)
            {
                timer += Time.deltaTime;
                if (timer > totalTime_dissolve) timer = totalTime_dissolve;

                for (int i = 0; i < midParts.Length; i++)
                {
                    midParts[i].transform.position = Vector3.Lerp(start[i], end, timer / totalTime_dissolve);
                    if (timer == totalTime_dissolve) midParts[i].gameObject.SetActive(false);
                }
            }
            else
            {
                this.enabled = false;
            }
        }
    }

}
