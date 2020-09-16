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
