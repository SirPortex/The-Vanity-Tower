using UnityEngine;

public class Teddy : MonoBehaviour
{
    public PlayerLeftArm playerMovementTeddy;
    public Animator teddyAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovementTeddy = FindAnyObjectByType<PlayerLeftArm>();
        teddyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerMovementTeddy.isTeddyActive && !playerMovementTeddy.teddyReady && !playerMovementTeddy.isEmoting)
        {
            teddyAnimator.SetBool("IsDancing", true);
            Invoke(nameof(StopDancing), 9.8f);
        }
    }

    public void StopDancing()
    {
        teddyAnimator.SetBool("IsDancing", false);
    }

}
