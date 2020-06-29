
public class State_TrashRising : AbstractState
{
    public State_TrashRising(GameInfo info) : base(info)
    {
        NextState = new State_DeadNature(info);
        Name = STATE.TRASH_RISING;
    }

    public override AbstractState UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
