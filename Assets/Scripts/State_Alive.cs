
using System.Transactions;
using UnityEngine;

public class State_Alive : AbstractState
{
    
    
    private readonly float[] spawnTimings;
    private int TimingPointer;

    public State_Alive(GameInfo info) : base(info)
    {
        NextState = new State_StartDecay(info);
        Name = STATE.NATURE;
        SecondsToStateChange = 120f;
        spawnTimings = new float[] { 8f, 5f };
        TimingPointer = 0;
        SecondsToSpawnTrash = spawnTimings[TimingPointer];
    }

    public override AbstractState UpdateState()
    {
        // anchor: ran out of Time in this state 
        if (GameInfo.spentSecondsIngame >= SecondsToStateChange)
        {
            return NextState;
        }
        // else spawn small Trash every time spawnTiming is hit;
        else
        {
            SpawnTrashOverTime();
        }

        return this;

    }

    public void UpdateTreeDestroyStatus(bool newValue)
    {
        if(newValue)
        {
            TimingPointer = 1;
        }
        else
        {
            TimingPointer = 0;
        }
        SecondsToSpawnTrash = spawnTimings[TimingPointer];
    }
}
