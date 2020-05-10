using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSize { small, big};

public class ChoosePlayer : MonoBehaviour
{
    public PlayerSize selection = PlayerSize.big;
    public GameObject small;
    public GameObject big;

    //Start is called before the first frame update
    void Start()
    {
        if(selection == PlayerSize.big)
        {
            small.SetActive(false);
        }
        else
        {
            big.SetActive(false);
        }
    }

}
