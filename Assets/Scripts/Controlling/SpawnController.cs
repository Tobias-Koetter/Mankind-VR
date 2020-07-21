using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public Transform[] center;
    private MeshFilter[] spawnArea;
    public int totalNrCopies = 100;
    public float maxSpawnRadius = 20f;
    public float minSpawnRadius = 20f;
    public Spawned[] Prefabs_SmallTrash;
    public List<Spawned> spawnable_SmallTrash;
    public List<Spawned> spawned_SmallTrash;

    public Spawned[] Prefabs_TrashAreas;
    public List<Spawned>[] spawnable_TrashAreas;
    public List<Spawned>[] spawned_TrashAreas;

    private SpawnTrashArea trashAreaHandler;
    private KeyCode[] codes;



    private readonly float offsetY = 1;



    void Start()
    {
        spawnArea = new MeshFilter[center.Length];
        for (int i = 0; i < center.Length; i++)
        {
            spawnArea[i] = center[i].gameObject.GetComponent<MeshFilter>();
        }

        
        if (maxSpawnRadius < minSpawnRadius)
        {
            throw new System.Exception("SpawnController: maxRadius is smaller then the planed min Distance to the player.");
        }

        
        spawnable_SmallTrash = new List<Spawned>();

        // serves as a unique number for every trash element
        int counter = 0;
        foreach (Spawned sp in Prefabs_SmallTrash)
        {
            for (int i = 0; i < totalNrCopies; i++)
            {
                Spawned curObject = Instantiate<Spawned>(sp);
                spawnable_SmallTrash.Add(curObject);
                curObject.SpawnMaster = this;
                curObject.PoolNumber = counter++;
                curObject.gameObject.SetActive(false);
            }
        }
        spawned_SmallTrash = new List<Spawned>();


        codes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5};


        /*
         * Prepare 
         */
        trashAreaHandler = new SpawnTrashArea();

        // Create jagged Arrays for TrashArea Pooling
        spawnable_TrashAreas = new List<Spawned>[Prefabs_TrashAreas.Length];
        spawned_TrashAreas = new List<Spawned>[Prefabs_TrashAreas.Length];
        for (int i = 0; i< Prefabs_TrashAreas.Length; i++)
        {
            List<Spawned> spawnable_temp = new List<Spawned>();
            for(int j= 0; j < totalNrCopies; j++)
            {
                Spawned newAreaObject = Instantiate<Spawned>(Prefabs_TrashAreas[i]);
                spawnable_temp.Add(newAreaObject);
                newAreaObject.SpawnMaster = this;
                newAreaObject.PoolNumber = counter++;
                newAreaObject.gameObject.SetActive(false);
            }
            spawnable_TrashAreas[i] = spawnable_temp;

            List<Spawned> spawned_temp = new List<Spawned>();
            spawned_TrashAreas[i] = spawned_temp;
        }

        



    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                spawnOnTimer();
            }
            else
            {
                TA_SHAPES current = TA_SHAPES.Plane_Small;
                for (int i = 0; i < codes.Length; i++)
                {

                    if (Input.GetKeyDown(codes[i]))
                    {
                        List<Spawned> spawnable_curList = spawnable_TrashAreas[i];
                        List<Spawned> spawned_curList = spawned_TrashAreas[i];

                        int lastPos = spawnable_TrashAreas[i].Count - 1;
                        Spawned curPopped = spawnable_curList[lastPos];

                        

                        curPopped.gameObject.SetActive(true);
                        //trashAreaHandler.Spawn(current + i, spawnArea[/*Random.Range(0, spawnArea.Length)*/4], curPopped);
                        bool spawnedCorrectly = false;
                        spawnedCorrectly = trashAreaHandler.Spawn(current + i, spawnArea[Random.Range(0, spawnArea.Length)], curPopped);

                        if(spawnedCorrectly)
                        {
                            spawnable_curList.RemoveAt(lastPos);
                            spawned_curList.Add(curPopped);
                        }
                        else if(!spawnedCorrectly)
                        {
                            curPopped.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

    }


    public void spawnOnTimer()
    {
        if (spawnable_SmallTrash.Count > 0)
        {
            //spawnObject();
            for (int i= 0; i < spawnArea.Length;i++)
            {

                spawnObjectInMesh(i);
            }
        }
        else
        {
            Spawned current = spawned_SmallTrash[0];
            spawned_SmallTrash.RemoveAt(0);
            spawnable_SmallTrash.Add(current);
            //spawnObject();
            for (int i = 0; i < spawnArea.Length; i++)
            {

                spawnObjectInMesh(i);
            }
        }
    }

    private void spawnObject()
    {

        // Spawn Object in a Circle around the set Spawnpoint with fixed y-Axis Value
        // Center = spawnPoint
        // Radius = between minRadius and maxRadius (min/max Distance to the player)


        // Find Random radius size between possible min and max
        float curRadius = Random.Range(minSpawnRadius, maxSpawnRadius);

        /* -[Create Position behind Player]
         *   Find random vector on a unit Circle border and put it in a Vector3
         *   Random.insideUnitCircle returns Point inside a unit Circle and when normalized its placed on the borderline of the unit circle
         *   is facing (negative forward vector + created random direction)
         *   The calculated spawn should be behind the player, so he doesn't see the spawn.
         */
        Vector2 posXZ = Random.insideUnitCircle.normalized;
        Vector3 randDir = new Vector3(posXZ.x, 0f, posXZ.y);
        Vector3 tempPoint = center[0].position + ((-center[0].forward + randDir).normalized * curRadius);
        tempPoint.y = center[0].position.y + offsetY;
        Quaternion tempRotation = Random.rotation;


        // Object gets picked out of the SpawnPool and its components get reset for scene interaction
        Spawned current = spawnable_SmallTrash[Random.Range(0, spawnable_SmallTrash.Count)];
        current.gameObject.transform.position = tempPoint;
        current.gameObject.transform.rotation = tempRotation;
        current.gameObject.SetActive(true);

        spawnable_SmallTrash.Remove(current);
        spawned_SmallTrash.Add(current);
    }

    private void spawnObjectInMesh(int index)
    {
        Mesh m = spawnArea[index].mesh;
        Transform t = center[index];
        Spawned current = spawnable_SmallTrash[Random.Range(0, spawnable_SmallTrash.Count)];

        Vector3 vec = m.GetRandomPointInsideConvex();
        vec = t.TransformPoint(vec);

        RaycastHit info;
        LayerMask mask = LayerMask.GetMask("Ground");

        if (Physics.Raycast(vec, Vector3.down, out info, Mathf.Infinity, mask))
        {
            current.gameObject.transform.position = info.point + Vector3.up * 0.2f;
            current.gameObject.transform.rotation = Random.rotation;
            current.gameObject.SetActive(true);
            spawnable_SmallTrash.Remove(current);
            spawned_SmallTrash.Add(current);
        }


        LevelBalancing.SetTrashValue(current.personalTrashValue);
    }

    public void despawnObjectWithID(int PoolNumber)
    {
        Spawned current = null;
        foreach (Spawned i in spawned_SmallTrash)
        {
            if (i.PoolNumber == PoolNumber)
            {
                current = i;
                break;
            }
        }
        current.gameObject.transform.position = transform.position;
        current.Interact();
        current.gameObject.SetActive(false);

        spawnable_SmallTrash.Add(current);
        spawned_SmallTrash.Remove(current);
    }
}
