using System.Collections.Generic;
using System.Linq;
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
    private GameInfo state;
    private bool isInvokingMiddle = false;
    private bool isInvokingEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        tC = new TreeComparer();

        int counter = 0;

        if (treeParent == null)
        {
            Debug.LogError("treeParent is not selected in Editor.");
        }
        else
        {
            Trees[] temp = treeParent.GetComponentsInChildren<Trees>();
            foreach (Trees t in temp)
            {
                t.TreeNumber = counter++;
                t.Controller = this;
                aliveTrees.Add(t);
            }

            float pNatVal= Trees.startingNatureValue;

            foreach (Trees t in aliveTrees)
            {
                t.personalNatureValue = pNatVal;
            }
            Debug.LogWarning(pNatVal + "||" + counter);
            LevelBalancing.ResetBalanceValue(pNatVal * counter);
        }

        counter = 0;

        if (PoIParent == null)
        {
            Debug.LogError("POIParent is not selected in Editor.");
        }
        else
        {
            PoI[] temp2 = PoIParent.GetComponentsInChildren<PoI>();
            if (temp2 == null)
            {
                Debug.LogError($"No POI selected");
            }
            else
            {
                foreach (PoI p in temp2)
                {
                    p.Setup(this, counter++);
                    PoIList.Add(p);
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(!isInvokingMiddle && state.currentState >= STATE.DECAY_MAIN)
        {
            InvokeRepeating("destroyRandomTreeInMiddleState",0f,10f);
            isInvokingMiddle = true;
        }
        if (!isInvokingEnd && state.currentState >= STATE.FINAL)
        {
            if(isInvokingMiddle)
                CancelInvoke("destroyRandomTreeInMiddleState");

            InvokeRepeating("destroyRandomTreeInMiddleState", 0.5f, 3f);
            isInvokingMiddle = false;
            isInvokingEnd = true;
        }*/
    }
    public void Setup(GameInfo state) => this.state = state;

    public void handleTreeDestroy(Trees t)
    {
        //Debug.Log($"In DestroyVegetation because of {t}.");
        int treeIndex = aliveTrees.BinarySearch(t,tC);
        //Debug.Log($"Found {t} at position {treeIndex}.");
        if(treeIndex >= 0)
        {
            if(!t.lDis.isDissolving)
                t.Interact();
            if(t.status == TREE_STAGE.ALIVE2)
            {
                float substractionValue = Trees.startingNatureValue/100f*20f;      // gets reduced by 20% from original value 
                t.personalNatureValue -= substractionValue;
                LevelBalancing.SetNatureValue(substractionValue); 
            }
            if (t.status == TREE_STAGE.BETWEEN1 || t.status == TREE_STAGE.BETWEEN2)
            {
                float substractionValue = Trees.startingNatureValue / 100f * 15f; // gets reduced by 15% from original value
                t.personalNatureValue -= substractionValue;
                LevelBalancing.SetNatureValue(substractionValue);
            }
            else if (t.status == TREE_STAGE.DEAD)
            {
                aliveTrees.RemoveAt(treeIndex);
                deadTrees.Add(t);
                deadTrees.Sort(tC);
                LevelBalancing.SetNatureValue(t.personalNatureValue);            // gets reduced by remaining 50% from original value

                if(deadTrees.Count == 1)
                {
                    state.SwitchFirstTreeDestroyed();
                }
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

    public void DestroyRandomTreeInMiddleState()
    {
        int index = Random.Range(0, aliveTrees.Count);
        Trees t = aliveTrees.ElementAt<Trees>(index);
        handleTreeDestroy(t);
    }

}
