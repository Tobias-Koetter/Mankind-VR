using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public Transform center;
    public Transform spawnPool;
    public List<Spawned> spawnable;
    public List<Spawned> spawned;

    public float maxRadius = 20f;
    public float minRadius = 20f;

    private readonly float offsetY = 2;

    private int spawnMax = 0;



    void Start()
    {

        if(maxRadius < minRadius)
        {
            throw new System.Exception("SpawnController: maxRadius is smaller then the planed min Distance to the player");
        }


        spawnable = new List<Spawned>();
        spawnMax = spawnPool.childCount;
        for ( int i= 0; i < spawnMax; i++)
        {
            Spawned curObject = spawnPool.GetChild(i).GetComponent<Spawned>();
            spawnable.Add(curObject);
            curObject.SpawnMaster = this;
            curObject.PoolNumber = i;
        }

        spawned = new List<Spawned>();
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.P))
        {
            spawnOnTimer();
        }
    }


    public void spawnOnTimer()
    {
        if (spawnable.Count > 0)
        {
            spawnObject();
        }
        else
        {
            Spawned current = spawned[0];
            spawned.RemoveAt(0);
            spawnable.Add(current);
            spawnObject();
        }
    }

    private void spawnObject()
    {

    // Spawn Object in a Circle around the set Spawnpoint with fixed y-Axis Value
    // Center = spawnPoint
    // Radius = between minRadius and maxRadius (min/max Distance to the player)


    // Find Random radius size between possible min and max
        float curRadius = Random.Range(minRadius, maxRadius);

    // Find random vector on a unit Circle border and put it in a Vector3
    // Random.insideUnitCircle returns Point inside a unit Circle and when normalized its placed on the borderline of the unit circle
        Vector2 posXZ = Random.insideUnitCircle.normalized;
        Vector3 randDir = new Vector3(posXZ.x, 0f, posXZ.y);
    // The spawn vector is calculated from the point vector (center) and the direction vector, considering in which direction the player is facing (negative forward vector + created random direction )
        Vector3 Point = center.position + ((-center.forward + randDir).normalized * curRadius);
        Point.y = center.position.y + offsetY;

    // The calculated spawn should be behind the player, so he doesn't see the spawn.


    // Object gets picked out of the SpawnPool and its components get reset for scene interaction
        Spawned current = spawnable[0];
        current.gameObject.transform.position = Point;
        current.Spawn(true);

        spawnable.RemoveAt(0);
        spawned.Add(current);
    }

    public void despawnObjectWithID(int PoolNumber)
    {
        Spawned current = null;
        foreach(Spawned i in spawned)
        {
            if (i.PoolNumber == PoolNumber)
            {
                current = i;
                break;
            }
        }
        current.Spawn(false);
        current.gameObject.transform.position = spawnPool.position;
        current.Interact();

        spawnable.Add(current);
        spawned.Remove(current);
    }
}