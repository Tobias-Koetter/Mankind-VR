using UnityEngine;

public class State_MainDecay : AbstractState
{
    private float lastbaBalanceUpdate;
    private float timeBetweenBalancing;

    public State_MainDecay(GameInfo info) : base(info)
    {
        NextState = new State_TrashRising(info);
        Name = STATE.DECAY_MAIN;
        SecondsToStateChange = GlobalSettingsManager.GetStateTime(this.Name);
        this.SecondsToSpawnTrash = 1.5f;  
    }

    public override bool EnterState()
    {
        base.EnterState();
        lastbaBalanceUpdate = -1f;
        timeBetweenBalancing = 1f;
        GameInfo.animatorGlobalVolume.SetBool("ToOne", true);
        return true;
    }

    public override AbstractState UpdateState()
    {
        // anchor: ran out of Time in this state
        if (RemainingTimeInState >= SecondsToStateChange)
        {
            // return next AbstractState -> State_TrashRising
            return NextState;
        }

        //Debug.Log("LevelBalancing: " + LevelBalancing.GetBalanceVariance()+"\nLastBalanceInState: "+ LastBalanceInState());
        if (!SpawnTrashOverTime()                                                           // If the normal SpawnOverTime logic takes place, skip this step
            && (LastBalanceInState() >= timeBetweenBalancing || lastbaBalanceUpdate < 0f))  // Check if enough time has elapsed since last Balance spawn. Or its the first Balance spawn
        {
            //Debug.Log("Balancing is triggered");
            if (LevelBalancing.GetBalanceVariance() < 0f)    // Check if there is need for Balance | Look up description in LevelBalancing Class for more info

            {

                GameInfo.TrashSpawner.SpawnOnTimer();
            }
            else if (LevelBalancing.GetBalanceVariance() > Trees.startingNatureValue / 2f)
            {
                //Debug.Log("Balancing is executing.");
                GameInfo.PlantDestroyer.DestroyRandomTreeInMiddleState();
            }
            lastbaBalanceUpdate = GameInfo.SpentSecondsIngame;
        }
        return this;
    }

    public override bool ExitState()
    {
        base.ExitState();
        StageChanging changer = GameInfo.changer;
        changer.StartCoroutine(changer.ChangeToStage2_5());
        return true;
    }

    private float LastBalanceInState()
    {
        return GameInfo.SpentSecondsIngame - lastbaBalanceUpdate;
    }
}
