using JetBrains.Annotations;
using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    [Header("Movement")]
    public float IdleSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float speed;

    bool sprint;

    [Header("Jumping")]

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;

    bool jumping;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    public float groundAdjustment;
    public float sphereCastRadius;
    public bool readyToCrouch;

    private float startYScale;

    bool crouch;

    [Header("Ground Check")]

    public float playerHeight;
    public bool grounded;
    public bool wantToStand;
    public LayerMask whatIsGround;

    [Header("Attack")]

    public int combo = 0;
    public int comboVar = 1;
    public bool isAttaking;

    public bool readyToAttack = true;

    bool attackPC;

    [Header("Block")]

    public GameObject blockCollider;

    public bool isBlocking;

    public bool readyToBlock = true;

    bool blockPC;


    [Header("Gamepad")]

    public Gamepad gamepad; // Referencia al Gamepad actual
    public float rtValue;
    public float ltValue;

    [Header("Other")]

    public Transform orientation;
    public MovementState state;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        idle,
        air
    }

    RaycastHit hit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Application.targetFrameRate = 120;
        //QualitySettings.vSyncCount = 1;

        isAttaking = false; // Inicializa el estado de ataque como falso

        rb = GetComponent<Rigidbody>(); 
        rb.freezeRotation = true; // Congela la rotaci�n del Rigidbody para evitar que se voltee

        readyToJump = true; // Inicializa el estado de salto como listo
        startYScale = transform.localScale.y; // Guarda la escala Y inicial del objeto para poder restaurarla al salir de la posicion de agachado
    }

    // Update is called once per frame
    void Update()
    {

        gamepad = Gamepad.current; // Obtiene la referencia al Gamepad actual

        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); // Comprobamos si estamos tocando el suelo mediante un raycast hacia abajo
        //wantToStand = Physics.Raycast(transform.position, Vector3.up, playerHeight * 0.5f + 0.2f, whatIsGround); // Comprobamos si queremos levantarnos mediante un raycast hacia arriba
        grounded = Physics.SphereCast(transform.position, sphereCastRadius, Vector3.down, out hit, playerHeight * 0.5f + groundAdjustment, whatIsGround); // Comprobamos si estamos tocando el suelo mediante un spherecast hacia abajo
        wantToStand = Physics.SphereCast(transform.position, 0.4f, Vector3.up, out hit, playerHeight * 0.5f - 0.3f, whatIsGround); // Comprobamos si queremos levantarnos mediante un spherecast hacia arriba

        Attack01();
        Attack02();
        MyInput();
        SpeedControl();
        StateHandler();
        DetectController();

        if(grounded)
        {
            rb.linearDamping = groundDrag; // Si estamos en el suelo, aplicamos la friccion
        }
        else
        {
            rb.linearDamping = 0; // Si estamos en el aire, no aplicamos friccion
        }
    }

    public void DetectController()
    {
        if(gamepad != null)
        {
            rtValue = gamepad.rightTrigger.ReadValue(); // Lee el valor del gatillo derecho del Gamepad
            ltValue = gamepad.leftTrigger.ReadValue(); // Lee el valor del gatillo izquierdo del Gamepad
            if(rtValue >= 0.5f && !isAttaking && readyToAttack && ltValue < 0.1f) // Si el valor del gatillo derecho es mayor que 0.5
            {
                combo = comboVar;
                isAttaking = true;
                isBlocking = true;
            }
            else if(ltValue >= 0.2f && !isBlocking && readyToBlock && rtValue < 0.4f)
            {
                isAttaking = true;
                isBlocking = true;
                animator.SetBool("IsBlocking", true);
                StartCoroutine(BlockDelay());
                //Invoke(nameof(CanBlockAgain), 1f);
            }

        }

    }
    public void Attack01()
    {
        if(combo == 1 && isAttaking)
        {
            isBlocking = true;
            Debug.Log("01");
            animator.SetBool("IsAttacking01", true);
            animator.SetBool("IsAttacking02", false);
            Invoke(nameof(AttackDelay01), 0.4f);
        }
    }

    public void Attack02()
    {
        if (combo == 2 && isAttaking)
        {
            isBlocking = true;
            Debug.Log("02");
            animator.SetBool("IsAttacking02", true);
            animator.SetBool("IsAttacking01", false);
            Invoke(nameof(AttackDelay02), 0.4f);

        }
    }

    void FixedUpdate()
    {
        MovePlayer(); // Llama a la funcion MovePlayer para mover al jugador
    }

    public void AttackDelay01()
    {
        combo = 0;
        comboVar = 2;
        animator.SetBool("IsAttacking01", false);
        isAttaking = false;
        isBlocking = false;
    }

    public void AttackDelay02()
    {
        combo = 0;
        comboVar = 1;
        animator.SetBool("IsAttacking02", false);
        isAttaking = false;
        isBlocking = false;
    }

    private IEnumerator BlockDelay()
    {
        blockCollider.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("IsBlocking", false);
        blockCollider.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        isBlocking = false;
        isAttaking = false;
        yield return new WaitForSeconds(0.2f);

    }

    public void CanBlockAgain()
    {
        isAttaking = false;
        isBlocking = false;
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); 
        verticalInput = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("JumpUp");
        sprint = Input.GetButton("Sprint");
        crouch = Input.GetButton("Crouch");
        attackPC = Input.GetButton("Attack");
        blockPC = Input.GetButton("Block");

        if (blockPC && !isBlocking && readyToBlock && !attackPC)
        {
            isAttaking = true;
            isBlocking = true;

            combo = 0;

            animator.SetBool("IsBlocking", true);
            StartCoroutine(BlockDelay());
            //Invoke(nameof(CanBlockAgain), 1f);
        }

        if (attackPC && !isAttaking && readyToAttack && !blockPC)
        {
            combo = comboVar;
            isAttaking = true;
            isBlocking = true;
        }

        if (Input.GetButton("JumpUp"))
        {
            //animator.SetBool("IsJumping", true); // Cambia el estado de la animacion a saltando
            //StartCoroutine(JumpingAnim()); // Llama a la coroutine JumpingAnim para reproducir la animacion de salto
            animator.SetBool("IsWalking", false); // Cambia el estado de la animacion a no caminando
            animator.SetBool("IsRunning", false); // Cambia el estado de la animacion a no corriendo
        }
        else
        {
            animator.SetBool("IsJumping", false); // Cambia el estado de la animacion a no saltando
        }

        if(Input.GetButton("JumpUp") && readyToJump && grounded && !wantToStand) // Si se presiona la tecla de salto, estamos listos para saltar y estamos en el suelo
        {
            animator.SetBool("IsJumping", true); // Cambia el estado de la animacion a saltando
            StartCoroutine(JumpingAnim()); // Llama a la coroutine JumpingAnim para reproducir la animacion de salto
            readyToJump = false; // Cambia el estado a no listo para saltar
            Jump(); // Llama a la funcion Jump para saltar
            Invoke(nameof(ResetJump), jumpCooldown); // Reinicia el estado de salto después de un tiempo de espera
        }

        if (Input.GetButtonDown("Crouch") && grounded && readyToCrouch)
        {
            readyToCrouch = false;
            readyToJump = false;
            GetDown();
            groundAdjustment = -0.1f;
            sphereCastRadius = 0.25f;
        }

        if (Input.GetButtonUp("Crouch") && grounded && !readyToCrouch)
        {
            readyToCrouch = true;
            readyToJump = true;
            GetUp();
            groundAdjustment = 0f;
            sphereCastRadius = 0.3f;
        }
    }

    public IEnumerator JumpingAnim()
    {
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("IsJumping", false);
    }

    private void GetDown()
    {
        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z); // Cambia la escala Y del objeto a la escala de agachado
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); // Ajuste para que al agacharse no estemos flotando, añadiendo una fuerza hacia abajo al Rigidbody 
    }

    private void GetUp()
    {
        if (!wantToStand)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

            //transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z); // Cambia la escala Y del objeto a la escala normal
            //rb.AddForce(Vector3.up * 0.1f, ForceMode.Impulse); // Ajuste para que al levantarse no estemos flotando, añadiendo una fuerza hacia arriba al Rigidbody
            state = MovementState.idle;

        }
    }

    private void StateHandler()
    {
        if(grounded && Input.GetButton("Crouch")) // Si se presiona la tecla de agachado y estamos en el suelo
        {
            //readyToJump = false;
            state = MovementState.crouching; // Cambia el estado a crouching
            speed = crouchSpeed; // Establece la velocidad al valor de velocidad de agachado
        }
        else if (grounded && Input.GetButton("Sprint") && !wantToStand) // Si estamos en el suelo y se presiona la tecla de sprint
        {

            readyToCrouch = true;
            state = MovementState.sprinting; // Cambia el estado a sprinting

            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z); // Cambia la escala Y del objeto a la escala normal

            speed = sprintSpeed; // Establece la velocidad al valor de velocidad de sprint

            animator.SetBool("IsRunning", true);

            animator.SetBool("IsWalking", false);
            animator.SetBool("IsIdle", false);
        }
        else if(grounded && rb.linearVelocity.magnitude >= 0.2f && !wantToStand) // Si estamos en el suelo
        {
            readyToCrouch = true;

            state = MovementState.walking; // Cambia el estado a walking

            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z); // Cambia la escala Y del objeto a la escala normal

            speed = walkSpeed; // Establece la velocidad al valor de velocidad de caminar
            animator.SetBool("IsWalking", true); 

            animator.SetBool("IsIdle", false);
            animator.SetBool("IsRunning", false);

        }
        else if(!grounded && !wantToStand) // Si no estamos en el suelo
        {
            //readyToCrouch = false;
            state = MovementState.air; // Cambia el estado a air
        }

        else if (grounded && !wantToStand)
        {
            readyToCrouch = true;


            state = MovementState.idle; // Cambia el estado a idle

            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z); // Cambia la escala Y del objeto a la escala normal

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
            
            rb.AddForce(Vector3.down * 10f); // Aplica una fuerza hacia abajo al Rigidbody para simular la gravedad
        }
    }

    private void OnCollisionEnter(Collision other) // Se llama cuando el objeto colisiona con otro objeto
    {
        if(other.gameObject.tag == "Ground") // Si el objeto con el que colisionamos tiene la etiqueta "Ground"
        {
            rb.linearDamping = groundDrag; // Establece la friccion del Rigidbody al valor de friccion del suelo
            rb.linearDamping = 0; // Establece la resistencia del Rigidbody a 0
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Reinicia la velocidad vertical del Rigidbody a 0

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

    public void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Reinicia la velocidad vertical del Rigidbody a 0 antes de aplicar la fuerza de salto

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); // Aplica una fuerza de salto al Rigidbody en la dirección hacia arriba del objeto actual, "Impulse" por que se aplica una vez.
    }

    private void ResetJump()
    {
        readyToJump = true; // Reinicia el estado de salto, permitiendo que el jugador salte nuevamente
    }

    private void OnDrawGizmos() // Se llama para dibujar Gizmos en la escena
    {
        Gizmos.color = Color.red; // Establece el color de los Gizmos a rojo
        //Gizmos.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.2f)); // Dibuja un rayo hacia abajo desde la posicion del objeto actual, con una longitud igual a la mitad de la altura del jugador + 0.2f

        Vector3 start = transform.position;
        Vector3 end = start + Vector3.down * (playerHeight * 0.5f + groundAdjustment);
        Gizmos.DrawWireSphere(end, sphereCastRadius);
        //Gizmos.DrawLine(start, end);

        Gizmos.color = Color.green; // Establece el color de los Gizmos a verde
        //Gizmos.DrawRay(transform.position, Vector3.up * (playerHeight * 0.5f + 0.2f)); // Dibuja un rayo hacia arriba desde la posicion del objeto actual, con una longitud igual a la mitad de la altura del jugador + 0.2f

        Vector3 start2 = transform.position;
        Vector3 end2 = start2 + Vector3.up * (playerHeight * 0.5f - 0.3f);
        Gizmos.DrawWireSphere(end2, 0.4f);
        //Gizmos.DrawLine(start2, end2);

        Gizmos.color = Color.blue; // Establece el color de los Gizmos a azul
        Gizmos.DrawRay(transform.position, orientation.forward * verticalInput + orientation.right * horizontalInput); // Dibuja un rayo en la direccion de movimiento

    }
}