using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBullet : MonoBehaviour
{
    public GameObjectPool bulletPool;
    FieldOfView fov;


    // Start is called before the first frame update
    void Start()
    {
        fov = GetComponentInParent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fov.canSeePlayer)
        {
            Invoke(nameof(Pium), 1f);
        }
    }

    void Pium()
    {
        Debug.Log("Pium");

        GameObject obj = bulletPool.GimmeInactiveGameObject();

        if (obj)
        {
            obj.SetActive(true); //quitar el boli del estuche, ya no esta disponible en la poool
            obj.transform.position = transform.position;
            obj.GetComponent<Bullet>().SetDirection(transform.forward);
        }
    }
}
