using UnityEngine;

public class GlobalSettingsManager: MonoBehaviour
{
    public bool debug;
    public bool clickAction;
    public static bool debugActive = false;
    public static bool clickActionActive = true;

    void Start()
    {
        debugActive = debug;
        clickActionActive = clickAction;
    }

}
