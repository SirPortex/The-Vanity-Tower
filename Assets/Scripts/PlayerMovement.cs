using System;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    [Header("Movement")]
    public float IdleSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    bool sprint;
    public float speed;

    [Header("Jumping")]

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    bool jumping;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    bool crouch;

    [Header("Ground Check")]

    public float playerHeight;
    public bool grounded; 
    public LayerMask whatIsGround;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        idle,
        air
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        rb.freezeRotation = true; // Congela la rotaci�n del Rigidbody para evitar que se voltee

        readyToJump = true; // Inicializa el estado de salto como listo
        startYScale = transform.localScale.y; // Guarda la escala Y inicial del objeto para poder restaurarla al salir de la posicion de agachado
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); // Comprobamos si estamos tocando el suelo mediante un raycast hacia abajo

        MyInput();
        SpeedControl();
        StateHandler();

        if(grounded)
        {
            rb.linearDamping = groundDrag; // Si estamos en el suelo, aplicamos la friccion
        }
        else
        {
            rb.linearDamping = 0; // Si estamos en el aire, no aplicamos friccion
        }

        if (Input.GetKey(KeyCode.F))
        {
            animator.SetBool("IsTeddy", true);
            Debug.Log("Teddy");
        }
    }

    void FixedUpdate()
    {
        MovePlayer(); // Llama a la funcion MovePlayer para mover al jugador
    }


    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); 
        verticalInput = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        sprint = Input.GetButton("Fire3");
        crouch = Input.GetButton("Crouch");


        if(Input.GetButton("Jump") && readyToJump && grounded) // Si se presiona la tecla de salto, estamos listos para saltar y estamos en el suelo
        {
            readyToJump = false; // Cambia el estado a no listo para saltar
            Jump(); // Llama a la funcion Jump para saltar
            Invoke(nameof(ResetJump), jumpCooldown); // Reinicia el estado de salto después de un tiempo de espera
        }

        if (Input.GetButtonDown("Crouch") && grounded)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z); // Cambia la escala Y del objeto a la escala de agachado
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); // Ajuste para que al agacharse no estemos flotando, añadiendo una fuerza hacia abajo al Rigidbody
        }

        if(Input.GetButtonUp("Crouch") && grounded)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z); // Cambia la escala Y del objeto a la escala normal
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse); // Ajuste para que al levantarse no estemos flotando, añadiendo una fuerza hacia arriba al Rigidbody
        }

        
    }

    private void StateHandler()
    {
        if(grounded && Input.GetButton("Crouch")) // Si se presiona la tecla de agachado y estamos en el suelo
        {
            state = MovementState.crouching; // Cambia el estado a crouching
            speed = crouchSpeed; // Establece la velocidad al valor de velocidad de agachado
        }
        else if (grounded && Input.GetButton("Fire3")) // Si estamos en el suelo y se presiona la tecla de sprint
        {
            state = MovementState.sprinting; // Cambia el estado a sprinting
            speed = sprintSpeed; // Establece la velocidad al valor de velocidad de sprint

            animator.SetBool("IsRunning", true);

            animator.SetBool("IsWalking", false);
            animator.SetBool("IsIdle", false);
        }
        else if(grounded && rb.linearVelocity.magnitude >= 0.1f) // Si estamos en el suelo
        {
            state = MovementState.walking; // Cambia el estado a walking
            speed = walkSpeed; // Establece la velocidad al valor de velocidad de caminar
            animator.SetBool("IsWalking", true); 

            animator.SetBool("IsIdle", false);
            animator.SetBool("IsRunning", false);

        }
        else if(!grounded) // Si no estamos en el suelo
        {
            state = MovementState.air; // Cambia el estado a air
        }

        else if (grounded)
        {
            state = MovementState.idle; // Cambia el estado a idle

            animator.SetBool("IsIdle", true); 

            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; // Calcula la direccion de movimiento en funcion de la orientacion y la entrada del usuario, asi siempre caminamos hacia donde estamos mirando.

        if(grounded) // Si estamos en el suelo
        {
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force); // Aplica una fuerza al Rigidbody en la direcci�n de movimiento normalizada, multiplicada por la velocidad y un factor de 10 para aumentar la fuerza aplicada
        }
        else if(!grounded) // Si no estamos en el suelo
        {
            rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force); // Aplica una fuerza al Rigidbody en la direcci�n de movimiento normalizada, multiplicada por la velocidad y un factor de 10 para aumentar la fuerza aplicada
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if(flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Reinicia la velocidad vertical del Rigidbody a 0 antes de aplicar la fuerza de salto

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); // Aplica una fuerza de salto al Rigidbody en la dirección hacia arriba del objeto actual, "Impulse" por que se aplica una vez.
    }

    private void ResetJump()
    {
        readyToJump = true; // Reinicia el estado de salto, permitiendo que el jugador salte nuevamente
    }
}
