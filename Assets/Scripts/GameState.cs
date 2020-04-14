using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum STATE { START = 1, MIDDLE = 2, FINAL = 3 };



public class GameState : MonoBehaviour
{
    STATE currentState = STATE.START;
    public Text debugInfo;
    public float totalTime = 360f; // 6 minutes
    public bool DebugMode;

    private float timer;
    private float timeInState;
    private int numberOfStates;

    private void Start() 
    {
        // Takes the amount of states created in the enum STATE
        numberOfStates = Enum.GetValues(typeof(STATE)).GetUpperBound(0) + 1;

        // Time to spend in one of the States
        timeInState = totalTime / numberOfStates; 
        timer = totalTime;
    }

    private void Update()
    {
        if (timer > 0f)
        {
            // Use the enum value of currentState to create a specific time border for the next state change
            if (timer < totalTime - (timeInState * (float)currentState))
            {
                currentState += 1;
            }

            // Prints timer and STATE to the screen
            // allows to jump to the next state by pressing the "T" key
            if (DebugMode)
            {
                bool jumpTime = Input.GetKeyDown(KeyCode.T);
                timer -= Time.deltaTime;
                if (jumpTime)
                {
                    jumpTimer();
                }
                UpdateLevelTimer(timer);
            }
        }
        else if(timer < 0f)
        {
            timer = 0f;
        }



    }


    #region Debug Functions: Only active if bool debugMode is set TRUE in Editor
    private void UpdateLevelTimer(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60f);
        int seconds = Mathf.FloorToInt(totalSeconds % 60f);

        if (seconds == 60)
        {
            seconds = 0;
            minutes += 1;
        }
        debugInfo.text = $"Time: {minutes.ToString("00")}:{seconds.ToString("00")} \nSTATE: {currentState.ToString()}";
        
    }

    private void jumpTimer()
    {
        timer = totalTime - (timeInState * (float)currentState);
    }
    #endregion 
}
