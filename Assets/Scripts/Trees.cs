using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : Interactable
{
    public Transform alive;
    public Transform destroyed;

    public bool isDestroyed = false;

    private void Update()
    {
        if(isDestroyed)
        {
            alive.gameObject.SetActive(false);
            destroyed.gameObject.SetActive(true);
        }
    }

    public override void Interact()
    {
        isDestroyed = true;
    }
}
