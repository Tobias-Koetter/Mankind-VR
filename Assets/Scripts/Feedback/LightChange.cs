using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChange : MonoBehaviour
{
    public GameInfo state;
    public Transform Sunlight;
    public bool turnOn;

    private Vector3 daytime;
    private Vector3 middletime;
    private Vector3 latetime;

    private int current;
    private int last;
    private float t = 0;
    private bool inLerp = false;
    // Start is called before the first frame update
    void Start()
    {
        daytime = new Vector3(60f, 155f, 90f);
        middletime = new Vector3(34f, 180f, 90f);
        latetime = new Vector3(8.5f, 180f, 90f);
        current = 1;
        last = current;
        Sunlight.rotation = Quaternion.Euler(daytime);
    }

    // Update is called once per frame
    void Update()
    {
        if (turnOn)
        {
            if (state.currentState == STATE.DECAY_MAIN && !inLerp)
            {
                if (current == 1)
                {
                    current = 2;
                    inLerp = true;
                }
            }
            else if (state.currentState == STATE.FINAL && !inLerp)
            {
                if (current == 2)
                {
                    current = 3;
                    inLerp = true;
                }
            }
            if (current != last)
            {
                t += 0.05f * Time.deltaTime;
                if (t > 1.0f)
                {
                    t = 1.0f;
                }
                Vector3 cur = Vector3.zero;
                if (current == 2)
                {
                    cur = Vector3.Lerp(daytime, middletime, t);
                }
                if (current == 3)
                {
                    cur = Vector3.Lerp(middletime, latetime, t);
                }
                Sunlight.rotation = Quaternion.Euler(cur);
                if (t == 1.0f)
                {
                    t = 0f;
                    last = current;
                    inLerp = false;
                }
            }
        }
    }
}
