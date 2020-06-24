

using System.Collections;
using System.Dynamic;
using UnityEngine;

public class State_StartDecay : AbstractState
{
    private float startTime;
    private float lastbaBalanceUpdate;
    private float timeBetweenBalancing;

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
        lastbaBalanceUpdate = -1f;
        timeBetweenBalancing = 2f;
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
            //Debug.Log("LevelBalancing: " + LevelBalancing.GetBalanceVariance()+"\nLastBalanceInState: "+ LastBalanceInState());
            if (!SpawnTrashOverTime()                                                           // If the normal SpawnOverTime logic takes place, skip this step
                && (LastBalanceInState() >= timeBetweenBalancing || lastbaBalanceUpdate < 0f))  // Check if enough time has elapsed since last Balance spawn. Or its the first Balance spawn
            {
                Debug.Log("Balancing is triggered");
                if (LevelBalancing.GetBalanceVariance() < 0f )    // Check if there is need for Balance | Look up description in LevelBalancing Class for more info
               
                {
                    
                    GameInfo.TrashSpawner.spawnOnTimer();
                }
                else if(LevelBalancing.GetBalanceVariance() > Trees.startingNatureValue /2f)
                {
                    GameInfo.PlantDestroyer.DestroyRandomTreeInMiddleState();
                }
                lastbaBalanceUpdate = GameInfo.spentSecondsIngame;
            }
        }
        return this;
    }


    private float LastBalanceInState()
    {
        return GameInfo.spentSecondsIngame - lastbaBalanceUpdate;
    }
}
