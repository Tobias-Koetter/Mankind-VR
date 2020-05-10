using UnityEngine;

public class TreeInformation : MonoBehaviour
{
    public LODGroup lodGroup;
    public LOD[] lods;
    public Material[] dissolveMats;

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
            dissolveMats[i] = lods[i].renderers[0].materials[1];
        }
    }
}
