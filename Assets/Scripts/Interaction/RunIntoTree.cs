using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    private WaitForSeconds wait3Sec = new WaitForSeconds(3f);

    public Transform radiusEndpoint;
    public Transform centerOfSphere;
    public LayerMask checkLayers;
    //public DecalProjector m_decalProjector;
    public GameObject m_decalProjector;

    private float radius;
    private List<ColliderInfo> lastTreeCollisions;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 endPoint = radiusEndpoint.position;
        Vector3 centerPoint = centerOfSphere.position;
        endPoint.y = 0f;
        centerPoint.y = 0f;
        radius = Vector3.Distance(centerPoint,endPoint);
        lastTreeCollisions = new List<ColliderInfo>();
        cam = Camera.main;
        m_decalProjector.gameObject.SetActive(false);
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
                            RaycastHit rHit = new RaycastHit();
                            Vector3 midPlayer = this.transform.position + (cam.transform.position - this.transform.position) * 0.75f;
                            Physics.Raycast(midPlayer, -midPlayer + new Vector3(tree.transform.position.x, midPlayer.y, tree.transform.position.z), out rHit, 100f, checkLayers);
                            //DecalProjector newDecal = Instantiate<DecalProjector>(m_decalProjector);
                            GameObject newDecal = Instantiate<GameObject>(m_decalProjector);
                            newDecal.transform.SetParent(this.transform);
                            newDecal.transform.position = midPlayer + (rHit.point - midPlayer );
                            Vector3 worldLookAtPoint = new Vector3(tree.transform.position.x, rHit.point.y, tree.transform.position.z);
                            newDecal.transform.LookAt(worldLookAtPoint);
                            newDecal.transform.Translate(Vector3.back * 0.1f, Space.Self);
                            newDecal.transform.SetParent(null);
                            newDecal.gameObject.SetActive(true);
                            StartCoroutine(ResetBumbFeedback(newDecal));
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
    private IEnumerator ResetBumbFeedback(DecalProjector decal) {
        yield return wait3Sec;
        DestroyImmediate(decal.gameObject);
    }
    private IEnumerator ResetBumbFeedback(GameObject decal) {
        yield return wait3Sec;
        DestroyImmediate(decal);
    }
}
