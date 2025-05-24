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
        if(targetLookAt == null) return;

        Vector3 direction = targetLookAt.transform.position - rotationReference.transform.position;
        direction.y = 0;

        objetiveAngle = Quaternion.LookRotation(direction).eulerAngles.y; //Angulo al que se va a mirar

        currentAngle = Mathf.SmoothDampAngle(rotationReference.transform.eulerAngles.y, objetiveAngle, ref currentRotationSpeed, offset, rotationSpeed);

        rotationReference.transform.eulerAngles = new Vector3(0, currentAngle, 0); //Rotar el objeto
    }
}
