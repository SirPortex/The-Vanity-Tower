using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLeftArm : MonoBehaviour
{
    public PlayerMovement playerMovement;

    [Header("Pushing")]

    public bool pushing;

    [Header("Emotes")]

    public bool isEmoting;
    public int emoteIndex;
    public bool downCross;

    bool emote;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerMovement.GetComponent<PlayerMovement>();
        playerMovement= GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        pushing = Physics.Raycast(transform.position, playerMovement.orientation.forward, out RaycastHit hit, 1f, playerMovement.whatIsGround);


        LeftArmInput();
        Push();
        LeftArmGamepad();
    }

    public void LeftArmInput()
    {
        emote = Input.GetButtonDown("Emote");

        if (emote && !isEmoting)
        {
            if(!playerMovement.isAttaking || !playerMovement.isBlocking)
            {
                playerMovement.readyToAttack = false;
                playerMovement.readyToBlock = false;
                playerMovement.isBlocking = true;
                playerMovement.isAttaking = true;

                EmoteHandler();
            }

        }
    }

    public void LeftArmGamepad()
    {
        if (playerMovement.gamepad != null)
        {
            downCross = playerMovement.gamepad.dpad.down.IsPressed();

            if (downCross && !isEmoting)
            {
                if (!playerMovement.isAttaking || !playerMovement.isBlocking)
                {
                    playerMovement.readyToAttack = false;
                    playerMovement.readyToBlock = false;
                    Invoke(nameof(EmoteHandler), 0.35f);
                    playerMovement.isBlocking = true;
                    playerMovement.isAttaking = true;

                    //EmoteHandler();
                }
            }
        }
    }

    public void Push()
    {
        if (pushing)
        {
            playerMovement.animator.SetBool("IsAttacking01", false);
            playerMovement.animator.SetBool("IsAttacking02", false);
            playerMovement.animator.SetBool("IsBlocking", false);
            isEmoting = true;
            playerMovement.animator.SetBool("IsPushing", true);
        }
        else
        {
            isEmoting = false;
            playerMovement.animator.SetBool("IsPushing", false);
        }
    }

    public void EmoteHandler()
    {
        CancelRightArm();

        isEmoting = true;
        emoteIndex = Random.Range(1, 7);
        switch (emoteIndex)
        {
            case 1:

                playerMovement.animator.SetBool("Emote1", true);
                Invoke("StopEmote", 3f);

                break;
            case 2:

                playerMovement.animator.SetBool("Emote2", true);
                Invoke("StopEmote", 3f);

                break;
            case 3:

                playerMovement.animator.SetBool("Emote3", true);
                Invoke("StopEmote", 3f);

                break;
            case 4:

                playerMovement.animator.SetBool("Emote4", true);
                Invoke("StopEmote", 3f);

                break;
            case 5:

                playerMovement.animator.SetBool("Emote5", true);
                Invoke("StopEmote", 3f);

                break;
            case 6:

                playerMovement.animator.SetBool("Emote6", true);
                Invoke("StopEmote", 3f);

                break;
        }

    }

    public void StopEmote()
    {
        ActivateRightArm();
        emoteIndex = 0;
        isEmoting = false;
        playerMovement.animator.SetBool("Emote1", false);
        playerMovement.animator.SetBool("Emote2", false);
        playerMovement.animator.SetBool("Emote3", false);
        playerMovement.animator.SetBool("Emote4", false);
        playerMovement.animator.SetBool("Emote5", false);
        playerMovement.animator.SetBool("Emote6", false);
    }

    public void CancelRightArm()
    {
        playerMovement.isBlocking = true;
        playerMovement.isAttaking = true;
    }

    public void ActivateRightArm()
    {
        playerMovement.isBlocking = false;
        playerMovement.isAttaking = false;
        playerMovement.readyToAttack = true;
        playerMovement.readyToBlock = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, playerMovement.orientation.forward);
    }
}
