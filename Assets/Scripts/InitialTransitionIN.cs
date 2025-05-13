using UnityEngine;
using UnityEngine.UI;

public class InitialTransitionIN : MonoBehaviour
{
    public Animator blackTransitionanimator;
    public Image blackImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blackTransitionanimator = GetComponent<Animator>();
        blackImage = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Invoke(nameof(StartGame), 0.5f);
    }

    public void StartGame()
    {
        blackImage.enabled = false;
    }
}
