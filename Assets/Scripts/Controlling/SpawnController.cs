using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SpawnedList
{
    public List<Spawned> listObjects;

    public SpawnedList()
    {
        listObjects = new List<Spawned>();
    }

    public Spawned this[int key] { get => listObjects[key]; set => listObjects[key] = value; }
    public void Add(Spawned item) => listObjects.Add(item);
    public int Count => listObjects.Count;
    public void RemoveAt(int index) => listObjects.RemoveAt(index);
    public bool Remove(Spawned item) => listObjects.Remove(item);
    public int BinarySearch(Spawned item) => listObjects.BinarySearch(item);



}

public class SpawnController : MonoBehaviour
{
    [System.Serializable]
    public struct AreaOfSpawns
    {
        public MeshFilter[] AreaFilter;
        public Area_Spawn[] AreaScript;
    }
    public bool VisibleDebug;
    public Transform[] center;
    private AreaOfSpawns spawnArea;
    public int totalNrCopies = 100;
    public float maxSpawnRadius = 20f;
    public float minSpawnRadius = 20f;
    public Spawned[] Prefabs_SmallTrash;
    public SpawnedList[] spawnable_SmallTrash;
    public SpawnedList[] spawned_SmallTrash;

    public Spawned[] Prefabs_TrashAreas;
    public SpawnedList[] spawnable_TrashAreas;
    public SpawnedList[] spawned_TrashAreas;
    public ParticleSystem DirtThrow;

    public SpawnAreaTracker SpArTracker;
    public bool cantSpawnTrashareas = false;
    //private ParticleSystem dTInstance;

    private SpawnTrashArea trashAreaHandler;
    private KeyCode[] codes;

    private Spawned last;

    private int maxSpawnIndexSmallTrash = 0;
    private int minSpawnIndexTrashAreas = 0;
    private int maxSpawnIndexTrashAreas = 0;
    private bool TrashAreasCanSpawn = false;

    


    private readonly float offsetY = 1;



    void Start()
    {
        spawnArea.AreaFilter = new MeshFilter[center.Length];
        spawnArea.AreaScript = new Area_Spawn[center.Length];

        for (int i = 0; i < center.Length; i++)
        {
            spawnArea.AreaFilter[i] = center[i].gameObject.GetComponent<MeshFilter>();
            spawnArea.AreaScript[i] = center[i].gameObject.GetComponent<Area_Spawn>();
        }

        if (maxSpawnRadius < minSpawnRadius)
        {
            throw new System.Exception("SpawnController: maxRadius is smaller then the planed min Distance to the player.");
        }

        int counter = 0;

        // Trash Setup before
        // spawnable_SmallTrash = new List<Spawned>();
        // 
        // // serves as a unique number for every trash element
        // foreach (Spawned sp in Prefabs_SmallTrash)
        // {
        //     for (int i = 0; i < totalNrCopies; i++)
        //     {
        //         Spawned curObject = Instantiate<Spawned>(sp);
        //         spawnable_SmallTrash.Add(curObject);
        //         curObject.SpawnMaster = this;
        //         curObject.PoolNumber = counter++;
        //         curObject.gameObject.SetActive(false);
        //     }
        // }
        // spawned_SmallTrash = new List<Spawned>();
        // -----------------------------------


        // Create jagged Arrays for TrashArea Pooling
        spawnable_SmallTrash = new SpawnedList[Prefabs_SmallTrash.Length];
        spawned_SmallTrash= new SpawnedList[Prefabs_SmallTrash.Length];

        for (int i = 0; i < Prefabs_SmallTrash.Length; i++)
        {
            SpawnedList spawnable_temp = new SpawnedList();
            for (int j = 0; j < totalNrCopies; j++)
            {
                Spawned newSmallObject = Instantiate<Spawned>(Prefabs_SmallTrash[i]);
                spawnable_temp.Add(newSmallObject);
                newSmallObject.SpawnMaster = this;
                newSmallObject.PoolNumber = counter++;
                newSmallObject.gameObject.SetActive(false);
            }
            spawnable_SmallTrash[i] = spawnable_temp;

            SpawnedList spawned_temp = new SpawnedList();
            spawned_SmallTrash[i] = spawned_temp;
        }
        /*dTInstance = GameObject.Instantiate(DirtThrow);
        dTInstance.transform.position = Vector3.up * -10f;
        dTInstance.Stop();*/

        codes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5};


