using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition; // Referencia a la posición de la cámara

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition.position; // Actualiza la posición del objeto actual a la posición de la cámara
    }
}
