using UnityEngine;


public class Lava : MonoBehaviour
{
    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponentInParent<PlayerEssence>().TakeDamage(10f);
            player.GetComponentInParent<PlayerEssence>().borderAnimator.SetBool("IsFire", true);
            //Invoke(nameof(CancelFireAnimation), 1f);

            player.GetComponentInParent<PlayerMovement>().Jump();
        }
    }

    public void CancelFireAnimation()
    {
        player.GetComponentInParent<PlayerEssence>().borderAnimator.SetBool("IsFire", false);
    }
}
