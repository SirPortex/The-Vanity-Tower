using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class RotationReference : MonoBehaviour
{
    public GameObject rotationReference; //Referencia al objeto al que se va a rotar

    public GameObject targetLookAt; //Referencia al objeto al que se va a mirar

    private float rotationSpeed = 50f; //Velocidad de rotacion
    private Vector3 rotationAxis = Vector3.up; //Eje de rotacion

    private float offset = 0.2f; //Offset de la rotacion

    private float currentRotationSpeed = 0f;
    private float objetiveAngle;
    private float currentAngle;

    public void RotateObject()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime); //Rotar el objeto
    }

    public void LookAtTarget()
    {
        //Vector3 direction = new Vector3(targetLookAt.transform.position.x, 0, targetLookAt.transform.position.z) - rotationReference.transform.position; //Direccion a la que se va a mirar
        //Quaternion lookRotation = Quaternion.LookRotation(direction); //Rotacion a la que se va a mirar

        //float angleY = lookRotation.eulerAngles.y; //Angulo de rotacion en Y

        //float currentAngleY = rotationReference.transform.eulerAngles.y; //Angulo de rotacion actual en Y
        //float newAngle = Mathf.LerpAngle(currentAngleY, angleY, rotationSpeed * Time.deltaTime); //Nuevo angulo de rotacion

        //transform.eulerAngles = new Vector3(0, newAngle, 0); //Rotar el objeto

        if(targetLookAt == null) return;

        Vector3 direction = targetLookAt.transform.position - rotationReference.transform.position;
        direction.y = 0;

        objetiveAngle = Quaternion.LookRotation(direction).eulerAngles.y; //Angulo al que se va a mirar

        currentAngle = Mathf.SmoothDampAngle(rotationReference.transform.eulerAngles.y, objetiveAngle, ref currentRotationSpeed, offset, rotationSpeed);

        rotationReference.transform.eulerAngles = new Vector3(0, currentAngle, 0); //Rotar el objeto
    }
}
