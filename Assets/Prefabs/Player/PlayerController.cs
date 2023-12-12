using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ce script provient de https://sharpcoderblog.com/blog/unity-3d-fps-controller#:~:text=FPS%20(or%20First%2Dperson%20shooter,move%20freely%20around%20the%20level.

[RequireComponent(typeof(CharacterController))]//Assure d'avoir le composant Character Controller attaché au GameObject

public class PlayerController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    private Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        // Vérouille le curseur
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Calcul de la direction en fonction des axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Appuyer sur "Left Shift" permet de courir
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && verticalInput > 0 && Mathf.Abs(horizontalInput) < 0.5f;

        if (isRunning && characterController.isGrounded)
            animator.SetBool("IsRunning", true);
        else
            animator.SetBool("IsRunning", false);

        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * verticalInput : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * horizontalInput : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Permet de sauter si le joueur touche le sol
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
            if (animator != null)
            {
                animator.SetBool("IsJumping", true); // Set IsJumping to true when jumping
            }
        }
        else
        {
            moveDirection.y = movementDirectionY;
            if (animator != null)
            {
                animator.SetBool("IsJumping", false); // Set IsJumping to false when not jumping
            }
        }

        // Applique la gravité lorsque le joueur ne touche pas au sol
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Déplace le joueur
        characterController.Move(moveDirection * Time.deltaTime);

        // Rotation du joueur et de la caméra
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if (animator != null)
        {
            animator.SetFloat("Horizontal", horizontalInput);
            animator.SetFloat("Vertical", verticalInput);


        }

    }
}