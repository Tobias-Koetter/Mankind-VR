using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE { NONE = 0, NATURE = 1, DECAY_START = 2, DECAY_MAIN = 3, TRASH_RISING = 4, FINAL = 5 };

[RequireComponent(typeof(GameInfo))]
public class FiniteStateMachine : MonoBehaviour
{
    public AbstractState currentState { get; protected set; }
    AbstractState next;
    AbstractState last;

    GameInfo gameInfo;

    void Awake()
    {
        currentState = new State_Alive(gameInfo);
        next = null;
        last = null;

        gameInfo = this.GetComponent<GameInfo>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == null)
        {
            currentState = last;
        }
        else
        {
            if (currentState != null && currentState.ExecutionState == ExecutionState.NONE)
            {
                currentState.EnterState();
            }
            else
            {
                next = currentState.UpdateState();
                if(next != currentState)
                {
                    last = currentState;
                    last.ExitState();
                    currentState = next;
                    currentState.EnterState();
                    
                }

            }
        }
    }
}
