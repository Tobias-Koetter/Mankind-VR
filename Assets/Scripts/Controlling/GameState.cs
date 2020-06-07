﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum STATE { NATURE = 1, DECAY_START = 2 , DECAY_MAIN = 3, TRASH_RISING = 4, FINAL = 5 };



public class GameState : MonoBehaviour
{
    public STATE currentState = STATE.NATURE;
    public Text debugInfo;
    public float totalTime = 360f; // 6 minutes
    public bool DebugMode;

    public SpawnController trashSpawner;
    public DestroyVegetation plantDestroyer;
    public StageChanging changer;
    private LevelBalancing balancer;
    private int lastInt = -1;

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
        plantDestroyer.Setup(this);

        balancer = new LevelBalancing();
    }

    private void Update()
    {
        if (timer > 0f)
        {
            if (currentState != changer.curState) { ActivateChanger(); }

            if(currentState.Equals(STATE.NATURE))
            {
                int curMod = Mathf.FloorToInt(timer % 5f);
                int curInt = Mathf.FloorToInt(timer / 5f);
                if (curInt != lastInt && curMod == 0)
                {
                    //print("Spawn in STATE START");
                    trashSpawner.spawnOnTimer();
                    lastInt = curInt;
                }
            }
            if (currentState.Equals(STATE.DECAY_MAIN))
            {
                int curMod = Mathf.FloorToInt(timer % 2f);
                int curInt = Mathf.FloorToInt(timer / 2f);
                if (curInt != lastInt && curMod == 0)
                {
                    //print("Spawn in STATE MIDDLE");
                    trashSpawner.spawnOnTimer();
                    lastInt = curInt;
                }
            }
            if (currentState.Equals(STATE.FINAL))
            {
                int curMod = Mathf.FloorToInt(timer % 1f);
                int curInt = Mathf.FloorToInt(timer / 1f);
                if (curInt != lastInt && curMod == 0)
                {
                    print("Spawn in STATE FINAL");
                    trashSpawner.spawnOnTimer();
                    lastInt = curInt;
                }
            }
            // Use the enum value of currentState to create a specific time border for the next state change
            // OR: If amount of trees are dead [ 30 ]
            if (timer < totalTime - (timeInState * (float)currentState) 
                || (currentState.Equals(STATE.NATURE) && plantDestroyer.deadTrees.Count == 5))
            {
                currentState += 1;
            }

            timer -= Time.deltaTime;

            // Prints timer and STATE to the screen
            // allows to jump to the next state by pressing the "T" key
            if (DebugMode)
            {
                bool jumpTime = Input.GetKeyDown(KeyCode.T);
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
            int curMod = Mathf.FloorToInt(timer % 1f);
            int curInt = Mathf.FloorToInt(timer / 1f);
            if (curInt != lastInt && curMod == 0)
            {
                print("Spawn after STATE FINAL");
                trashSpawner.spawnOnTimer();
                lastInt = curInt;
            }
        }



    }


    private void ActivateChanger()
    {
        Debug.Log("We activate the Changer."+ currentState+"|"+changer.curState);
        if (currentState != changer.curState)
        {
            switch (currentState)
            {
                case STATE.DECAY_START:
                    StartCoroutine(changer.ChangeToStage1_5());
                    break;
                case STATE.DECAY_MAIN:
                    StartCoroutine(changer.ChangeToStage2());
                    break;
                case STATE.TRASH_RISING:
                    StartCoroutine(changer.ChangeToStage2_5());
                    break;
                case STATE.FINAL:
                    StartCoroutine(changer.ChangeToStage3());
                    break;
            }
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
        debugInfo.text = $"Time: {minutes.ToString("00")}:{seconds.ToString("00")} \nSTATE: {currentState.ToString()} " +
            $"\nNature: {LevelBalancing.GetCurrentNatureValue()}\nTrash: {LevelBalancing.GetCurrentTrashValue()}";
        
    }

    private void jumpTimer()
    {
        timer = totalTime - (timeInState * (float)currentState);
    }
    #endregion 
}