using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TreeInformation : MonoBehaviour
{
    public LODGroup lodGroup;
    public LOD[] lods;
    public List<Material> dissolveMats;
    public Renderer[] renderersInLOD;
    private List<MeshCollider> listOfColliders;
    public bool hasRigidBody = false;
    private TreeCutting cutter;

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
        dissolveMats = new List<Material>();
        renderersInLOD = new Renderer[lods.Length];
        for (int i = 0; i < lods.Length; i++)
        {
            try{
                renderersInLOD[i] = lods[i].renderers[0];
                Material[] mats = renderersInLOD[i].materials;
                for (int j=0; j < mats.Length; j++)
                {
                    if (mats[j].name.Contains("leaves") || mats[j].name.Contains("Better"))
                    {
                        dissolveMats.Add( renderersInLOD[i].materials[j]);
                    }
                }
            }catch(IndexOutOfRangeException)
            {
                renderersInLOD[i] = null;
                //dissolveMats[i] = null;
            }
        }
        listOfColliders = new List<MeshCollider>();
        GetColliders();
        if (hasRigidBody)
        {
            cutter = GetComponent<TreeCutting>();
        }
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
    
    public bool StartTreeCutting() {
        bool ret;
        if(ret = hasRigidBody)
        {
            cutter.enabled = true;
        }
        return ret;
    }
}
