using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public MeshRenderer Renderer;
    public float CurrentStage { get; protected set; } = 0f;
    [Range(0, 2)]
    public int pointer = 0;
    private float[] stageValues = new float[] { 0f, 1f, 2f };
    private Material changingGround;
    private Material changingSand;
    private float lerp_start = -1f;
    private float lerp_end = -1f;
    private float interpolator;
    private bool inLerp;
    private float personalSpeed;

    void Awake()
    {
        changingGround = Renderer.materials[0];
        changingSand = Renderer.materials[3];
        CurrentStage = stageValues[pointer];
        changingGround.SetFloat("StageValue", CurrentStage);
        changingSand.SetFloat("StageValue", CurrentStage);
        inLerp = false;
        personalSpeed = Random.Range(0.09f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inLerp)
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                if (pointer < stageValues.Length - 1)
                {
                    pointer += 1;
                    lerp_start = CurrentStage;
                    lerp_end = stageValues[pointer];
                    interpolator = 0f;
                    inLerp = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                if (pointer > 0)
                {
                    pointer -= 1;
                    lerp_start = CurrentStage;
                    lerp_end = stageValues[pointer];
                    interpolator = 0f;
                    inLerp = true;
                }
            }
        }
        else if (inLerp)
        {

            CurrentStage = Mathf.Lerp(lerp_start, lerp_end, interpolator);
            interpolator += personalSpeed * Time.deltaTime;

            if (interpolator > 1.0f)
            {
                CurrentStage = lerp_end;
                lerp_end = -1f;
                lerp_start = -1f;
                inLerp = false;
            }
            changingGround.SetFloat("StageValue", CurrentStage);
            changingSand.SetFloat("StageValue", CurrentStage);
        }
    }
}
