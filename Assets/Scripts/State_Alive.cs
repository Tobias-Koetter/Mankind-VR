
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
        SecondsToStateChange = GlobalSettingsManager.GetStateTime(this.Name);
        spawnTimings = new float[] { 8f, 5f };
        TimingPointer = 0;
        SecondsToSpawnTrash = spawnTimings[TimingPointer];
    }
    public override bool EnterState()
    {
        return base.EnterState();
    }
    public override AbstractState UpdateState()
    {
        // anchor: ran out of Time in this state 
        if (RemainingTimeInState >= SecondsToStateChange)
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

    public override bool ExitState()
    {
        base.ExitState();
        StageChanging changer = GameInfo.changer;
        changer.StartCoroutine(changer.ChangeToStage1_5());
        return true;
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
