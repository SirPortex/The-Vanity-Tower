using UnityEngine;

public class RotateAtCursor : MonoBehaviour
{
    public float cameraDsitance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posicionMouse = Input.mousePosition;
        posicionMouse.z = cameraDsitance;
        Vector3 posicionMundo = Camera.main.ScreenToWorldPoint(posicionMouse);

        Quaternion rotacionDeseada = Quaternion.LookRotation(posicionMundo - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, cameraDsitance * Time.deltaTime);
    }
}
