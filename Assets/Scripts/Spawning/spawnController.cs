using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform spawnPool;
    public Interactable[] objectPool;

    int spawnMax = 0;
    int pointer_spawnPool = 0;

    float spawnRadius = 20f;

    // Start is called before the first frame update
    void Start()
    {
        objectPool = new Interactable[spawnPool.childCount];
        spawnMax = objectPool.Length;
        for ( int i= 0; i < objectPool.Length; i++)
        {
            objectPool[i] = spawnPool.GetChild(i).GetComponent<Interactable>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if (pointer_spawnPool < objectPool.Length)
            {
                spawnObject();
            }
            else
            {
                pointer_spawnPool = 0;
                spawnObject();
            }
        }
    }




    private void spawnObject()
    {

    // Spawn Object in a Circle around the set Spawnpoint with fixed y-Axis Value
    // Center = spawnPoint
    // Radius = spawnRadius

        Vector3 randPoint = Vector3.zero;
        randPoint.y = spawnPoint.position.y;

        randPoint.x = Random.Range(spawnPoint.position.x - spawnRadius,
            spawnPoint.position.x + spawnRadius);
        randPoint.z = Random.Range(spawnPoint.position.z - spawnRadius,
            spawnPoint.position.z + spawnRadius);

    // Object gets picked out of the SpawnPool and its components get reset for scene interaction
        Interactable current = objectPool[pointer_spawnPool];
        current.gameObject.transform.position = randPoint;
        print(current.gameObject.transform.position);
        current.Spawn(true);


        if(current.SpawnMaster == null)
        {
            current.PoolNumber = pointer_spawnPool;
            current.SpawnMaster = this;
        }


    // Pointer increses
        pointer_spawnPool++;
    }

    public void despawnObjectWithID(int PoolNumber)
    {
        Interactable current = objectPool[PoolNumber];
        current.Spawn(false);
        current.gameObject.transform.position = spawnPool.position;
        current.Interact();
    }
}
