
public class State_DeadNature : AbstractState
{
    public State_DeadNature(GameInfo info) : base(info)
    {
        NextState = null;
        Name = STATE.FINAL;
        SecondsToStateChange = GlobalSettingsManager.GetStateTime(this.Name);
        SecondsToSpawnTrash = 2f;
    }

    public override bool EnterState()
    {
        base.EnterState();
        return true;
    }

    public override AbstractState UpdateState()
    {
        // anchor: ran out of Time in this state 
        if (RemainingTimeInState >= SecondsToStateChange)
        {
            this.GameInfo.setGameOver();
        }
        // else spawn small Trash every time spawnTiming is hit;
        else
        {
            SpawnTrashOverTime();
        }

        return this;
    }
}
