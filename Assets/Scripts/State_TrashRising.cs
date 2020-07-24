
public class State_TrashRising : AbstractState
{
    private float startTime;

    public State_TrashRising(GameInfo info) : base(info)
    {
        NextState = new State_DeadNature(info);
        Name = STATE.TRASH_RISING;
        SecondsToStateChange = GlobalSettingsManager.GetStateTime(this.Name);
        this.SecondsToSpawnTrash = 2f;
    }

    public override bool EnterState()
    {
        base.EnterState();
        startTime = GameInfo.spentSecondsIngame;
        return true;
    }

    public override AbstractState UpdateState()
    {
        // anchor: ran out of Time in this state 
        if ((GameInfo.spentSecondsIngame - startTime) >= SecondsToStateChange)
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
}
