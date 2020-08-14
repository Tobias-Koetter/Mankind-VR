using System;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(FiniteStateMachine))]
public class GameInfo : MonoBehaviour
{
    public STATE currentState;
    public Text debugInfo;
    public float totalTime;
    private bool DebugMode;
    private bool isjumpingTime = false;

    public SpawnController TrashSpawner;
    public DestroyVegetation PlantDestroyer;
    public StageChanging changer;
    private LevelBalancing balancer;
    public FiniteStateMachine fsm;
    private int lastInt = -1;

    public float Timer{ get; private set; }
    private int numberOfStates;
    private bool gameOverFlag = false;

    public bool firstTreeDestroyed { get; private set; } = false;

    public float spentSecondsIngame => totalTime - Timer;

    public void setGameOver() { gameOverFlag = true; }

    private void Awake() 
    {
        totalTime = 0f;
        totalTime += GlobalSettingsManager.GetTotalGameTime();
        currentState = GlobalSettingsManager.firstState;

        // Takes the amount of states created in the enum STATE
        numberOfStates = Enum.GetValues(typeof(STATE)).GetUpperBound(0) + 1;

        Timer = totalTime;
        PlantDestroyer.Setup(this);

        balancer = new LevelBalancing();
        //fsm = this.GetComponent<FiniteStateMachine>();
        DebugMode = GlobalSettingsManager.debugActive;
        if(!DebugMode)
        {
            debugInfo.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        //if (Timer > 0f)
        {
            AbstractState current = fsm.currentState;

            //float spTimer = current.SecondsToSpawnTrash;
            /*
            if (current.Name.Equals(STATE.NATURE))
            {
                int curMod = Mathf.FloorToInt(Timer % spTimer);
                int curInt = Mathf.FloorToInt(Timer / spTimer);
                if (curInt != lastInt && curMod == 0)
                {
                    //print("Spawn in STATE START");
                    TrashSpawner.spawnOnTimer();
                    lastInt = curInt;
                }
            }
            if (currentState.Equals(STATE.DECAY_MAIN))
            {
                int curMod = Mathf.FloorToInt(Timer % 2f);
                int curInt = Mathf.FloorToInt(Timer / 2f);
                if (curInt != lastInt && curMod == 0)
                {
                    //print("Spawn in STATE MIDDLE");
                    TrashSpawner.spawnOnTimer();
                    lastInt = curInt;
                }
            }
            if (currentState.Equals(STATE.FINAL))
            {
                int curMod = Mathf.FloorToInt(Timer % 1f);
                int curInt = Mathf.FloorToInt(Timer / 1f);
                if (curInt != lastInt && curMod == 0)
                {
                    print("Spawn in STATE FINAL");
                    TrashSpawner.spawnOnTimer();
                    lastInt = curInt;
                }
            }*/
            /*
             
            // Use the enum value of currentState to create a specific time border for the next state change
            // OR: If amount of trees are dead [ 30 ]
            if (timer < totalTime - (timeInState * (float)currentState) 
                || (currentState.Equals(STATE.NATURE) && plantDestroyer.deadTrees.Count == 5))
            {
                currentState += 1;
            }
            */

            Timer -= Time.deltaTime;

            // Prints timer and STATE to the screen
            // allows to jump to the next state by pressing the "T" key
            if (DebugMode)
            {
                bool jumpTime = Input.GetKeyDown(KeyCode.T);
                if (jumpTime && !isjumpingTime)
                {
                    jumpTimer();
                    isjumpingTime = true;
                }
                if(!jumpTime && isjumpingTime)
                {
                    isjumpingTime = false;
                }
                UpdateLevelTimer(Timer);
            }
        }
        /*
        else if(Timer < 0f)
        {
            Timer = 0f;
            int curMod = Mathf.FloorToInt(Timer % 1f);
            int curInt = Mathf.FloorToInt(Timer / 1f);
            if (curInt != lastInt && curMod == 0)
            {
                print("Spawn after STATE FINAL");
                TrashSpawner.spawnOnTimer();
                lastInt = curInt;
            }
        }
        */



    }

    public void SwitchFirstTreeDestroyed()
    {
        firstTreeDestroyed = true;
        if(fsm.currentState is State_Alive cur)
        {
            cur.UpdateTreeDestroyStatus(firstTreeDestroyed);
        }
        
    }

    public void UpdateAbstractState(AbstractState newState)
    {
        // Do Something special for statechange
        currentState = newState.Name;
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
            $"\nNature: {LevelBalancing.GetCurrentNatureValue()}\nTrash: {LevelBalancing.GetCurrentTrashValue()}\nBalanceValue: {LevelBalancing.GetBalanceVariance()}";
        
    }

    private void jumpTimer()
    {
        Debug.Log(Timer+ "|-|"+GlobalSettingsManager.GetStateTime(currentState)+"|-|"+ fsm.currentState.RemainingTimeInState+ "==>"+ (Timer - (GlobalSettingsManager.GetStateTime(currentState) - fsm.currentState.RemainingTimeInState)));
        Debug.Log(fsm.currentState.StartTime + "|||" + fsm.currentState.SecondsToStateChange);
        Timer = Timer - (GlobalSettingsManager.GetStateTime(currentState) - fsm.currentState.RemainingTimeInState);
    }
    #endregion 
}
