using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX, sensY, minUpRotation, maxUpRotation;

    public Transform orientation;

    private float _xRotation, _yRotation;

    float x, y; // Variables para almacenar la entrada del usuario en los ejes X e Y



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor en el centro de la pantalla
        Cursor.visible = false; // Hace invisible el cursor

    }

    // Update is called once per frame
    void Update()
    {
        //MyInput(); // Llama a la funci�n MyInput para obtener la entrada del usuario
        Look(); // Llama a la funci�n Look para actualizar la rotaci�n de la c�mara

    }

    public void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
    }

    public void Look()
    {
        //float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX; // Obtiene el movimiento del rat�n en el eje X
        //float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY; // Obtiene el movimiento del rat�n en el eje Y

        float mouseX = Input.GetAxisRaw("Horizontal 1") * Time.deltaTime * sensX; // Obtiene el movimiento del rat�n en el eje X
        float mouseY = Input.GetAxisRaw("Vertical 1") * Time.deltaTime * sensX; // Obtiene el movimiento del rat�n en el eje Y

        _yRotation += mouseX; // Actualiza la rotaci�n en el eje Y
        _xRotation -= mouseY; // Actualiza la rotaci�n en el eje X

        _xRotation = Mathf.Clamp(_xRotation, minUpRotation, maxUpRotation); // Limita la rotaci�n en el eje X entre -90 y 90 grados, "clampeamos" el valor al valor desado.

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0); // Aplica la rotaci�n al objeto actual
        orientation.rotation = Quaternion.Euler(0, _yRotation, 0); // Aplica la rotaci�n al objeto de orientaci�n
        //transform.localRotation = Quaternion.Euler(0, _yRotation, 0); // Aplica la rotaci�n al objeto actual
    }
}
