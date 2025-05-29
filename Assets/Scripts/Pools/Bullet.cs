using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float maxTime;

    public bool readyToDestroy = false;
    public bool damageCannon = false;

    public GameObject player;
    public GameObject bullet;
    public GameObject enemy;

    public LayerMask whatIsObstacle;

    //public GameObjectPool bulletPool;

    private float currentTime;
    private Vector3 _dir;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("SimpleCannon");
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= maxTime || readyToDestroy)
        {
            readyToDestroy = false;
            currentTime = 0;
            gameObject.SetActive(false); //Se "devuelve" a la pool
        }
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = speed * _dir;
    }

    public void SetDirection(Vector3 value)
    {
        _dir = value;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponentInParent<PlayerEssence>().TakeDamage(10f);
            player.GetComponentInParent<PlayerEssence>().borderAnimator.SetBool("IsDamaged", true);
        }

        if(other.gameObject.CompareTag("Block"))
        {
            Debug.Log("Bloqueado");

            damageCannon = true;

            SetDirection(_dir * -1f); //Invertir la direccion del disparo

        }

        if (other.gameObject.CompareTag("SimpleCannon") && damageCannon)
        {
            damageCannon = false;
            enemy.GetComponentInChildren<CannonCanvas>().TakeEnemyDamage(25f);
            readyToDestroy = true; //Bullet se "devuelve" a la pool
        }

        if (((1 << other.gameObject.layer) & whatIsObstacle) != 0 )
        {
            Debug.Log("Hit Obstacle");
            //bullet.SetActive(false); //Se "devuelve" a la pool
            readyToDestroy = true;
            
        }
    }
}
