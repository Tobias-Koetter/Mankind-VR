using System.Collections.Generic;
using UnityEngine;

public class TreeComparer : IComparer<Trees>
{
    public int Compare(Trees x, Trees y) => x.TreeNumber.CompareTo(y.TreeNumber);
}

public class PoIComparer : IComparer<PoI>
{
    public int Compare(PoI x, PoI y) => x.Number.CompareTo(y.Number);
}
public class DestroyVegetation : MonoBehaviour
{
    public GameObject PoIParent;
    public GameObject treeParent;
    public List<Trees> aliveTrees;
    public List<Trees> deadTrees;
    public List<PoI> PoIList;

    private TreeComparer tC;


    // Start is called before the first frame update
    void Start()
    {
        tC = new TreeComparer();

        Trees[] temp = treeParent.GetComponentsInChildren<Trees>();
        int counter = 0;
        foreach(Trees t in temp)
        {
            t.TreeNumber = counter++;
            t.Controller = this;
            aliveTrees.Add(t);
        }

        counter = 0;
        PoI[] temp2 = PoIParent.GetComponentsInChildren<PoI>();
        foreach(PoI p in temp2)
        {
            p.Setup(this, counter++);
            PoIList.Add(p);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void handleTreeDestroy(Trees t)
    {
        int treeIndex = aliveTrees.BinarySearch(t,tC);
        if(treeIndex > 0)
        {
            t.Interact();
            if (t.status == TREE_STAGE.DEAD)
            {
                aliveTrees.RemoveAt(treeIndex);
                deadTrees.Add(t);
                deadTrees.Sort(tC);
            }
        }
    }

    public bool handlePoIDestroy(PoI poi)
    {
        foreach(PoI p in PoIList)
        {
            if(p.Equals(poi))
            {
                p.gameObject.SetActive(false);
                return true;
            }
        }
        return false;
    }

}
