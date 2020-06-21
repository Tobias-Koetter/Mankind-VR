using System.Collections;
using UnityEngine;

public enum ExecutionState
{
    NONE,
    ACTIVE,
    COMPLETED,
    TERMINATED,
};

public abstract class AbstractState : ScriptableObject
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
        int curMod = Mathf.FloorToInt(GameInfo.Timer % SecondsToSpawnTrash);
        int curInt = Mathf.FloorToInt(GameInfo.Timer / SecondsToSpawnTrash);
        if (curInt != LastTimeForSpawn && curMod == 0)
        {
            //print("Spawn in STATE START");
            GameInfo.TrashSpawner.spawnOnTimer();
            LastTimeForSpawn = curInt;
            return true;
        }
        return false;
    }

    protected IEnumerator SpawnTrashforBalance()
    {
        if (!SpawnTrashOverTime() && LevelBalancing.GetBalanceVariance() > 0)
        {
            GameInfo.TrashSpawner.spawnOnTimer();
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;

    }
}
