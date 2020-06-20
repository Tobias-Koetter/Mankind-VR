
public class State_TrashRising : AbstractState
{
    public State_TrashRising() : base()
    {
        nextState = new State_DeadNature();
        name = STATE.TRASH_RISING;
    }

    public override AbstractState UpdateState(GameState info)
    {
        throw new System.NotImplementedException();
    }
}
