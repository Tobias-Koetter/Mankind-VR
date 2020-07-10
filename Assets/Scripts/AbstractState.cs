﻿using System.Collections;
using UnityEngine;

public enum ExecutionState
{
    NONE,
    ACTIVE,
    COMPLETED,
    TERMINATED,
};

public abstract class AbstractState
{
    public GameInfo GameInfo { get; protected set; }
    public AbstractState NextState { get; protected set; }
    public ExecutionState ExecutionState { get; protected set; }
    public STATE Name { get; protected set; }
    public float SecondsToSpawnTrash { get; protected set; }
    public int LastTimeForSpawn { get; protected set; } = -1;
    public float SecondsToStateChange { get; protected set; } = 1000f;

    protected AbstractState(GameInfo info)
    {
        GameInfo = info;
        ExecutionState = ExecutionState.NONE;
        Name = STATE.NONE;
        SecondsToSpawnTrash = 100f;
    }

    public virtual bool EnterState() 
    {
        ExecutionState = ExecutionState.ACTIVE;
        return true;
    }

    public abstract AbstractState UpdateState();

    public virtual bool ExitState() 
    {
        ExecutionState = ExecutionState.COMPLETED;
        return true; 
    }

    protected bool SpawnTrashOverTime()
    {
        int curMod = Mathf.FloorToInt(GameInfo.spentSecondsIngame % SecondsToSpawnTrash);
        int curInt = Mathf.FloorToInt(GameInfo.spentSecondsIngame / SecondsToSpawnTrash);
        if (curInt != LastTimeForSpawn && curMod == 0)
        {
            Debug.Log("Spawn in "+this.Name + " at time:"+ GameInfo.spentSecondsIngame);
            GameInfo.TrashSpawner.spawnOnTimer();
            LastTimeForSpawn = curInt;
            return true;
        }
        return false;
    }
}