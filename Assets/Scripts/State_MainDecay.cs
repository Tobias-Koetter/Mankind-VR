
public class State_MainDecay : AbstractState
{
    public State_MainDecay(GameInfo info) : base(info)
    {
        NextState = new State_TrashRising(info);
        Name = STATE.DECAY_MAIN;
    }

    public override AbstractState UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
