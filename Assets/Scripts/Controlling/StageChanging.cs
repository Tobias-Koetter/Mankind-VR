﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChanging : MonoBehaviour
{
    [Header("General Values")]
    public STATE curState = STATE.NATURE;

    [Range(1f,10f)]
    public float animSpeed = 1f;
    [Header("Water Plane Values")]
    public Transform Water;
    public float water_HighMark;
    public float water_LowMark;

    private Vector3 waterVector_50Percent;
    private Vector3 waterVector_66Percent;


    
    private float lerp_Value = 0f;
    private Vector3 lerp_Start;
    private Vector3 lerp_End;

    void Start()
    {
        float waterMark_66Percent;
        float waterMark_50Percent;
        float absoluteWaterRange = Mathf.Abs(water_HighMark) + Mathf.Abs(water_LowMark);
        waterMark_50Percent = water_HighMark - (absoluteWaterRange / 2);
        waterMark_66Percent = water_HighMark - (absoluteWaterRange / 3);

        Vector3 pos = Water.position;
        waterVector_50Percent = new Vector3(pos.x, waterMark_50Percent, pos.z);
        waterVector_66Percent = new Vector3(pos.x, waterMark_66Percent, pos.z);
        pos.Set(pos.x,water_HighMark,pos.z);
        Water.position = pos;
    }

    public IEnumerator ChangeToStage1_5()
    {
        curState = STATE.DECAY_START;
        yield return null;
    }
    public IEnumerator ChangeToStage2()
    {
        curState = STATE.DECAY_MAIN;
        SetupLerp(Water.position, waterVector_66Percent);

        while(lerp_Value < 1f)
        {
            lerp_Value += (animSpeed * 0.01f) * Time.deltaTime;
            lerp_Value = ((lerp_Value > 1f)? 1f : lerp_Value);
            Water.position = Vector3.Lerp(lerp_Start, lerp_End, lerp_Value);
            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
    }
    public IEnumerator ChangeToStage2_5()
    {
        curState = STATE.TRASH_RISING;
        SetupLerp(Water.position, waterVector_50Percent);

        while (lerp_Value < 1f)
        {
            lerp_Value += (animSpeed * 0.01f) * Time.deltaTime;
            lerp_Value = ((lerp_Value > 1f) ? 1f : lerp_Value);
            Water.position = Vector3.Lerp(lerp_Start, lerp_End, lerp_Value);
            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
    }
    public IEnumerator ChangeToStage3()
    {
        curState = STATE.FINAL;
        SetupLerp(Water.position, new Vector3(Water.position.x, water_LowMark, Water.position.z));

        while (lerp_Value < 1f)
        {
            lerp_Value += (animSpeed*0.01f) * Time.deltaTime;
            lerp_Value = ((lerp_Value > 1f) ? 1f : lerp_Value);
            Water.position = Vector3.Lerp(lerp_Start, lerp_End, lerp_Value);
            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
    }

    private void SetupLerp(Vector3 start, Vector3 end)
    {
        lerp_Start = start;
        lerp_End = end;
        lerp_Value = 0f;
    }

}