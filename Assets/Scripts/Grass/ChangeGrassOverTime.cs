using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGrassOverTime : MonoBehaviour
{

    public struct ChangeVals
    {
        public float height;
        public float width;
        public Color color;

        public ChangeVals(float h, float w, Color c) {
            height = h;
            width = w;
            color = c;
        }
    }
    public GrassComputeScript grass;
    public List<SO_GrassSettings> settings_stage1;
    public List<SO_GrassSettings> settings_stage2;
    [Button("ChangeSettings")]
    public string s;
    [Range(0f,5f)]
    public float height;
    private float lastH;

    private List<SO_GrassSettings>[] stages;
    public int external_ptr = 0;
    public int internal_ptr = 0;
    private bool inChange = false;
    private float timer = 0f;
    private float totalTime = 10f;
    private ChangeVals endVals;
    private ChangeVals startVals;
    private ChangeVals currentVals;
    private SO_GrassSettings currentSettings;

    private void Start() {
        external_ptr = 0;
        internal_ptr = 0;
        stages = new List<SO_GrassSettings>[2];
        stages[0] = settings_stage1;
        stages[1] = settings_stage2;
        currentSettings = settings_stage1[external_ptr];
        ResetGrassToPointer();
        height = currentSettings.MaxHeight;
        lastH = height;
    }
    private void Update() {
        if(lastH != height)
        {
            currentSettings.MaxHeight = height;
            grass.Reset();
            lastH = height;
        }
        if(inChange)
        {
            if (timer < totalTime)
            {
                timer += Time.deltaTime;
                timer = Mathf.Min(timer, totalTime);
                currentVals.height = Mathf.Lerp(startVals.height, endVals.height, timer / totalTime);
                currentVals.width = Mathf.Lerp(startVals.width, endVals.width, timer / totalTime);
                currentVals.color = Color.Lerp(startVals.color, endVals.color, timer / totalTime);
                //grass.FillshaderValues(currentVals.height, currentVals.width, currentVals.color);
                currentSettings.MaxHeight = currentVals.height;
                currentSettings.MaxWidth = currentVals.width;
                currentSettings.bottomTint = currentVals.color;
                settings_stage1[external_ptr] = currentSettings;
                ResetGrassToPointer();
            }
            else
            {
                /*internal_ptr = ++internal_ptr % stages[external_ptr].Count;
                if (internal_ptr == 0)
                {
                    external_ptr = ++external_ptr % 2;
                    inChange = false;
                }*/
                ResetGrassToPointer();
                timer = 0f;
                inChange = false;
            }

        }
    }

    public void ChangeSettings() {
        if (!inChange)
        {
            internal_ptr = external_ptr;
            external_ptr = ++external_ptr % settings_stage1.Count;

            startVals = new ChangeVals(settings_stage1[internal_ptr].MaxHeight, settings_stage1[internal_ptr].MaxWidth, settings_stage1[internal_ptr].bottomTint);
            endVals = new ChangeVals(settings_stage1[external_ptr].MaxHeight, settings_stage1[external_ptr].MaxWidth, settings_stage1[external_ptr].bottomTint);
            currentSettings = settings_stage1[external_ptr];

            currentVals = new ChangeVals(startVals.height, startVals.width, startVals.color);
            //grass.currentPresets = currentSettings;
            inChange = true;
            /*internal_ptr = ++internal_ptr % stages[external_ptr].Count;
            inChange = true;
            if (internal_ptr == 0)
            {
                internal_ptr = stages[external_ptr].Count-1;

            }*/
            
        }
    }
    private void ResetGrassToPointer() {
        //grass.currentPresets = stages[external_ptr][internal_ptr];
        grass.currentPresets = currentSettings;
        grass.ResetFaster();
    }
}
