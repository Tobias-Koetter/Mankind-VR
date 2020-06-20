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

    public AbstractState nextState { get; protected set; }
    public ExecutionState ExecutionState { get; protected set; }
    public STATE name { get; protected set; }
    public float secondsToSpawnTrash { get; protected set; }

    protected AbstractState()
    {
        ExecutionState = ExecutionState.NONE;
        name = STATE.NONE;
        secondsToSpawnTrash = 100f;
    }

    public virtual bool EnterState() 
    {
        ExecutionState = ExecutionState.ACTIVE;
        return true;
    }

    public abstract AbstractState UpdateState(GameState info);

    public virtual bool ExitState() 
    {
        ExecutionState = ExecutionState.COMPLETED;
        return true; 
    }
}
