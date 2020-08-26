using UnityEngine;

public class MouseLook : MonoBehaviour , IPauseListener
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    float yRotation = 0f;

    private bool inMenu;


    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        AddToController();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inMenu)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            yRotation -= mouseY;
            yRotation = Mathf.Clamp(yRotation, -80, 65f);

            transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    public void AddToController()
    {
        GameInfo.AddPauseListener(this);
    }

    public void UpdateListener(bool newListenerValue)
    {
        inMenu = newListenerValue;
    }
}