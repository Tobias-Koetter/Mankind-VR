
public class State_DeadNature : AbstractState
{
    public State_DeadNature(GameInfo info) : base(info)
    {
        NextState = null;
        Name = STATE.FINAL;
    }

    public override AbstractState UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
