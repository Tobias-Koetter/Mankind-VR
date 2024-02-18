using System.Collections;
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

    public bool AreAllTreesDead{get{return aliveTrees.Count == 0;}}

    private TreeComparer tC;
    private GameInfo state;
    private bool isInvokingMiddle = false;
    private bool isInvokingEnd = false;

    private GameObject alive;
    private GameObject dead;
    // Start is called before the first frame update
    void Start()
    {
        tC = new TreeComparer();

        int counter = 0;
        alive = new GameObject("ALIVE",typeof(Transform));
        dead = new GameObject("DEAD",typeof(Transform));

        alive.transform.SetParent(treeParent.transform);
        dead.transform.SetParent(treeParent.transform);
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
                t.transform.SetParent(alive.transform);
            }
            print(aliveTrees.Count);
            float pNatVal= Trees.startingNatureValue;

            foreach (Trees t in aliveTrees)
            {
                t.personalNatureValue = pNatVal;
            }
            //Debug.LogWarning(pNatVal + "||" + counter);
            LevelBalancing.ResetBalanceValue(pNatVal * counter);
        }

        counter = 0;

        if (PoIParent == null)
        {
            //Debug.LogError("POIParent is not selected in Editor.");
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

    public void handleTreeDestroy(Trees t, int index = -1)
    {
        int treeIndex;
        if (index > -1)                     // is the index of the tree in the aliveTrees already known (index >= 0) or need it be checked now?
        {
            treeIndex = index;
        }
        else                                         
        {
            //Debug.Log($"In DestroyVegetation because of {t}.");
            treeIndex = aliveTrees.BinarySearch(t, tC);
            //Debug.Log($"Found {t} at position {treeIndex}.");
        }
        if(treeIndex >= 0)
        {
            if(!t.lDis.isDissolving)
                t.Interact();
            if(t.status == TREE_STAGE.ALIVE2)
            {
                float substractionValue = Trees.startingNatureValue/100f*30f;      // gets reduced by 30% from original value 
                //Debug.Log("StartingNatureVal: " + Trees.startingNatureValue + "Trees.startingNatureValue/100f*30f: " + substractionValue);
                t.personalNatureValue -= substractionValue;
                LevelBalancing.SetNatureValue(substractionValue); 
            }
            if (t.status == TREE_STAGE.BETWEEN1 || t.status == TREE_STAGE.BETWEEN2)
            {
                float substractionValue = Trees.startingNatureValue / 100f * 20f; // gets reduced by 20% from original value
                t.personalNatureValue -= substractionValue;
                LevelBalancing.SetNatureValue(substractionValue);
            }
            else if (t.status == TREE_STAGE.DEAD)
            {
                //aliveTrees.RemoveAt(treeIndex);
                aliveTrees.Remove(t);
                deadTrees.Add(t);
                t.transform.SetParent(dead.transform);
                deadTrees.Sort(tC);
                LevelBalancing.SetNatureValue(t.personalNatureValue);            // gets reduced by remaining 30% from original value
                t.personalNatureValue = 0f;
                if (deadTrees.Count == 1)
                {
                    state.SwitchFirstTreeDestroyed();
                }
            }
        }
    }

    protected IEnumerator DestroyOverMoreStates(Trees t, int amountOfStates, int index = -1)
    {
        // number of destroys that need to be performed for the current Tree
        int numberOfStates;

        // check status with comparing the current stage of the given Tree (nrCurState) and the last stage possible (nrLastState)
        int nrCurState = (int)t.status;
        int nrLastState = (int)TREE_STAGE.DEAD;

        int val = t.jumpOverBetween2 ? 1 : 0;
        if( amountOfStates >=  ( nrLastState - nrCurState  - val)  )  // check if there are enough States to destroy for the given number of States
        {
            numberOfStates = (nrLastState - nrCurState - val);        // if not: destroy whole tree
        }
        else{ numberOfStates = amountOfStates; }                // if yes: setup for destroy secence with given number of States

        bool firstIteration = true;
        while(numberOfStates > 0)
        {
            if(firstIteration)
            {
                numberOfStates--;
                firstIteration = false;
                handleTreeDestroy(t, index);
            }
            yield return new WaitWhile(() => t.lDis.isDissolving);
            if(nrCurState != (int)t.status)
            {
                numberOfStates--;
                nrCurState = (int)t.status;
                handleTreeDestroy(t, index);
            }
        }

        yield return null;

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
        if (!AreAllTreesDead)
        {
            int index = Random.Range(0, aliveTrees.Count);
            Trees t = aliveTrees.ElementAt<Trees>(index);
            //handleTreeDestroy(t,index);
            //StartCoroutine(DestroyOverMoreStates(t, 3, index));
            StartCoroutine(DestroyOverMoreStates(t, 3));
        }
    }

    public void DestroyRandomTreeInRisingState()
    {
        DestroyRandomTreeInMiddleState();
        DestroyRandomTreeInMiddleState(); 
        DestroyRandomTreeInMiddleState();
        DestroyRandomTreeInMiddleState();
        DestroyRandomTreeInMiddleState();

    }

    public void DestroyTreesInAlive1()
    {
        if (!AreAllTreesDead)
        {
            List<Trees> alive1Trees = aliveTrees.Where(t => t.status == TREE_STAGE.ALIVE1).ToList();
            foreach (Trees t in alive1Trees)
            {
                handleTreeDestroy(t);
            }
        }
    }
    public void DestroyTreesInAlive2()
    {
        if (!AreAllTreesDead)
        {
            List<Trees> alive1Trees = aliveTrees.Where(t => t.status == TREE_STAGE.ALIVE2).ToList();
            foreach (Trees t in alive1Trees)
            {
                handleTreeDestroy(t);
            }
        }
    }
    public void DestroyTreesInBetween1()
    {
        if (!AreAllTreesDead)
        {
            List<Trees> alive1Trees = aliveTrees.Where(t => t.status == TREE_STAGE.BETWEEN1).ToList();
            foreach (Trees t in alive1Trees)
            {
                handleTreeDestroy(t);
            }
        }
    }
    public void DestroyTreesInBetween2()
    {
        if (!AreAllTreesDead)
        {
            List<Trees> alive1Trees = aliveTrees.Where(t => t.status == TREE_STAGE.BETWEEN2).ToList();
            foreach (Trees t in alive1Trees)
            {
                handleTreeDestroy(t);
            }
        }
    }
}
