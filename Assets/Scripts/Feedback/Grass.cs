using UnityEngine;
using UnityEngine.Analytics;

public class Grass : MonoBehaviour
{
    public MeshRenderer Renderer;
    public float CurrentStage { get; protected set; } = 0f;
    [Range(0,2)]
    public int pointer = 0;
    private float[] stageValues = new float[] {0f,1f,2f};
    private Material changingGrass;
    private float lerp_start = -1f;
    private float lerp_end = -1f;
    private float interpolator;
    private bool inLerp;

    void Awake()
    {
        changingGrass = Renderer.materials[0];
        CurrentStage = stageValues[pointer];
        changingGrass.SetFloat("StageValue", CurrentStage);
        inLerp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inLerp)
        {
            inLerp = true;

            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                if(pointer != stageValues.Length-1)
                {
                    pointer += 1;
                    lerp_start = CurrentStage;
                    lerp_end = stageValues[pointer];
                    interpolator = 0f;
                }
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                if(pointer != 0)
                {
                    pointer-= 1;
                    lerp_start = CurrentStage;
                    lerp_end = stageValues[pointer];
                    interpolator = 0f;
                }
            }
            else
            {
                inLerp = false;
            }
        }
        else if(inLerp)
        {
            
            CurrentStage = Mathf.Lerp(lerp_start, lerp_end, interpolator);
            interpolator += 0.5f * Time.deltaTime;

            if (interpolator > 1.0f)
            {
                CurrentStage = lerp_end;
                lerp_end = -1f;
                lerp_start = -1f;
                inLerp = false;
            }
            changingGrass.SetFloat("StageValue", CurrentStage);
        }
    }





}