        /*
         * Prepare 
         */
        trashAreaHandler = new SpawnTrashArea(VisibleDebug);

        // Create jagged Arrays for TrashArea Pooling
        spawnable_TrashAreas = new SpawnedList[Prefabs_TrashAreas.Length];
        spawned_TrashAreas = new SpawnedList[Prefabs_TrashAreas.Length];
        TA_SHAPES standard = TA_SHAPES.Plane_Small;
        for (int i = 0; i< Prefabs_TrashAreas.Length; i++)
        {
            SpawnedList spawnable_temp = new SpawnedList();
            for(int j= 0; j < totalNrCopies; j++)
            {
                Spawned newAreaObject = Instantiate<Spawned>(Prefabs_TrashAreas[i]);
                spawnable_temp.Add(newAreaObject);
                newAreaObject.SpawnMaster = this;
                newAreaObject.PoolNumber = counter++;
                newAreaObject.gameObject.SetActive(false);
                newAreaObject.TrashAreaShape = standard + i;
            }
            spawnable_TrashAreas[i] = spawnable_temp;

            SpawnedList spawned_temp = new SpawnedList();
            spawned_TrashAreas[i] = spawned_temp;
        }

        



    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                last = SpawnOnTimer();
            }
            else if (Input.GetKeyDown(KeyCode.O) && last != null)
            {
                DespawnObjectWithID(last.poolNumber);
            }
            else
            {
                for (int i = 0; i < codes.Length; i++)
                {

                    if (Input.GetKeyDown(codes[i]))
                    {
                        int indexSpawnArea = Random.Range(0, spawnArea.AreaFilter.Length);
                        SpawnSpecificTrashArea(i,indexSpawnArea);
                    }
                }
            }
        }

    }

    public Spawned SpawnOnTimer()
    {
        Spawned ret;
        if(TrashAreasCanSpawn)
        {
            int val = Random.Range(0, 100);
            if (val <= 16 * maxSpawnIndexTrashAreas)           // maxSpawnIndexTrashAres liegt zwischen 1 und 5 -> max Wahrscheinlichkeit bei 80% in letzter Stufe
            {
                ret = null;
                if (!cantSpawnTrashareas) { ret = SpawnTrashArea(); }
            }
            else
                ret = SpawnSmallTrash();
        }
        else
        {
            ret = SpawnSmallTrash();
        }
        return ret;
    }

    public Spawned SpawnSmallTrash()
    {
        Spawned ret = null;
        int index = Random.Range(0, maxSpawnIndexSmallTrash);       //Choose which Object gets spawned out of the small trash 

        if (spawnable_SmallTrash[index].Count >= spawnArea.AreaFilter.Length)
        {
            //spawnObject();
            for (int i= 0; i < spawnArea.AreaFilter.Length;i++)                // throw one small trash object in every spawnArea
            {
                if (!spawnArea.AreaScript[i].InMountainMode)
                {
                    ret = SpawnObjectInMesh(i,index);
                }
            }
        }
        else
        {
            for (int i = 0; i < spawnArea.AreaFilter.Length; i++)
            {
                Spawned current = spawned_SmallTrash[index][0];
                spawned_SmallTrash[index].RemoveAt(0);
                spawnable_SmallTrash[index].Add(current);
                LevelBalancing.SetTrashValue(current.personalTrashValue, false); // Trashvalue wird kurzfristig verringert
            }
            //spawnObject();
            for (int i = 0; i < spawnArea.AreaFilter.Length; i++)
            {
                if (!spawnArea.AreaScript[i].InMountainMode)
                {
                    ret = SpawnObjectInMesh(i, index);
                }
            }
        }
        return ret;
    }

    public Spawned SpawnTrashArea()
    {
        Spawned ret = null;
        int indexMeshForm = Random.Range(0, maxSpawnIndexTrashAreas);

        int indexSpawnArea = Random.Range(0, spawnArea.AreaFilter.Length);
        /*while( (counter < center.Length) 
            && 
            ( center[indexSpawnArea] == SpArTracker.getCurrentVisited()
            || spawnArea.AreaScript[indexSpawnArea].InMountainMode )
            )
        {
            counter++;
            indexSpawnArea = (indexSpawnArea + 1) % spawnArea.AreaFilter.Length;
        }*/
        if(spawnArea.AreaScript[indexSpawnArea].InMountainMode)
        {
            indexSpawnArea = -1;
            for(int i= 0; i< spawnArea.AreaFilter.Length; i++)
            {
                if(!spawnArea.AreaScript[i].InMountainMode)
                {
                    indexSpawnArea = i;
                }
            }
        }
        if (indexSpawnArea != -1)//(counter < center.Length)
        {
            SpawnSpecificTrashArea(indexMeshForm, indexSpawnArea);

            if (maxSpawnIndexTrashAreas >= 3 && maxSpawnIndexTrashAreas < 5)
            {
                Transform extraArea = SpArTracker.getVisitedArea();
                indexSpawnArea = System.Array.IndexOf<Transform>(center, extraArea);
                SpawnSpecificTrashArea(indexMeshForm, indexSpawnArea);
            }

            if (maxSpawnIndexTrashAreas == 5)
            {
                Transform extraArea = SpArTracker.getCurrentVisited();
                indexSpawnArea = System.Array.IndexOf<Transform>(center, extraArea);
                SpawnSpecificTrashArea(indexMeshForm, indexSpawnArea);
            }
        }
        else
        {
            cantSpawnTrashareas = true;
        }
        return ret;
    }

    public Spawned SpawnSpecificTrashArea(int indexForm, int indexSpawnArea)
    {
        Spawned ret = null;
        TA_SHAPES standard = TA_SHAPES.Plane_Small;

        SpawnedList spawnable_curList = spawnable_TrashAreas[indexForm];
        SpawnedList spawned_curList = spawned_TrashAreas[indexForm];

        Spawned curPopped;
        int lastPos;

        if (spawnable_curList.Count == 0)
        {
            lastPos = 0;
            curPopped = spawned_curList[0];
            spawned_curList.RemoveAt(0);
            spawnable_curList.Add(curPopped);
            LevelBalancing.SetTrashValue(curPopped.personalTrashValue, false); //normalUse == false => es wird der Wert reduziert
        }
        else
        {
            lastPos = spawnable_TrashAreas[indexForm].Count - 1;
            curPopped = spawnable_curList[lastPos];
        }

        curPopped.gameObject.SetActive(true);
        //trashAreaHandler.Spawn(current + i, spawnArea[/*Random.Range(0, spawnArea.Length)*/4], curPopped);
        
        bool spawnedCorrectly = trashAreaHandler.Spawn(standard + indexForm, spawnArea.AreaFilter[indexSpawnArea],spawnArea.AreaScript[indexSpawnArea], curPopped, Random.Range(100, 160));

        if (spawnedCorrectly)
        {
            StartCoroutine(GrowToSize(curPopped));
            spawnable_curList.RemoveAt(lastPos);
            spawned_curList.Add(curPopped);
            ret = curPopped;
            LevelBalancing.SetTrashValue(curPopped.personalTrashValue);
        }
        else if (!spawnedCorrectly)
        {
            curPopped.gameObject.SetActive(false);
        }

        return ret;
    }

    private Spawned SpawnObjectInMesh(int index, int trashArrayPos)
    {
        Mesh m = spawnArea.AreaFilter[index].mesh;
        Transform t = center[index];
        SpawnedList curRow = spawnable_SmallTrash[trashArrayPos];
        Spawned current = curRow[curRow.Count-1];

        Vector3 vec = m.GetRandomPointInsideConvex();
        vec = t.TransformPoint(vec);

        RaycastHit info;
        LayerMask mask = LayerMask.GetMask("Ground");

        if (Physics.Raycast(vec, Vector3.down, out info, Mathf.Infinity, mask))
        {
            current.gameObject.transform.position = info.point + Vector3.up * 0.2f;
            current.gameObject.transform.rotation = Random.rotation;
            current.gameObject.SetActive(true);
            curRow.Remove(current);
            spawned_SmallTrash[trashArrayPos].Add(current);

            spawnArea.AreaScript[index].AddSpawnObject(current);
            LevelBalancing.SetTrashValue(current.personalTrashValue);
            return current;
        }

        return null;
        

        
    }

    public void DespawnObjectWithID(int PoolNumber)
    {
        Spawned current = null;

        int row = PoolNumber / totalNrCopies;
        //Debug.Log(PoolNumber + "<----------->" + row);
        foreach (Spawned i in spawned_SmallTrash[row].listObjects)
        {
            if (i.PoolNumber == PoolNumber)
            {
                current = i;
                break;
            }
        }
        current.gameObject.transform.position = transform.position;
        current.Interact();
        current.currentArea.RemoveSpawnObject(current);
        current.gameObject.SetActive(false);
        current.currentArea = null;
        LevelBalancing.SetTrashValue(current.personalTrashValue, false); //normalUse == false => es wird der Wert reduziert
        spawnable_SmallTrash[row].Add(current);
        spawned_SmallTrash[row].Remove(current);

    }
    
    public void DespawnTrashArea(Spawned spawned) {
        int indexForm = spawned.TrashAreaShape - TA_SHAPES.Plane_Small;
        SpawnedList activeTAs = spawned_TrashAreas[indexForm];
        SpawnedList inactiveTAs = spawnable_TrashAreas[indexForm];
        spawned.gameObject.transform.position = transform.position;
        spawned.currentArea.RemoveSpawnObject(spawned);
        spawned.gameObject.SetActive(false);
        spawned.currentArea = null;

        LevelBalancing.SetTrashValue(spawned.personalTrashValue, false); //normalUse == false => es wird der Wert reduziert
        inactiveTAs.Add(spawned);
        activeTAs.Remove(spawned);
    }

    public bool MoveToNewState(STATE nextState)
    {
        switch(nextState)
        {
            case STATE.NATURE:
                maxSpawnIndexSmallTrash = 5;
                // Dosen, Zigarettenschachteln
                break;
            case STATE.DECAY_START:
                maxSpawnIndexSmallTrash = Prefabs_SmallTrash.Length;
                TrashAreasCanSpawn = true;
                maxSpawnIndexTrashAreas = 1;
                // + Reifen, Metallplatten, Trashareas 1.5
                break;
            case STATE.DECAY_MAIN:
                // + TrashAreas 2.0; weniger TA 1.5; weniger kleiner Müll näher an Spieler
                minSpawnIndexTrashAreas = 1;
                maxSpawnIndexTrashAreas = 2;
                break;
            case STATE.TRASH_RISING:
                // + TA 2.5;
                minSpawnIndexTrashAreas = 2;
                maxSpawnIndexTrashAreas = 4;
                break;
            case STATE.FINAL:
                // TA 3.0; fast kein kleiner Müll Mehr nur noch nahe Spieler
                maxSpawnIndexTrashAreas = Prefabs_TrashAreas.Length;
                break;
        }
        return true;
    }

    IEnumerator GrowToSize(Spawned currentPlaced)
    {
        // Mögliche Erweiterung: 
        //   -> Object wächst aus Mitte heraus zu voller Größe...
        /*
        currentPlaced.transform.localScale = Vector3.zero;
        Vector3 growing = Vector3.zero;
        for (float i = 0.02f; i <= 1f; i += 0.05f)
        {
            growing = Vector3.one * i;
            //Debug.Log("growing: " + growing+"/ i: "+i);
            currentPlaced.transform.localScale = growing;
            yield return new WaitForFixedUpdate();
        }
        currentPlaced.transform.localScale = Vector3.one;
        <-... bis hier. */

        Vector3 posTarget = currentPlaced.transform.position;
        Vector3 posStart = posTarget - Vector3.up*2f;
        Vector3 posCur = posStart;
        Vector3 help = Vector3.zero;

        ParticleSystem dTInstance = GameObject.Instantiate(DirtThrow);
        dTInstance.transform.position = currentPlaced.transform.position;
        dTInstance.transform.Translate(Vector3.up * 0.03f);

        currentPlaced.transform.position = posCur;

        dTInstance.Play();
        for(float value = 0f; value <= 1f; value+= Time.deltaTime* 1f)
        {
            help.y = (1 - value) * posStart.y + value * posTarget.y;
            help.x = posCur.x;
            help.z = posCur.z;

            posCur = help;
            currentPlaced.transform.position = posCur;
            yield return new WaitForFixedUpdate();
        }
        currentPlaced.transform.position = posTarget;
        Destroy(dTInstance.transform.GetChild(0).gameObject);
        yield return new WaitForSeconds(4f);

        Destroy(dTInstance.gameObject);
        yield return null;
    }

    public void ForceMountainSpawn(int index) {
        if (!spawnArea.AreaScript[index].InMountainMode)
            spawnArea.AreaScript[index].MoveUpManually();
    }
}
