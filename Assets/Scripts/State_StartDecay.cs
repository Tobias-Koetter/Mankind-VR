
public class State_StartDecay : AbstractState
{
    public State_StartDecay() : base()
    {
        nextState = new State_MainDecay();
        name = STATE.DECAY_START;
    }

    public override AbstractState UpdateState(GameState info)
    {
        throw new System.NotImplementedException();
    }
}
