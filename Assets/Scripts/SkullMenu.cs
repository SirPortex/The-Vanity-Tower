using UnityEngine;

public class SkullMenu : MonoBehaviour
{
    public bool isReady = false;
    public Animator skullAnimator;
    public float time = 0f;
    public float lerpSpeed = 0.1f;
    public float rotationDuration = 3f;
    RotateAtCursor rotateAtCursor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skullAnimator = GetComponent<Animator>();
        rotateAtCursor = GetComponent<RotateAtCursor>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.readyToPlay && !isReady && time <= 3f)
        {
            //time += Time.deltaTime;
            //float progress = Mathf.Clamp01(time / rotationDuration);
            skullAnimator.SetBool("IsPlaying", true);
            //isReady = true;
            rotateAtCursor.enabled = false;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,180,0), time += Time.deltaTime * lerpSpeed);
        }
        if (time >= rotationDuration)
        {
            time = 0;
        }
        if(GameManager.instance.readyToExit)
        {
            skullAnimator.SetBool("IsPlaying", true);
        }
    }
}
