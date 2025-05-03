using UnityEngine;

public class PlayerLeftArm : MonoBehaviour
{
    public PlayerMovement playerMovement;
    Animator animatorLeftArm;

    [Header("Pushing")]

    public bool pushing;

    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerMovement.GetComponent<PlayerMovement>();
        playerMovement= GetComponentInParent<PlayerMovement>();
        animatorLeftArm = playerMovement.animator;
    }

    // Update is called once per frame
    void Update()
    {
        pushing = Physics.Raycast(transform.position, playerMovement.orientation.forward, out RaycastHit hit, 1f, playerMovement.whatIsGround);

        LeftArmInput();
        Push();
    }

    private void LeftArmInput()
    {
        
    }

    public void Push()
    {
        if (pushing)
        {
            playerMovement.animator.SetBool("IsPushing", true);
        }
        else
        {
            playerMovement.animator.SetBool("IsPushing", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, playerMovement.orientation.forward);
    }
}
