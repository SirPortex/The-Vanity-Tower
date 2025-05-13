using UnityEngine;

public class CameraMenu : MonoBehaviour
{
    public Animator cameraMenuAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraMenuAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.readyToPlay)
        {
            cameraMenuAnimator.SetBool("CameraOn", true);
        }
    }
}
