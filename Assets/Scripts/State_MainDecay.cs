
public class State_MainDecay : AbstractState
{
    public State_MainDecay() : base()
    {
        nextState = new State_TrashRising();
        name = STATE.DECAY_MAIN;
    }

    public override AbstractState UpdateState(GameState info)
    {
        throw new System.NotImplementedException();
    }
}
