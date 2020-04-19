using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public Transform center;
    public Transform[] spawnPool;
    public List<Spawned> spawnable;
    public List<Spawned> spawned;

    public float maxRadius = 20f;
    public float minRadius = 20f;

    private readonly float offsetY = 1;



    void Start()
    {

        if(maxRadius < minRadius)
        {
            throw new System.Exception("SpawnController: maxRadius is smaller then the planed min Distance to the player");
        }


        spawnable = new List<Spawned>();

        // serves as a unique number for every trash element
        int counter = 0;

        foreach(Transform spP in spawnPool)
        {
            for (int i = 0; i < spP.childCount; i++)
            {
                Spawned curObject = spP.GetChild(i).GetComponent<Spawned>();
                spawnable.Add(curObject);
                curObject.SpawnMaster = this;
                curObject.PoolNumber = counter++;
                curObject.gameObject.SetActive(false);
            }
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

/* -[Create Position behind Player]
 *   Find random vector on a unit Circle border and put it in a Vector3
 *   Random.insideUnitCircle returns Point inside a unit Circle and when normalized its placed on the borderline of the unit circle
 *   is facing (negative forward vector + created random direction)
 *   The calculated spawn should be behind the player, so he doesn't see the spawn.
 */
        Vector2 posXZ = Random.insideUnitCircle.normalized;
        Vector3 randDir = new Vector3(posXZ.x, 0f, posXZ.y);
        Vector3 tempPoint = center.position + ((-center.forward + randDir).normalized * curRadius);
        tempPoint.y = center.position.y + offsetY;
        Quaternion tempRotation = Random.rotation;


    // Object gets picked out of the SpawnPool and its components get reset for scene interaction
        Spawned current = spawnable[Random.Range(0,spawnable.Count)];
        current.gameObject.transform.position = tempPoint;
        current.gameObject.transform.rotation = tempRotation;
        current.gameObject.SetActive(true);

        spawnable.Remove(current);
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
        current.gameObject.transform.position = spawnPool[0].position;
        current.Interact();
        current.gameObject.SetActive(false);

        spawnable.Add(current);
        spawned.Remove(current);
    }
}