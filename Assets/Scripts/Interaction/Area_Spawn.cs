using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_Spawn : MonoBehaviour
{
    [SerializeField] private List<Spawned> addedSmallTrash;
    [SerializeField] private List<Spawned> addedTAs;
    [SerializeField] private int threshold_MountainSpawn = 10;
    [SerializeField] private List<float> restingPosY;
    [SerializeField] private bool inMountainMode = false;
    [SerializeField] private List<GameObject> trashMountain;
    [SerializeField] private List<Spawned> insideColliders;
    [SerializeField] private string LayerToChangeTo;
    private List<MeshCollider> colliders;
    private List<float> trashtimings;
    private List<float> totalTimes;
    private List<Vector3> startPositions;
    private bool inActionState = false;
    
    public bool InMountainMode { get => inMountainMode; }

    private void Start() {
        trashtimings = new List<float>();
        totalTimes = new List<float>();
        startPositions = new List<Vector3>();
        colliders = new List<MeshCollider>();
        for (int i = 0; i <trashMountain.Count; i++)
        {
            GameObject g = trashMountain[i];
            g.SetActive(false);
            g.transform.Translate(Vector3.forward * restingPosY[i]);
            startPositions.Add(g.transform.position);
            trashtimings.Add(0f);
            totalTimes.Add(Random.Range(5f, 8f));
            colliders.Add(g.GetComponent<MeshCollider>());
        }
    }

    private void Update() {
        if(inActionState)
        {
            for(int i = 0; i < trashMountain.Count; i++)
            {
                trashtimings[i] += Time.deltaTime;
                trashMountain[i].transform.position = Vector3.Lerp(startPositions[i], Vector3.zero, trashtimings[i] / totalTimes[i]);
                if(addedSmallTrash.Count >0)
                {
                }
                if(addedTAs.Count >0)
                {
                }
            }
            bool endReached = true;
            for(int i = 0; i<trashtimings.Count;i++)
            {
                if(trashtimings[i] < totalTimes[i])
                {
                    endReached = false;
                    break;
                }
            }
            if(endReached)
            {
                int layerNr = LayerMask.NameToLayer(LayerToChangeTo);
                foreach(GameObject gO in trashMountain)
                {
                    gO.layer = layerNr;
                    /*foreach(Transform child in gO.transform)
                    {
                        child.gameObject.layer = layerNr;
                    }*/
                }
                for (int i = insideColliders.Count - 1; i >= 0; i--)
                {
                    Spawned spawned = insideColliders[i];
                    insideColliders.RemoveAt(i);
                    if (spawned.TrashAreaShape == TA_SHAPES.NONE)
                    {
                        //spawned.SpawnMaster.DespawnObjectWithID(spawned.poolNumber);
                    }
                    else
                        spawned.SpawnMaster.DespawnTrashArea(spawned);
                }
                /*
                for (int i = addedTAs.Count - 1; i >= 0; i--)
                {
                    //addedasTAs[i].SpawnMaster.DespawnTrashArea(addedTAs[i]);
                }*/
                inActionState = false;
            }
        }
    }

    public void AddSpawnObject(Spawned spawned) {
        if (spawned.TrashAreaShape == TA_SHAPES.NONE){
            addedSmallTrash.Add(spawned);
            
        }
        else {
            addedTAs.Add(spawned);
            if(addedTAs.Count > threshold_MountainSpawn)
            {
                inMountainMode = true;
                inActionState = true;
                foreach (GameObject g in trashMountain)
                    g.SetActive(true);
            }
        }
        spawned.currentArea = this;
    }
    public void RemoveSpawnObject(Spawned spawned){
        if (spawned.TrashAreaShape == TA_SHAPES.NONE) {
            addedSmallTrash.Remove(spawned);
        }
        else {
            addedTAs.Remove(spawned);
        }
        spawned.currentArea = null;

    }
    public void MoveUpManually() {
        inMountainMode = true;
        inActionState = true;
        foreach (GameObject g in trashMountain)
            g.SetActive(true);
    }
    public void MarkAsInsideCollider(Spawned spawned) {
        insideColliders.Add(spawned);
    }
}