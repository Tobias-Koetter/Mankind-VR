using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(AudioSource))]
public class MenuButtonController : MonoBehaviour
{
    public bool inMenu = true;
    public int index;
    public MenuWindowInfo currentWindow;
    [SerializeField]
    private Loading loading;
    public MenuCamMovement cam;
    [SerializeField]
    bool keyDown;
    [SerializeField]
    int maxIndex;
    bool cursorLocked;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cursorLocked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (inMenu)
        {
            if (cursorLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                cursorLocked = false;
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                if (!keyDown)
                {
                    if (Input.GetAxis("Vertical") < 0)
                    {
                        if (index < maxIndex)
                        {
                            index++;
                        }
                        else
                        {
                            index = 0;
                        }
                    }
                    else if (Input.GetAxis("Vertical") > 0)
                    {
                        if (index > 0)
                        {
                            index--;
                        }
                        else
                        {
                            index = maxIndex;
                        }

                    }
                    keyDown = true;
                }
            }
            else
            {
                keyDown = false;
            }
        }
        else
        {
            if(!cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                cursorLocked = true;
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame(true);
            }

        }
    }

    public void UpdateActiveIndex(int index)
    {
        this.index = index;
    }

    public IEnumerator SwitchToWindow(MenuWindowInfo next)
    {
        currentWindow.isActive = false;
        if(next.camRelation == WindowCamRelation.ZOOM_IN && currentWindow.camRelation == WindowCamRelation.ZOOM_OUT)
        {
            cam.ZoomIn(true);
        }
        else if(next.camRelation == WindowCamRelation.ZOOM_OUT && currentWindow.camRelation == WindowCamRelation.ZOOM_IN)
        {
            cam.ZoomIn(false);
        }
        yield return new WaitForSeconds(1);

        currentWindow = next;
        maxIndex = currentWindow.buttonCount-1;
        index = 0;
        currentWindow.isActive = true;
        yield return null;
    }

    public void StartLoading(int index)
    {
        loading.gameObject.SetActive(true);
        StartCoroutine(loading.StartAsyncLoading(index));
    }
    public void PauseGame(bool boolVal)
    {

        if (boolVal)
        {
            //currentWindow.gameObject.SetActive(true);
            currentWindow.isActive = true;
            inMenu = true;

        }
        else if (!boolVal)
        {
            inMenu = false;
            currentWindow.isActive = false;
            //currentWindow.gameObject.SetActive(false);
        }
    }
}
