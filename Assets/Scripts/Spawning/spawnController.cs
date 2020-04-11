using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public Transform center;
    public Transform spawnPool;
    public List<Interactable> spawnable;
    public List<Interactable> spawned;

    public float maxRadius = 20f;
    public float minRadius = 20f;
    float offsetY = 20;

    int spawnMax = 0;
    int pointer_spawnPool = 0;



    // Start is called before the first frame update
    void Start()
    {

        if(maxRadius < minRadius)
        {
            throw new System.Exception("SpawnController: maxRadius is smaller then the planed min Distance to the player");
        }


        spawnable = new List<Interactable>();
        spawnMax = spawnPool.childCount;
        for ( int i= 0; i < spawnMax; i++)
        {
            Interactable curObject = spawnPool.GetChild(i).GetComponent<Interactable>();
            spawnable.Add(curObject);
            curObject.SpawnMaster = this;
            curObject.PoolNumber = i;
        }

        spawned = new List<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.P))
        {
            if(spawnable.Count > 0)
            //if (pointer_spawnPool < objectPool.Length)
            {
                spawnObject();
            }
            else
            {
                Interactable current = spawned[0];
                spawned.RemoveAt(0);
                spawnable.Add(current);
                //pointer_spawnPool = 0;
                spawnObject();
            }
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
        Interactable current = spawnable[0];
        current.gameObject.transform.position = Point;
        current.Spawn(true);

        spawnable.RemoveAt(0);
        spawned.Add(current);

/*
        if(current.SpawnMaster == null)
        {
            current.PoolNumber = pointer_spawnPool;
            current.SpawnMaster = this;
        }


    // Pointer increses
        pointer_spawnPool++;
        */
    }

    public void despawnObjectWithID(int PoolNumber)
    {
        Interactable current = null;
        foreach(Interactable i in spawned)
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
