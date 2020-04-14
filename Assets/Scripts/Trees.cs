using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : Interactable
{
    public Transform alive;
    public Transform destroyed;

    public bool isDestroyed = false;


    private DestroyVegetation controller;
    private int treeNumber;

    public DestroyVegetation Controller { get => controller; set => controller = value; }
    public int TreeNumber { get => treeNumber; set => treeNumber = value; }



    public override void Interact()
    {
        isDestroyed = true;
    }

    public override string ToString()
    {

        return "| Tree [" + treeNumber + "] " + (isDestroyed ? "isDead" : "isAlive");
    }

    private void Update()
    {
        if (isDestroyed)
        {
            alive.gameObject.SetActive(false);
            destroyed.gameObject.SetActive(true);
        }
    }
}
