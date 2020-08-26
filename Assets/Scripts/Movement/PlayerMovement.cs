using UnityEngine;

public class PlayerMovement : MonoBehaviour , IPauseListener
{
    public CharacterController controller;

    public float baseSpeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    float currentSpeed = 0f;
    bool isGrounded;
    bool isSprinting;
    bool inMenu;

    void Start()
    {
        AddToController();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inMenu)
        {
            isSprinting = Input.GetButton("Sprint");
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if (isSprinting)
            {
                currentSpeed = baseSpeed * 2;
            }
            else
            {
                currentSpeed = baseSpeed;
            }
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");


            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * currentSpeed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
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
