
public class State_Alive : AbstractState
{
    float SecondsToStateChange = 120f;

    public State_Alive() : base()
    {
        nextState = new State_StartDecay();
        name = STATE.NATURE;
        secondsToSpawnTrash = 5f;
    }

    public override AbstractState UpdateState(GameState info)
    {
        if (info.spentSecondsIngame >= SecondsToStateChange)
        {
            return nextState;
        }

        return this;

    }
}
