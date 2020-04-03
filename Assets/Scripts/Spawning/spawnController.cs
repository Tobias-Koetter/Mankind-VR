using UnityEngine;

public class spawnController : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform spawnPool;
    public Transform[] objectPool;

    int spawnMax = 0;
    int pointer_spawnPool = 0;

    float spawnRadius = 20f;

    // Start is called before the first frame update
    void Start()
    {
        objectPool = new Transform[spawnPool.childCount];
        spawnMax = objectPool.Length;
        for ( int i= 0; i < objectPool.Length; i++)
        {
            objectPool[i] = spawnPool.GetChild(i);
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
        randPoint.z = Random.Range(spawnPoint.position.y - spawnRadius,
            spawnPoint.position.z + spawnRadius);

    // Object gets picked out of the SpawnPool and its components get reset for scene interaction
        Transform current = objectPool[pointer_spawnPool];
        current.position = randPoint;
        print(current.position);
        current.GetComponent<MeshRenderer>().enabled = true;
        current.GetComponent<MeshCollider>().enabled = true;
        //current.GetComponent<Rigidbody>().velocity = Vector3.zero;
        current.GetComponent<Rigidbody>().useGravity = true;
        pointer_spawnPool++;
    }
}
