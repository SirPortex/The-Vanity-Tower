using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBullet : MonoBehaviour
{
    public GameObjectPool bulletPool;
    public Animator animator;

    public bool canShoot = false;

    FieldOfView fov;


    // Start is called before the first frame update
    void Start()
    {
        fov = GetComponentInParent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fov.canSeePlayer && !canShoot)
        {
            canShoot = true;
            Invoke(nameof(Pium), 2f);
        }
    }

    void Pium()
    {
        Debug.Log("Pium");

        GameObject obj = bulletPool.GimmeInactiveGameObject();

        animator.SetBool("Shooting", true); //animacion de disparo
        Invoke(nameof(DisableAnimation), 1f); //desactivar la animacion de disparo

        if (obj)
        {
            obj.SetActive(true); //quitar el boli del estuche, ya no esta disponible en la poool
            obj.transform.position = transform.position;
            obj.GetComponent<Bullet>().SetDirection(transform.forward);
            canShoot = false;
        }
    }

    void DisableAnimation()
    {
        animator.SetBool("Shooting", false);
    }
}
