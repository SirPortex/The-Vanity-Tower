using UnityEngine;
using UnityEngine.UI;

public class PlayerEssence : MonoBehaviour
{
    [Header("Sliders")]

    public Slider healthSlider;
    public Slider fearSlider;

    [Header("Health")]

    public float maxhealth = 100f;
    public float currentHealth;
    public float targetHealth;
    public float veloicity = 0f;
    public float smoothTime = 0.3f;


    [Header("Fear")]

    public float fear = 1f;

    [Header("Emotions")]

    public Animator borderAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxhealth;
        targetHealth = maxhealth;
        healthSlider.maxValue = maxhealth;
        healthSlider.value = maxhealth;


        fearSlider.value = fear;
        borderAnimator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = Mathf.SmoothDamp(currentHealth, targetHealth, ref veloicity, smoothTime);
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        targetHealth -= damage;
        targetHealth = Mathf.Clamp(targetHealth, 0, maxhealth);

        borderAnimator.SetBool("IsDamaged", true);
        Invoke(nameof(ReturnToIdle), 0.5f);
    }

    public void Heal(float heal)
    {
        targetHealth += heal;
        targetHealth = Mathf.Clamp(targetHealth, 0, maxhealth);
    }

    public void ReturnToIdle()
    {
        borderAnimator.SetBool("IsDamaged", false);
    }
}
