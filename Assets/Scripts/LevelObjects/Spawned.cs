using System.Dynamic;
using UnityEngine;

public class Spawned : Interactable
{
    public MeshRenderer mesh;

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
        ownRenderer = mesh;
    }
    public override void Interact()
    {
        
        print(">> i "+ this.name+" was forced to interact from someone else <<");
    }
}
