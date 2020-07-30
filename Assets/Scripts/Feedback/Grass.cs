using UnityEngine;
using UnityEngine.Analytics;

public class Grass : MonoBehaviour
{
    public MeshRenderer Renderer;
    public float CurrentStage { get; protected set; } = 0f;
    [Range(0,2)]
    public int pointer = 0;
    private float[] stageValues = new float[] {0f,0.5f,1f,1.5f,2f};
    private Material changingGrass;
    private float lerp_start = -1f;
    private float lerp_end = -1f;
    private bool inLerp;
    private float personalSpeed;

    void Awake()
    {
        changingGrass = Renderer.materials[0];
        CurrentStage = stageValues[pointer];
        changingGrass.SetFloat("StageValue", CurrentStage);
        inLerp = false;
        personalSpeed = Random.Range(0.09f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!inLerp)
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                if(pointer < stageValues.Length-1)
                {
                    pointer += 1;
                    lerp_start = CurrentStage;
                    lerp_end = stageValues[pointer];
                    inLerp = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                if(pointer > 0)
                {
                    pointer-= 1;
                    lerp_start = CurrentStage;
                    lerp_end = stageValues[pointer];
                    inLerp = true;
                }
            }
        }
        else */if(inLerp)
        {
            if (!Renderer.isVisible)
            {
                CurrentStage = lerp_end;
                lerp_end = -1f;
                lerp_start = -1f;
                inLerp = false;
            }
            else
            {
                CurrentStage += personalSpeed * Time.deltaTime;

                if (CurrentStage >= lerp_end)
                {
                    CurrentStage = lerp_end;
                    lerp_end = -1f;
                    lerp_start = -1f;
                    inLerp = false;
                }
            }
            changingGrass.SetFloat("StageValue", CurrentStage);
        }
    }

    public void NextGrassStage()
    {
        if (pointer < stageValues.Length - 1)
        {
            pointer += 1;
            lerp_start = CurrentStage;
            lerp_end = stageValues[pointer];
            inLerp = true;
        }
    }



}
