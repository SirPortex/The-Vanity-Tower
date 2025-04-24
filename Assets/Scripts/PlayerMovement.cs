using UnityEngine;


[System.Serializable]
public struct Weapon
{
    public Mesh mesh;
    public Material material;
}

public class PlayerMovement : MonoBehaviour
{
    public Weapon[] weapons;
    public int diegoIndex;
    public GameObject sword;

    [Header("Movement")]
    public float speed;
    public float groundDrag;

    [Header("Ground Check")]

    public float playerHeight;

    public bool grounded; 

    public LayerMask whatIsGround;

    public Transform orientation;

    float horizontalInput, verticalInput; // Variables para almacenar la entrada del usuario en los ejes X e Y

    Vector3 moveDirection; // Variable para almacenar la direcci�n de movimiento

    Rigidbody rb; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        rb.freezeRotation = true; // Congela la rotaci�n del Rigidbody para evitar que se voltee
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); // Comprobamos si estamos tocando el suelo mediante un raycast hacia abajo

        MyInput(); 

        if(grounded)
        {
            rb.linearDamping = groundDrag; // Si estamos en el suelo, aplicamos la fricci�n
        }
        else
        {
            rb.linearDamping = 0; // Si estamos en el aire, no aplicamos fricci�n
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            diegoIndex++;
            diegoIndex %= weapons.Length;
            sword.GetComponent<MeshFilter>().mesh = weapons[diegoIndex].mesh;
            sword.GetComponent<MeshRenderer>().material = weapons[diegoIndex].material;
        }
    }

    void FixedUpdate()
    {
        MovePlayer(); // Llama a la funci�n MovePlayer para mover al jugador
    }


    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); 
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; // Calcula la direcci�n de movimiento en funci�n de la orientaci�n y la entrada del usuario, asi siempre caminamos hacia donde estamos mirando.

        rb.AddForce(moveDirection.normalized * speed *10f , ForceMode.Force); // Aplica una fuerza al Rigidbody en la direcci�n de movimiento normalizada, multiplicada por la velocidad y un factor de 10 para aumentar la fuerza aplicada
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
}
