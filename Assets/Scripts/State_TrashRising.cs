using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class State_TrashRising : AbstractState
{
    private float lastbaBalanceUpdate;
    private float timeBetweenBalancing;
    private bool preStatechangeAction = true;

    public State_TrashRising(GameInfo info) : base(info)
    {
        NextState = new State_DeadNature(info);
        Name = STATE.TRASH_RISING;
        SecondsToStateChange = GlobalSettingsManager.GetStateTime(this.Name);
        this.SecondsToSpawnTrash = 0.5f;
    }

    public override bool EnterState()
    {
        base.EnterState();
        lastbaBalanceUpdate = -1f;
        timeBetweenBalancing = 0.8f;
        return true;
    }

    public override AbstractState UpdateState()
    {
        // anchor: ran out of Time in this state 
        if (RemainingTimeInState >= SecondsToStateChange)
        {
            return NextState;
        }
        else if((RemainingTimeInState >= SecondsToStateChange - 10f) && (preStatechangeAction) )
        {
            SpawnTrashOverTime();
            GameInfo.PlantDestroyer.DestroyTreesInAlive1();
            preStatechangeAction = false;
        }
        // else spawn small Trash every time spawnTiming is hit;
        else
        {
            SpawnTrashOverTime();     

            if(LastBalanceInState() >= timeBetweenBalancing || lastbaBalanceUpdate < 0f)  // Check if enough time has elapsed since last Balance spawn. Or its the first Balance spawn
            {
                Debug.Log("Balancing is triggered");
                if (LevelBalancing.GetBalanceVariance() < 0f)    // Check if there is need for Balance | Look up description in LevelBalancing Class for more info
                {
                    GameInfo.TrashSpawner.SpawnOnTimer();
                }
                else if (LevelBalancing.GetBalanceVariance() > (Trees.startingNatureValue/100f *30f))
                {
                    Debug.Log("Balancing is executing.");
                    GameInfo.PlantDestroyer.DestroyRandomTreeInRisingState();
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
        changer.StartCoroutine(changer.ChangeToStage3());
        return true;
    }

    private float LastBalanceInState()
    {
        return GameInfo.SpentSecondsIngame - lastbaBalanceUpdate;
    }
}
