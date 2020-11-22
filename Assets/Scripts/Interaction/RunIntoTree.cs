using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TypeOfParent{P_POI,P_INT}

public struct ColliderInfo
{
    public Collider c;
    public TypeOfParent pType;
    public UnityEngine.Object parent;

    public void Setup(Collider c, TypeOfParent t, UnityEngine.Object o)
    {
        this.c = c;
        pType = t;
        parent = o;
    }
}

public class RunIntoTree : MonoBehaviour
{
    public Transform radiusEndpoint;
    public Transform centerOfSphere;
    public LayerMask checkLayers;

    private float radius;
    private List<ColliderInfo> lastTreeCollisions;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 endPoint = radiusEndpoint.position;
        Vector3 centerPoint = centerOfSphere.position;
        endPoint.y = 0f;
        centerPoint.y = 0f;
        radius = Vector3.Distance(centerPoint,endPoint);
        lastTreeCollisions = new List<ColliderInfo>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapSphere(centerOfSphere.position, radius, checkLayers);
        if(hits.Length != 0)
        {
            foreach(Collider c in hits)
            {
                if(lastTreeCollisions.Select(items => items.c).Contains(c))
                {
                    break;
                }
                if (c.gameObject.layer == LayerMask.NameToLayer("PoI"))
                {
                    PoI parent = GlobalMethods.FindParentWithTag(c.gameObject, "PoI")?.GetComponent<PoI>();
                    if(parent is FallenBigTree fallen)
                    {
                        BrokenTreePart brokenPart = c.gameObject.GetComponent<BrokenTreePart>();
                        if(brokenPart.Alive)
                        {
                            fallen.DestroyPart(brokenPart);
                            ColliderInfo cInfo = new ColliderInfo(); 
                            cInfo.Setup(c, TypeOfParent.P_POI, parent);
                            lastTreeCollisions.Add(cInfo);
                        }

                    }
                }
                else if (c.gameObject.layer == LayerMask.NameToLayer("Interactable"))
                {
                    Interactable parent = GlobalMethods.FindParentWithTag(c.gameObject, "TreeLogic")?.GetComponent<Interactable>();

                    if (parent is Trees tree)
                    {
                        if(c.name.EndsWith("0") && !tree.lDis.isDissolving)
                        {
                            tree.Controller.handleTreeDestroy(tree);
                            ColliderInfo cInfo = new ColliderInfo();
                            cInfo.Setup(c, TypeOfParent.P_INT, parent);
                            lastTreeCollisions.Add(cInfo);
                        }
                    }
                }
            }
            Collider[] tempCopy = lastTreeCollisions.Select(item => item.c).ToArray();
            for(int i = 0; i<tempCopy.Length;i++)
            {
                Collider c = tempCopy[i];
                Collider found = hits.FirstOrDefault(pC => pC.name.Equals(c.name));
                if (found == null)
                {
                    lastTreeCollisions.RemoveAt(i);
                }
            }
            /*
            foreach(Collider c in tempCopy)
            {
                Collider found = hits.FirstOrDefault(pC => pC.name.Equals(c.name));
                if(found == null)
                {
                    lastTreeCollisions.Remove(c);
                }
            }
            */
        }
        else
        {
            lastTreeCollisions.Clear();
        }
    }
}
