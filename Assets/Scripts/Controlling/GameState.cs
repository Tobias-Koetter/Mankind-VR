using System;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(FiniteStateMachine))]
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
    private FiniteStateMachine fsm;
    private int lastInt = -1;

    private float timer;
    private float timeInState;
    private int numberOfStates;

    public float spentSecondsIngame => totalTime - timer;

    private void Start() 
    {
        // Takes the amount of states created in the enum STATE
        numberOfStates = Enum.GetValues(typeof(STATE)).GetUpperBound(0) + 1;

        // Time to spend in one of the States
        timeInState = totalTime / numberOfStates; 
        timer = totalTime;
        plantDestroyer.Setup(this);

        balancer = new LevelBalancing();
        fsm = this.GetComponent<FiniteStateMachine>();
    }

    private void Update()
    {
        if (timer > 0f)
        {
            AbstractState current = fsm.currentState;
            float spTimer = current.secondsToSpawnTrash;

            if (current.name.Equals(STATE.NATURE))
            {
                int curMod = Mathf.FloorToInt(timer % spTimer);
                int curInt = Mathf.FloorToInt(timer / spTimer);
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
            /*
             
            // Use the enum value of currentState to create a specific time border for the next state change
            // OR: If amount of trees are dead [ 30 ]
            if (timer < totalTime - (timeInState * (float)currentState) 
                || (currentState.Equals(STATE.NATURE) && plantDestroyer.deadTrees.Count == 5))
            {
                currentState += 1;
            }
            */

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
