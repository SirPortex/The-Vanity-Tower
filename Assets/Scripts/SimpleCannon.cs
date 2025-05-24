using UnityEngine;
using UnityEngine.UI;

public class SimpleCannon : MonoBehaviour
{
    public Animator cannonAnimator;
    public Slider healthEnemySlider;
    public GameObject cannonVision;


    public bool isEnemyDefeated = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cannonAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(healthEnemySlider.value <= 5)
        {
            cannonAnimator.SetBool("EnemyDefeated", true);
            isEnemyDefeated = true;
            cannonVision.GetComponentInChildren<FieldOfView>().radius = 0f;
            ShotBullet shotBullet = GetComponentInChildren<ShotBullet>();
            shotBullet.CancelInvoke("Pium"); // Cancel any ongoing shooting
        }
    }
}
