using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLeftArm : MonoBehaviour
{
    public PlayerMovement playerMovement;

    [Header("Pushing")]

    public LayerMask pushableLayer;
    public bool pushing;
    public bool isPushing;

    [Header("Emotes")]

    public bool isEmoting;
    public int emoteIndex;
    public bool downCross;
    public bool readyToEmote = true;

    bool emote;

    [Header("Teddy")]

    public float teddyCooldown;
    public float teddyMaxTime;
    public bool teddyReady;
    public bool isTeddyActive = false;
    public bool readyToTeddy = false;

     bool teddy;

    [Header("Item")]

    public bool upCross;
    bool item;

    [Header("Objects")]

    public GameObject[] gameObjects;
    public int gameObjectIndex;

    RaycastHit hit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement= GetComponentInParent<PlayerMovement>();
        readyToEmote = true;
    }

    // Update is called once per frame
    void Update()
    {
        pushing = Physics.Raycast(transform.position, playerMovement.orientation.forward, out hit, 1f, pushableLayer);
        LeftArmInput();
        Push();
        LeftArmGamepad();

    }

    public void LeftArmInput()
    {
        emote = Input.GetButtonDown("Emote");
        teddy = Input.GetButtonDown("Teddy");

        if (emote && !isEmoting && !isTeddyActive && readyToEmote)
        {
            isTeddyActive = true;
            CancelRightArm();
            Invoke(nameof(EmoteHandler), 0.3f);
        }

        if (teddy && !isTeddyActive && teddyReady && !pushing)
        {
            CancelRightArm();
            Invoke(nameof(Teddy), 0.3f);
        }
    }

    public void LeftArmGamepad()
    {
        if (playerMovement.gamepad != null)
        {
            downCross = playerMovement.gamepad.dpad.down.IsPressed();
            upCross = playerMovement.gamepad.dpad.up.IsPressed();

            if (downCross && !isEmoting && !isTeddyActive && readyToEmote)
            {
                isTeddyActive = true;
                CancelRightArm();
                Invoke(nameof(EmoteHandler), 0.3f);
            }

            if (upCross)
            {

            }
        }
    }

    public void Push()
    {
        if (pushing && !isPushing)
        {
            isPushing = true;
            readyToEmote = false;
            playerMovement.readyToAttack = false;
            playerMovement.readyToBlock = false;
            playerMovement.animator.SetBool("IsPushing", true);
        }
        if(!pushing && isPushing)
        {
            isPushing = false;
            readyToEmote = true;
            playerMovement.readyToAttack = true;
            playerMovement.readyToBlock = true;
            playerMovement.animator.SetBool("IsPushing", false);
        }
    }

    public void EmoteHandler()
    {
        isEmoting = true;
        emoteIndex = Random.Range(1, 7);
        switch (emoteIndex)
        {
            case 1:
                emoteIndex = 0;
                playerMovement.animator.SetBool("Emote1", true);
                Invoke(nameof(StopEmote), 3f);

                break;
            case 2:
                emoteIndex = 0;
                playerMovement.animator.SetBool("Emote2", true);
                Invoke(nameof(StopEmote), 3f);

                break;
            case 3:
                emoteIndex = 0;
                playerMovement.animator.SetBool("Emote3", true);
                Invoke(nameof(StopEmote), 3f);

                break;
            case 4:
                emoteIndex = 0;
                playerMovement.animator.SetBool("Emote4", true);
                Invoke(nameof(StopEmote), 3f);

                break;
            case 5:
                emoteIndex = 0;
                playerMovement.animator.SetBool("Emote5", true);
                Invoke(nameof(StopEmote), 3f);

                break;
            case 6:

                playerMovement.animator.SetBool("Emote6", true);
                Invoke(nameof(StopEmote), 3f);

                break;
        }

    }

    public void StopEmote()
    {
        isTeddyActive = false;
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
        playerMovement.readyToAttack = false;
        playerMovement.readyToBlock = false;
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

    public void Teddy()
    {
        readyToTeddy = true;
        isTeddyActive = true;
        playerMovement.animator.SetBool("IsTeddy", true);
        isTeddyActive = true;
        teddyReady = false;
        Invoke(nameof(FinishTeddy), 9.8f);
    }

    public void FinishTeddy()
    {
        readyToTeddy = false;
        ActivateRightArm();
        playerMovement.animator.SetBool("IsTeddy", false);
        isTeddyActive = false;
        Invoke(nameof(TeddyCooldown), teddyMaxTime);
    }
    public void TeddyCooldown()
    {
        teddyReady = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, playerMovement.orientation.forward);
    }
}
