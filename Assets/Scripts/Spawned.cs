using UnityEngine;

public class Spawned : Interactable
{
    private SpawnController spawnMaster;
    public SpawnController SpawnMaster { get => spawnMaster; set => spawnMaster = value; }

    private int poolNumber = 0;
    public int PoolNumber { get => poolNumber; set => poolNumber = value; }

    public override void Interact()
    {
        
        print(">> i "+ this.name+" was forced to interact from someone else <<");
    }
}
