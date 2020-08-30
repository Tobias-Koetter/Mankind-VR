using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WindowCamRelation { ZOOM_IN, ZOOM_OUT}

public class MenuWindowInfo : MonoBehaviour
{
    public bool isActive;
    public int buttonCount;
    public WindowCamRelation camRelation;
    public MenuButtonController menuButtonController;
    public MenuButton[] ownButtons;
    public Image[] ownNotClickable;

    private void Awake()
    {
        if (ownButtons.Length != 0)
        {
            buttonCount = ownButtons.Length;
        }
        else
        {
            throw new ArgumentException("Needs to be set in the editor.", ownButtons.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ownNotClickable.Length > 0)
        {
            if(!isActive && ownNotClickable[0].enabled == true)
            {
                foreach(Image i in ownNotClickable)
                {
                    i.enabled = false;
                }
            }
            else if(isActive && ownNotClickable[0].enabled == false)
            {
                foreach (Image i in ownNotClickable)
                {
                    i.enabled = true;
                }
            }
        }
            

    }
}
