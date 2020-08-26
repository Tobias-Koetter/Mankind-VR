using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MenuCamMovement : MonoBehaviour
{
    Animator aCon;

    public void Start()
    {
        aCon = GetComponent<Animator>();
    }

    public void ZoomIn(bool value)
    {
        aCon.SetBool("ZoomIn", value);
        aCon.SetBool("ZoomOut", !value);
    }

}
