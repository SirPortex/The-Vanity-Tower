using UnityEngine;
using UnityEngine.UI;

public class CanvasTransition : MonoBehaviour
{
    public Animator blackTransitionanimator;
    public Image blackImage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; // Bloquea el cursor en el centro de la pantalla
        Cursor.visible = true; // Hace invisible el cursor

        blackTransitionanimator = GetComponent<Animator>();
        blackImage = GetComponentInChildren<Image>();
        Invoke(nameof(TurnOffImage), 1.2f);

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.readyToPlay)
        {
            blackImage.enabled = true;
            Invoke(nameof(TurnOnImage), 2f);
        }

        if (GameManager.instance.readyToExit)
        {
            blackImage.enabled = true;
        }
    }

    public void TurnOffImage()
    {
        blackImage.enabled = false;
    }

    public void TurnOnImage()
    {
        blackTransitionanimator.SetBool("TransitionON", true);

    }
}
