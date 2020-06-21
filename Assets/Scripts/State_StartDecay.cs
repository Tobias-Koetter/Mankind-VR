

using System.Collections;
using UnityEngine;

public class State_StartDecay : AbstractState
{
    private float startTime;

    public State_StartDecay(GameInfo info) : base(info)
    {
        NextState = new State_MainDecay(info);
        Name = STATE.DECAY_START;
        SecondsToStateChange = 60f;
        this.SecondsToSpawnTrash = 3f;
    }
    // override because in need for tracking the state entrance time.
    public override bool EnterState()
    {
        base.EnterState();
        startTime = GameInfo.spentSecondsIngame;
        return true;
    }

    public override AbstractState UpdateState()
    {
        // anchor: ran out of Time in this state
        if ((GameInfo.spentSecondsIngame-startTime) >= SecondsToStateChange)
        {
            Debug.LogError("<||State_StartDecay||>: Next State is not implemented yet.");
            // return next AbstractState -> State_MainDecay
        }
        else
        {

        }
        return this;
    }
}
