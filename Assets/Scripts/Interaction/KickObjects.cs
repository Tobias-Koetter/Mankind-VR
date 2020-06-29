using System.Collections;
using UnityEngine;

public class KickObjects : MonoBehaviour
{
    public Transform kickcheck;
    public float objectDistance = 0.2f;
    public LayerMask kickableLayers;
    public string[] targetTags;

    public float kickPower = 10f;

    private Collider lastCol = null;
    private int counter = 0;

    private void Start()
    {
        //StartCoroutine(DoProximityCheck());
    }

    IEnumerator DoProximityCheck()
    {
        while (true)
        {
            Debug.Log("Is Running");
            Collider[] colliders = Physics.OverlapSphere(kickcheck.position, objectDistance, kickableLayers);

            if (colliders.Length > 0 && (lastCol == null || !lastCol.name.Equals(colliders[0])))
            {
                Debug.Log(colliders.Length);
                lastCol = colliders[0];
                foreach (Collider c in colliders)
                {

                    if (c.tag.Equals(targetTags[0]))
                    {
                        //Non-Physics
                        /*
                        Vector3 dir = c.ClosestPointOnBounds(kickcheck.position) - kickcheck.position;
                        dir = dir.normalized;
                        Vector3 newPoint = c.transform.position + dir * kickPower;
                        StartCoroutine(DoLerp(c.transform.position, newPoint,c.transform));
                        */

                        //Physics
                        //c.attachedRigidbody.AddExplosionForce(kickPower, kickcheck.position, 0f, 0.05f, ForceMode.Impulse);

                    }

                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }
    
    IEnumerator DoLerp(Vector3 start, Vector3 end, Transform t)
    {
        Debug.Log("DoLerp gets created");
        float amount = 0.1f;
        for(float i = 0f; i <1f;i += amount)
        {
            Vector3 cur = Vector3.Lerp(start, end, i);
            t.position = cur;

            if (i >= 0.2f && i <= 0.8f && amount < 0.02f)
            {
                amount -= 0.01f;
            }
            else if (i > 0.8f && amount > 0f)
            {
                amount -= 0.001f;
            }
            yield return null;
        }
        
    }

    /*
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (LayerMask.LayerToName(hit.collider.gameObject.layer).Equals("Trash"))
        {
            Rigidbody body = hit.rigidbody;
            Vector3 triggerPoint = hit.point;
            Vector3 dir = triggerPoint - kickcheck.position;
            if (dir.y >= -0.3f && !(body.mass >= 2))
            {
                dir.y = 0;
                dir = dir.normalized;
                Vector3 oldPos = body.transform.position;
                body.transform.position = (oldPos + dir) + Vector3.up * 0.5f;
                body.velocity = dir * kickPower*body.mass;
                body.AddTorque(Random.onUnitSphere* kickPower, ForceMode.Force);
            }
        }
    }
    */

    
    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer).Equals("Trash"))
        {
            Rigidbody body = other.attachedRigidbody;
            Vector3 triggerPoint = other.ClosestPointOnBounds(kickcheck.position);
            Vector3 dir = triggerPoint - kickcheck.position;
            if (dir.y >= -0.3f && !(body.mass >= 2))
            {
               
                dir.y = 0;
                dir = dir.normalized;
                Vector3 oldPos = body.transform.position;
                //body.transform.position =( oldPos+dir*0.2f) + Vector3.up * 0.5f;
                body.velocity = (dir+Vector3.up * 0.3f) * kickPower*body.mass;
                body.AddTorque(Random.onUnitSphere * kickPower, ForceMode.Force);
            }

        }

    }
    
}
