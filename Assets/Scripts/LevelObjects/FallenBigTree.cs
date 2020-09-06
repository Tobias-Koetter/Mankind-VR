using System.Collections;
using System.Collections.Generic;
using System.Deployment.Internal;
using UnityEngine;

public class FallenBigTree : PoI
{
    [SerializeField]
    private BrokenTreePart[] parts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BrokenTreePart GetTreePartByID(int id)
    {
        BrokenTreePart found = null;
        foreach (BrokenTreePart bTP in parts)
        {
            if(bTP.GetPartID == id)
            {
                found = bTP;
                break;
            }
        }
        return found;
    }

    public void DestroyPart(BrokenTreePart current)
    {
        BrokenTreePart before = current.childToDestroyFirst;
        if(before != null && before.Alive)
        {
            DestroyPart(before);
        }
        if (current.lDis != null)
        {
            current.lDis.startDissovle();
        }
        StartCoroutine(WaitForDissolve(current));
    }

    IEnumerator WaitForDissolve(BrokenTreePart current)
    {
        if (current.lDis != null)
        {
            yield return new WaitWhile(() => current.lDis.isDissolving);
        }
        current.SetAliveState(false);
        yield return null;
    }
}
