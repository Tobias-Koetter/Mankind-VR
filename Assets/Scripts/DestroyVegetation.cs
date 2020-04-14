using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeComparer : IComparer<Trees>
{
    public int Compare(Trees x, Trees y) => x.TreeNumber.CompareTo(y.TreeNumber);
}

public class DestroyVegetation : MonoBehaviour
{
    public GameObject treeParent;
    public List<Trees> aliveTrees;
    public List<Trees> deadTrees;

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
            aliveTrees.RemoveAt(treeIndex);
            deadTrees.Add(t);
            deadTrees.Sort(tC);
            foreach (Trees a in deadTrees)
                print(a.ToString());
        }
    }
}
