using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AdvancedVision : MonoBehaviour
{
    public Material VisionConeMaterial;

    public float VisionRange;
    public float VisionAngle;

    public LayerMask VisionObstructingLayer; //Capa con la que el rayo de vision puede chocar

    public int VisionConeResolution = 120; //Triangulos que se van a crear para el cono de vision

    Mesh VisionConeMesh;

    MeshFilter MeshFilter_;

    public List<GameObject> list = new List<GameObject>(); //Creamos una lista de objetos que seran los que entren en contacto con el cono

    public bool lol = false;
    public RaycastHit hit;
    GameObject collisionObject;


    void Start()
    {
        transform.AddComponent<MeshRenderer>().material = VisionConeMaterial;
        MeshFilter_ = transform.AddComponent<MeshFilter>();
        VisionConeMesh = new Mesh();
        VisionAngle *= Mathf.Deg2Rad;
    }

    void Update()
    {
        DrawVisionCone();//llamamos a la funcion que dibuja el cono de vision
    }

    public void DrawVisionCone()//Creamos el cono de vision (mesh)
    {

        int[] triangles = new int[(VisionConeResolution - 1) * 3];

        Vector3[] Vertices = new Vector3[VisionConeResolution + 1];

        Vertices[0] = Vector3.zero;

        float Currentangle = -VisionAngle / 2;

        float angleIcrement = VisionAngle / (VisionConeResolution - 1);

        float Sine;

        float Cosine;



        for (int i = 0; i < VisionConeResolution; i++)
        {

            Sine = Mathf.Sin(Currentangle);

            Cosine = Mathf.Cos(Currentangle);



            Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);

            Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);

            if (Physics.Raycast(transform.position, RaycastDirection, out hit, VisionRange, VisionObstructingLayer))
            {
                Vertices[i + 1] = VertForward * hit.distance;

                collisionObject = hit.collider.gameObject;

                if (collisionObject.GetComponentInParent<PlayerMovement>())
                {
                    lol = true;
                }

                else if (collisionObject.GetComponentInParent<PlayerMovement>() == null)
                {
                    lol = false;
                }
            }


            else
            {
                Vertices[i + 1] = VertForward * VisionRange;
            }

            Currentangle += angleIcrement;

        }

        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;

            triangles[i + 1] = j + 1;

            triangles[i + 2] = j + 2;
        }

        VisionConeMesh.Clear();

        VisionConeMesh.vertices = Vertices;

        VisionConeMesh.triangles = triangles;

        MeshFilter_.mesh = VisionConeMesh;

    }

    private void OnTriggerEnter(Collider other)
    {
        list.Add(other.gameObject); // Si se detecta una colision con el cono se añade un elemento a la lista
    }

    private void OnTriggerExit(Collider other)
    {
        list.Remove(other.gameObject); // Si el objecto que se detecta sale del cono se elimina un elemento de la lista
    }


    public List<GameObject> GetVisionList()
    {
        return list;
    }

}