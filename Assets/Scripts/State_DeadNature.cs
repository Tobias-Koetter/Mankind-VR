
public class State_DeadNature : AbstractState
{
    public State_DeadNature() : base()
    {
        nextState = null;
        name = STATE.FINAL;
    }

    public override AbstractState UpdateState(GameState info)
    {
        throw new System.NotImplementedException();
    }
}
