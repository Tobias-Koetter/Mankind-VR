
public class State_DeadNature : AbstractState
{
    float checkForGlobalDestroy1;
    float checkForGlobalDestroy2;
    bool destroy1 = true;
    bool destroy2 = true;

    public State_DeadNature(GameInfo info) : base(info)
    {
        NextState = null;
        Name = STATE.FINAL;
        SecondsToStateChange = GlobalSettingsManager.GetStateTime(this.Name);
        SecondsToSpawnTrash = 0.5f;
    }

    public override bool EnterState()
    {
        base.EnterState();
        GameInfo.animatorGlobalVolume.SetBool("ToTwo", true);
        GameInfo.PlantDestroyer.DestroyTreesInAlive1();
        GameInfo.PlantDestroyer.DestroyTreesInAlive2();
        checkForGlobalDestroy1 = (SecondsToStateChange / 100f * 10f);
        checkForGlobalDestroy2 = (SecondsToStateChange / 100f * 50f);
        return true;
    }

    public override AbstractState UpdateState()
    {
        // anchor: ran out of Time in this state 
        if (RemainingTimeInState >= SecondsToStateChange)
        {
            //this.GameInfo.SetGameOver();
        }
        else if(RemainingTimeInState >= checkForGlobalDestroy1 && destroy1)
        {
            GameInfo.PlantDestroyer.DestroyTreesInBetween1();
            destroy1 = false;
        }
        else if (RemainingTimeInState >= checkForGlobalDestroy2 && destroy2)
        {
            GameInfo.PlantDestroyer.DestroyTreesInBetween2();
            destroy2 = false;
        }
        // else spawn small Trash every time spawnTiming is hit;
        else
        {
            EndgameOverTime();
        }

        return this;
    }
}
