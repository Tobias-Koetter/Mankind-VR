using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPauseListener
{
    void UpdateListener(bool newListenerValue);
    void AddToController();
    
}
