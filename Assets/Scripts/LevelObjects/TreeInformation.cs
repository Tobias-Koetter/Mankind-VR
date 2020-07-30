using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TreeInformation : MonoBehaviour
{
    public LODGroup lodGroup;
    public LOD[] lods;
    public Material[] dissolveMats;
    public Renderer[] renderersInLOD;
    private List<MeshCollider> listOfColliders;

    public TreeInformation(LODGroup l)
    {
        setUpInformation(l);
    }

    void Start()
    {
        setUpInformation(lodGroup);
    }


    private void setUpInformation(LODGroup l)
    {
        lodGroup = l;
        lods = l.GetLODs();
        dissolveMats = new Material[lods.Length];
        renderersInLOD = new Renderer[lods.Length];
        for (int i = 0; i < lods.Length; i++)
        {
            try{
                renderersInLOD[i] = lods[i].renderers[0];
                dissolveMats[i] = renderersInLOD[i].materials[1];
            }catch(IndexOutOfRangeException)
            {
                renderersInLOD[i] = null;
                dissolveMats[i] = null;
            }
        }
        listOfColliders = new List<MeshCollider>();
        GetColliders();
    }

    private void GetColliders()
    {
        foreach (MeshCollider collider in lodGroup.gameObject.GetComponentsInChildren<MeshCollider>())
        {
            listOfColliders.Add(collider);
        }
    }

    public void EnableColliders(bool active)
    {
        foreach(MeshCollider c in listOfColliders)
        {
            c.enabled = active;
        }
    }

    public bool CheckAnyRendererIsVisible(bool toCheck)
    {
        bool ret = false;
        foreach (Renderer r in renderersInLOD)
        {
            if(r.isVisible)
            {
                ret = true;
                break;
            }
        }
        return ret == toCheck;
    }
}
