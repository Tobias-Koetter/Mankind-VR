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
    [Range(0f, 1f)]
    public float velocityDeadzone = 0.01f;
    public LayerMask groundMask;
    private float timer = 0f;

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
    }

    void Update() {
        if (inMidExpl)
        {
            timer += Time.deltaTime;
            if (timer >= totalTime_MidExplosion)
            {

                Vector2 torque = new Vector2(Random.Range(-100f, 100f), Random.Range(-10f, 10f));
                topPart.isKinematic = false;
                SphereCollider col = topPart.GetComponent<SphereCollider>();
                Vector3 forcePos = topPart.transform.TransformPoint(col.center);
                forcePos = topPart.transform.position + (forcePos-topPart.transform.position)  * 0.8f;
                Vector3 forceDir = Vector3.back;
                float forceStrength = 100f;
                //topPart.AddForceAtPosition(forceDir * forceStrength, forcePos - forceDir,ForceMode.Impulse);
                topPart.AddRelativeTorque(200f, 0f, 0f, ForceMode.Force);
                /*GameObject o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                o.transform.position = forcePos - forceDir;*/
                timer = 0f;
                inMidExpl = false;
                inBetween = true;
            }
        }
        else if(inBetween)
        {
            timer += Time.deltaTime;
            topPart.AddRelativeTorque(200f, 0f, 0f, ForceMode.Force);
            if (timer >= totalTime_inBetween)
            {
                /*foreach (Rigidbody r in midParts)
                {
                    r.gameObject.SetActive(false);
                }*/
                timer = 0f;
                inBetween = false;
                inFinal = true;
            }
            
        }
        else if(inFinal)
        {
            if(isGrounded)
            {
                //topPart.isKinematic = true;
                isDead = true;
                inFinal = false;
            }
            else
                topPart.AddRelativeTorque(200f, 0f, 0f, ForceMode.Force);
        }

    }

}
