using System.Collections;
using UnityEngine;

[SelectionBase]
public class Spawned : Interactable
{
    public float personalTrashValue;
    public MeshRenderer meshRndr;
    public MeshFilter meshFltr;
    public TA_SHAPES TrashAreaShape = TA_SHAPES.NONE;
    public Area_Spawn currentArea = null;
    private LODGroup lodG;
    private SpawnController spawnMaster;
    public SpawnController SpawnMaster { get => spawnMaster; set => spawnMaster = value; }

    public int poolNumber = 0;
    public int PoolNumber { get => poolNumber; set => poolNumber = value; }

    public MeshRenderer getRenderer() => ownRenderer;
    public Rigidbody getBody() => ownRigidBody;

    new void Start()
    {
        base.Start();
        ownRenderer = meshRndr;
        ownFilter = meshFltr;
    }
    public override void Interact()
    {
        
        print(">> i "+ this.name+" was forced to interact from someone else <<");
    }

    public IEnumerator SetObjectToSleepAfterSecs()
    {
        //Debug.Log(this.name + " will stop after 3 secs to move");
        yield return new WaitForSeconds(5);
        //Debug.Log(this.name + " should have stopped");
        ownRigidBody.Sleep();
        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 31)
        {
            StartCoroutine(SetObjectToSleepAfterSecs());
        }
    }
}
