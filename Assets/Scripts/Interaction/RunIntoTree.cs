using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RunIntoTree : MonoBehaviour
{
    public Transform radiusEndpoint;
    public Transform centerOfSphere;
    public LayerMask checkLayers;

    private float radius;
    private List<Collider> lastTreeCollisions;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 endPoint = radiusEndpoint.position;
        Vector3 centerPoint = centerOfSphere.position;
        endPoint.y = 0f;
        centerPoint.y = 0f;
        radius = Vector3.Distance(centerPoint,endPoint);
        lastTreeCollisions = new List<Collider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapSphere(centerOfSphere.position, radius, checkLayers);
        if(hits.Length != 0)
        {
            foreach(Collider c in hits)
            {
                if(lastTreeCollisions.Contains(c))
                {
                    break;
                }
                Interactable parent = GlobalMethods.FindParentWithTag(c.gameObject, "TreeLogic")?.GetComponent<Interactable>();
                if(parent is Trees tree && c.name.EndsWith("0"))
                {
                    tree.Controller.handleTreeDestroy(tree);
                    lastTreeCollisions.Add(c);
                }
            }
            Collider[] tempCopy = lastTreeCollisions.ToArray();
            foreach(Collider c in tempCopy)
            {
                Collider found = hits.FirstOrDefault(pC => pC.name.Equals(c.name));
                if(found == null)
                {
                    lastTreeCollisions.Remove(c);
                }
            }
        }
        else
        {
            lastTreeCollisions.Clear();
        }
    }
}
