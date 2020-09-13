using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BUTTON_TYPE { START, END, IN_MENU, RESUME_GAME}

public struct ButtonChilds
{
    public bool IsActive { get; set; }
    public TextMeshProUGUI Text { get; set; }
    public Image Image { get; set; }
}

public class MenuButton: MonoBehaviour
{
    public BUTTON_TYPE type;

    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] MenuWindowInfo ownWindow;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] int thisIndex;
    [SerializeField] MenuWindowInfo nextMenuWindow;
    [SerializeField] int nextSceneIndex;

    bool mouseClicked = false;

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
            if (!bC.IsActive)
            {
                if (thisIndex == 0)
                {
                    animatorFunctions.disableOnce = true;
                }
                animator.SetBool("isActive", true);
                Hide(false);
                if(thisIndex == 0)
                {
                    animatorFunctions.disableOnce = true;
                }
            }

            if (menuButtonController.index == thisIndex)
            {
                animator.SetBool("selected", true);
                if (Input.GetAxis("Submit") == 1 || mouseClicked)
                {
                    animator.SetBool("pressed", true);
                }
                else if (animator.GetBool("pressed"))
                {
                    animator.SetBool("pressed", false);
                    animatorFunctions.disableOnce = true;
                    OnClick();
                }
            }
            else
            {
                animator.SetBool("selected", false);
            }
        }
        else
        {
            if (bC.IsActive)
            {
                animator.SetBool("isActive", false);
                Hide(true);
            }
        }
    }

    public void OnClick()
    {
        
        if (menuButtonController.index == thisIndex)
        {
            
            switch (type)
            {
                case BUTTON_TYPE.START:
                    menuButtonController.StartLoading(nextSceneIndex);
                    break;
                case BUTTON_TYPE.END:
                    Debug.Log("I Quit!");
                    Application.Quit();
                    break;
                case BUTTON_TYPE.IN_MENU:
                    this.animatorFunctions.disableOnce = false;
                    StartCoroutine(menuButtonController.SwitchToWindow(nextMenuWindow));
                    break;
                case BUTTON_TYPE.RESUME_GAME:
                    menuButtonController.PauseGame(false);
                    break;
            }
        }
    }

    public void Hide(bool value)
    {
        bC.IsActive = !value;
        //bC.Image.enabled = !value;
        //bC.Text.enabled = !value;
    }

    public void MouseHoverEvent()
    {
        menuButtonController.UpdateActiveIndex(thisIndex);
    }
    public void MouseClick()
    {
        if(animator.GetBool("selected"))
        {
            mouseClicked = true;
        }
        
    }
    public void OnMouseDown()
    {
        mouseClicked = true;
    }
    public void OnMouseUp()
    {
        mouseClicked = false;
    }
}
