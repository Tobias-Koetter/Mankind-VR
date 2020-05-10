using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceTrashModel : MonoBehaviour
{
    public GameObject biggerTrash;

    private Spawned ownSpawnScript;
    private float verticalAdjustmant = 0.16f;
    private int groundLayerNR = 31;
    private int replaceTreshold = 2;

    private bool unChecked = true;
    

    private void Start()
    {
        ownSpawnScript = this.gameObject.GetComponent<Spawned>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        unChecked = false;
        handleReplacementLogic(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if(unChecked)
        {
            unChecked = false;
            handleReplacementLogic(collision);
        }
    }


    private void handleReplacementLogic(Collision c)
    {
        if (c.collider.gameObject.layer == groundLayerNR)
        {
            List<Spawned> found = new List<Spawned>();
            if (CheckForOtherTrash(ref found))
            {
                ChangeModels(ref found);
            }
        }
    }

    private bool CheckForOtherTrash(ref List<Spawned> found)
    {
        Vector3 mid = transform.position;
        float dist = 4f;
        LayerMask interactableCheck = LayerMask.GetMask("Trash");
        Collider[] temp = Physics.OverlapSphere(mid, dist, interactableCheck);
        if (temp.Length >= replaceTreshold)
        {
            print($"there are at least 3 or more collider in range of [{ownSpawnScript.poolNumber}]");
            foreach (Collider c in temp)
            {
                if(c.gameObject.CompareTag("Small Trash"))
                {
                    Spawned s = c.gameObject.GetComponent<Spawned>();
                    if (s.poolNumber != ownSpawnScript.poolNumber && !found.Contains(s))
                    {
                        print($"Call from [{ownSpawnScript.poolNumber}]-> despawn: [{s.poolNumber}]");
                        found.Add(s);
                    }
                }
            }
            print("-----------------");
            if (found.Count >= replaceTreshold)
            {
                print("there are 3 or more Small Trash Objects in range");
                return true;
            }
        }
        return false;
    }

    private void ChangeModels(ref List<Spawned> found)
    {
        
        foreach(Spawned s in found)
        {
            print($"NR [{s.poolNumber}] gets passed back to ready pool");
            s.SpawnMaster.despawnObjectWithID(s.PoolNumber);
            print($"NR [{s.poolNumber}] -> done!");
        }
        GameObject newTr = GameObject.Instantiate(biggerTrash);

        newTr.transform.position = transform.position;
        newTr.transform.position -= Vector3.up * verticalAdjustmant;

        float rotVal = Random.Range(0f, 360f);
        Quaternion newRot = Quaternion.Euler(0f, rotVal, 0f);
        newTr.transform.rotation = newRot;

        ownSpawnScript.SpawnMaster.despawnObjectWithID(ownSpawnScript.PoolNumber);
    }
}
