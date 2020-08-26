using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuText : MonoBehaviour
{
    public BUTTON_TYPE type;

    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] MenuWindowInfo ownWindow;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;

    private ButtonChilds bC;

    public void Awake()
    {
        bC = new ButtonChilds
        {
            Image = GetComponentInChildren<Image>(),
            Text = GetComponentInChildren<TextMeshProUGUI>()
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (ownWindow.isActive)
        {
            animator.SetBool("isActive", true);
            hide(false);
        }
        else
        {
            animator.SetBool("isActive", false);
            hide(true);
        }
    }

    private void hide(bool value)
    {
        bC.Image.enabled = !value;
        bC.Text.enabled = !value;
    }
}
