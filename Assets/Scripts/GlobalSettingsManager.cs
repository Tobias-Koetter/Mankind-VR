using System;
using UnityEngine;

public class GlobalSettingsManager: MonoBehaviour
{
    public bool debug;
    public bool clickAction;
    public STATE startState = STATE.NATURE;
    [NamedArray(new String[] { "Nature", "StartDecay", "MainDecay", "TrashRising", "Final"})]
    public  float[] SecondsInState = new float[Enum.GetValues(typeof(STATE)).GetUpperBound(0)];

    public static bool debugActive = false;
    public static bool clickActionActive = true;
    public static STATE firstState;
    public static float[] StateSeconds;

    void Awake()
    {
        firstState = startState;
        debugActive = debug;
        clickActionActive = clickAction;
        StateSeconds = SecondsInState;
    }


    public static float GetStateTime(STATE stateName)
    {
        int arrayPos = (int)stateName -1;
        return StateSeconds[arrayPos];
    }

    public static float GetTotalGameTime()
    {
        float total = 0f;
        for (int i= (int)firstState-1;i < Enum.GetValues(typeof(STATE)).GetUpperBound(0); i++)
        {
            float f = StateSeconds[i];
            total += f;
        }
        Debug.Log(total);
        return total;
    }

}
