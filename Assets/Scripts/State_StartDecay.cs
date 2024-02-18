

using System.Collections;
using System.Dynamic;
using UnityEngine;

public class State_StartDecay : AbstractState
{
    private float lastbaBalanceUpdate;
    private float timeBetweenBalancing;

    public State_StartDecay(GameInfo info) : base(info)
    {
        NextState = new State_MainDecay(info);
        Name = STATE.DECAY_START;
        SecondsToStateChange = GlobalSettingsManager.GetStateTime(this.Name);
        this.SecondsToSpawnTrash = 2f;
    }
    // override because in need for tracking the state entrance time.
    public override bool EnterState()
    {
        base.EnterState();
        lastbaBalanceUpdate = -1f;
        timeBetweenBalancing = 2f;
        return true;
    }

    public override AbstractState UpdateState()
    {
        // anchor: ran out of Time in this state
        if (RemainingTimeInState >= SecondsToStateChange)
        {
            // return next AbstractState -> State_MainDecay
            return NextState;
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
                    
                    GameInfo.TrashSpawner.SpawnOnTimer();
                }
                else if (LevelBalancing.GetBalanceVariance() >= 0 && LevelBalancing.GetBalanceVariance() < 10f)
                {
                    Debug.Log("StartDecay -> Balancing is executing.");
                    GameInfo.PlantDestroyer.DestroyRandomTreeInMiddleState();
                }
                else if (LevelBalancing.GetBalanceVariance() >=10 && LevelBalancing.GetBalanceVariance()< 20f)
                {
                    Debug.Log("StartDecay -> Balancing is executing.");
                    GameInfo.PlantDestroyer.DestroyRandomTreeInMiddleState();
                    GameInfo.PlantDestroyer.DestroyRandomTreeInMiddleState();
                }
                else if(LevelBalancing.GetBalanceVariance() >= 20f)
                {
                    Debug.Log("StartDecay -> Balancing is executing.");
                    GameInfo.PlantDestroyer.DestroyRandomTreeInMiddleState();
                    GameInfo.PlantDestroyer.DestroyRandomTreeInMiddleState();
                    GameInfo.PlantDestroyer.DestroyRandomTreeInMiddleState();
                    GameInfo.PlantDestroyer.DestroyRandomTreeInMiddleState();
                }
                lastbaBalanceUpdate = GameInfo.SpentSecondsIngame;
            }
        }
        return this;
    }

    public override bool ExitState()
    {
        base.ExitState();
        StageChanging changer = GameInfo.changer;
        changer.StartCoroutine(changer.ChangeToStage2());
        return true;
    }

    private float LastBalanceInState()
    {
        return GameInfo.SpentSecondsIngame - lastbaBalanceUpdate;
    }
}
