using UnityEngine;

public class spawnController : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform spawnPool;
    public Transform[] objectPool;

    int spawnMax = 0;
    int pointer_spawnPool = 0;
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
        if(Input.GetKeyDown(KeyCode.P) && pointer_spawnPool < spawnMax)
        {
            Transform current = objectPool[pointer_spawnPool];
            current.position = spawnPoint.position;
            current.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
