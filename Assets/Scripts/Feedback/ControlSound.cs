using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ControlSound : MonoBehaviour
{
    private const string VOL_NAT = "_NatureVolume";
    private const string VOL_WIND = "_WindVolume";
    private const string VOL_TREES = "_FallingVolume";
    public AudioMixer master;
    public AudioSource music;

    float startNature;
    float startMusic;

    bool musicOn = true;
    bool natureOn = true;
    bool windOn = false;
    bool treesOn = false;

    bool changeWind = false;
    bool changeMusic = false;
    bool changeNature = false;
    bool changeTrees = false;

    float t_wind = 0f;
    float t_nature = 0f;
    float t_music = 0f;
    float t_Trees = 0f;

    float total = 3f;

    // Start is called before the first frame update
    void Start()
    {
        master.GetFloat(VOL_NAT, out startNature);
        startMusic = music.volume;
        master.SetFloat(VOL_WIND,-80f);
    }

    // Update is called once per frame
    void Update()
    {
        if (changeMusic)
        {
            t_music += Time.deltaTime;
            t_music = Mathf.Min(t_music, total);
            music.volume = Mathf.Lerp(startMusic, 0f, t_music / total);
            if(t_music == total)
            {
                changeMusic = false;
                musicOn = false;
            }
        }
        if(changeNature)
        {
            t_nature += Time.deltaTime;
            t_nature = Mathf.Min(t_nature, total);
            master.SetFloat(VOL_NAT, Mathf.Lerp(startNature, -80f, t_nature / total));
            if(t_nature == total)
            {
                changeNature = false;
                natureOn = false;
            }
        }
        if(changeWind)
        {
            t_wind += Time.deltaTime;
            t_wind = Mathf.Min(t_wind, total);
            master.SetFloat(VOL_WIND, Mathf.Lerp(-80f, 0f, t_wind / total));
            if (t_wind == total)
            {
                changeWind = false;
                windOn = true;
            }
        }
        if (changeTrees)
        {
            t_Trees += Time.deltaTime;
            t_Trees = Mathf.Min(t_Trees, total);
            if (!treesOn)
            {
                master.SetFloat(VOL_TREES, Mathf.Lerp(-80f, 0f, t_Trees / total));
                if (t_Trees == total)
                {
                    t_Trees = 0f;
                    changeTrees = false;
                    treesOn = true;
                }
            }
            else
            {
                master.SetFloat(VOL_TREES, Mathf.Lerp(0f, -80f, t_Trees / total));
                if (t_Trees == total)
                {
                    t_Trees = 0f;
                    changeTrees = false;
                    treesOn = false;
                }
            }
        }
    }

    public void DecreaseMusic() {
        if (musicOn && !changeMusic) changeMusic = true;
    }
    public void DecreaseNature() {
        if (natureOn && !changeNature) changeNature = true;
    }
    public void IncreaseWind() {
        if (!windOn && !changeWind) changeWind = true;
    }

    public void IncreaseTrees(bool notReverse) {
        if(notReverse)
        {
            if (!treesOn && !changeTrees) changeTrees = true;
        }
        else
        {
            if (treesOn && !changeTrees) changeTrees = true;
        }
    }
}
