
public class State_DeadNature : AbstractState
{
    private float startTime;
    public State_DeadNature(GameInfo info) : base(info)
    {
        NextState = null;
        Name = STATE.FINAL;
        SecondsToStateChange = GlobalSettingsManager.GetStateTime(this.Name);
        SecondsToSpawnTrash = 2f;
    }

    public override AbstractState UpdateState()
    {
        // anchor: ran out of Time in this state 
        if ((GameInfo.spentSecondsIngame - startTime) >= SecondsToStateChange)
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
