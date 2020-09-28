using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(FiniteStateMachine))]
public class GameInfo : MonoBehaviour
{
    public STATE currentState;
    public Text debugInfo;
    public static bool menuOpen;
    public float totalTime;
    private bool DebugMode;
    private bool isjumpingTime = false;

    public SpawnController TrashSpawner;
    public DestroyVegetation PlantDestroyer;
    public StageChanging changer;
    //private LevelBalancing balancer;
    public FiniteStateMachine fsm;
    public Canvas canvas;
    public Animator animatorGlobalVolume;
    private MenuButtonController mBC;
    private static List<IPauseListener> listener;
    public static void AddPauseListener(IPauseListener iPL) { listener.Add(iPL); iPL.UpdateListener(menuOpen); }

    public float Timer{ get; private set; }

    public bool FirstTreeDestroyed { get; private set; } = false;

    public float SpentSecondsIngame => totalTime - Timer;

    public void SetGameOver() 
    { 
        mBC.StartLoading(0);

    }

    private void Awake() 
    {
        listener = new List<IPauseListener>();
        totalTime = 0f;
        totalTime += GlobalSettingsManager.GetTotalGameTime();
        currentState = GlobalSettingsManager.firstState;

        Timer = totalTime;
        PlantDestroyer.Setup(this);

        new LevelBalancing();
        //fsm = this.GetComponent<FiniteStateMachine>();
        DebugMode = GlobalSettingsManager.debugActive;
        if(!DebugMode)
        {
            debugInfo.gameObject.SetActive(false);
        }

        mBC = canvas.GetComponent<MenuButtonController>();
        menuOpen = mBC.inMenu;

    }

    private void Update()
    {
        if (!TrashSpawner)
        {
            TrashSpawner = FindObjectOfType<SpawnController>();
        }
        if (menuOpen != mBC.inMenu)
        {
            menuOpen = mBC.inMenu;
            foreach(IPauseListener iPL in listener)
            {
                iPL.UpdateListener(menuOpen);
            }
        }
        //if (Timer > 0f)
        {

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
    }

    public void SwitchFirstTreeDestroyed()
    {
        FirstTreeDestroyed = true;
        if(fsm.CurrentState is State_Alive cur)
        {
            cur.UpdateTreeDestroyStatus(FirstTreeDestroyed);
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
        Debug.Log(Timer+ "|-|"+GlobalSettingsManager.GetStateTime(currentState)+"|-|"+ fsm.CurrentState.RemainingTimeInState+ "==>"+ (Timer - (GlobalSettingsManager.GetStateTime(currentState) - fsm.CurrentState.RemainingTimeInState)));
        Debug.Log(fsm.CurrentState.StartTime + "|||" + fsm.CurrentState.SecondsToStateChange);
        Timer = Timer - (GlobalSettingsManager.GetStateTime(currentState) - fsm.CurrentState.RemainingTimeInState);
    }
    #endregion 
}
