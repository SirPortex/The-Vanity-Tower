using UnityEngine;
using UnityEngine.UI;

public class CannonCanvas : MonoBehaviour
{
    public Slider healthEnemySlider;

    public float maxEnemyHealth = 100f;
    public float currentEnemyHealth;
    public float targetEnemyHealth;
    public float velocity = 0f;
    public float smoothTime = 0.3f;

    private Camera playerCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentEnemyHealth = maxEnemyHealth;
        targetEnemyHealth = maxEnemyHealth;
        healthEnemySlider.maxValue = maxEnemyHealth;
        healthEnemySlider.value = maxEnemyHealth;

        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        currentEnemyHealth = Mathf.SmoothDamp(currentEnemyHealth, targetEnemyHealth, ref velocity, smoothTime);
        healthEnemySlider.value = currentEnemyHealth;

        transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.transform.position);
    }

    public void TakeEnemyDamage(float damage)
    {
        targetEnemyHealth -= damage;
        targetEnemyHealth = Mathf.Clamp(targetEnemyHealth, 0, maxEnemyHealth);
    }
}
