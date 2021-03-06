﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE { NONE = 0, NATURE = 1, DECAY_START = 2, DECAY_MAIN = 3, TRASH_RISING = 4, FINAL = 5 };

[RequireComponent(typeof(GameInfo))]
public class FiniteStateMachine : MonoBehaviour
{
    public AbstractState CurrentState { get; protected set; }
    AbstractState next;
    AbstractState last;

    GameInfo gameInfo;

    void Awake()
    {
        gameInfo = this.GetComponent<GameInfo>();
        gameInfo.fsm = this;
        CurrentState = CreateStartState();
        next = null;
        last = null;

        
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentState == null)
        {
            CurrentState = last;
        }
        else
        {
            if (CurrentState != null && CurrentState.ExecutionState == ExecutionState.NONE)
            {
                CurrentState.EnterState();
            }
            else
            {
                next = CurrentState.UpdateState();
                if(next != CurrentState)
                {
                    last = CurrentState;
                    last.ExitState();
                    CurrentState = next;
                    CurrentState.EnterState();

                    Debug.Log("Switching state from " + last.Name + " to " + CurrentState.Name);
                    gameInfo.UpdateAbstractState(CurrentState);
                    
                }

            }
        }
    }




    AbstractState CreateStartState()
    {
        AbstractState ret = new State_Alive(gameInfo);
        switch (GlobalSettingsManager.firstState)
        {
            case STATE.NONE:
                Debug.LogError(new ArgumentException("There was no start state set in the GlobalSettingsManager. Check Controller -> GameState Object in Scene", "startState"));
                ret = null;
                break;
            case STATE.NATURE:
                break;
            case STATE.DECAY_START:
                ret = new State_StartDecay(gameInfo);
                break;
            case STATE.DECAY_MAIN:
                ret = new State_MainDecay(gameInfo);
                break;
            case STATE.TRASH_RISING:
                ret = new State_TrashRising(gameInfo);
                break;
            case STATE.FINAL:
                ret = new State_DeadNature(gameInfo);
                break;
        }
        return ret;
    }
}
