using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition; // Referencia a la posici�n de la c�mara

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition.position; // Actualiza la posici�n del objeto actual a la posici�n de la c�mara
    }
}
