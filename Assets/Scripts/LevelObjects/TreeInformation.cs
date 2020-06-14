using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TreeInformation : MonoBehaviour
{
    public LODGroup lodGroup;
    public LOD[] lods;
    public Material[] dissolveMats;
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
        for (int i = 0; i < lods.Length; i++)
        {
            try{
                dissolveMats[i] = lods[i].renderers[0].materials[1];
            }catch(IndexOutOfRangeException)
            {
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
}
