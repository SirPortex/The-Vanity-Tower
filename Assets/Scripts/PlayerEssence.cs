using UnityEngine;
using UnityEngine.SceneManagement;
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

    public bool fearReady;
    public bool canFear;

    public float maxFear = 100f;
    public float currentFear;
    public float targetFear;
    public float fearVelocity = 0f;
    public float fearSmoothTime = 0.3f;

    [Header("Emotions")]

    public Animator borderAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        borderAnimator.GetComponent<Animator>();

        currentHealth = maxhealth;
        targetHealth = maxhealth;
        healthSlider.maxValue = maxhealth;
        healthSlider.value = maxhealth;

        currentFear = 0;
        targetFear = 0;
        fearSlider.maxValue = maxFear;
        fearSlider.value = 0;
        fearReady = false;
        canFear = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = Mathf.SmoothDamp(currentHealth, targetHealth, ref veloicity, smoothTime);
        healthSlider.value = currentHealth;

        currentFear = Mathf.SmoothDamp(currentFear, targetFear, ref fearVelocity, fearSmoothTime);
        fearSlider.value = currentFear;

        if (fearReady == false && canFear)
        {
            fearReady = true;
            IncreaseFear(1f);
            Invoke(nameof(FearOff), 1f);
        }

        if (targetHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void TakeDamage(float damage)
    {
        targetHealth -= damage;
        targetHealth = Mathf.Clamp(targetHealth, 0, maxhealth);

        IncreaseFear(10f);

        //borderAnimator.SetBool("IsDamaged", true);
        Invoke(nameof(ReturnToIdle), 0.5f);
    }

    public void Heal(float heal)
    {
        targetHealth += heal;
        targetHealth = Mathf.Clamp(targetHealth, 0, maxhealth);
    }

    public void IncreaseFear(float fearIncrease)
    {
        targetFear += fearIncrease;
        targetFear = Mathf.Clamp(targetFear, 0, maxFear);
    }

    public void DecreaseFear(float fearDecrease)
    {
        targetFear -= fearDecrease;
        targetFear = Mathf.Clamp(targetFear, 0, maxFear);
    }

    public void FearOff()
    {
        fearReady = false;
    }

    public void ReturnToIdle()
    {
        borderAnimator.SetBool("IsDamaged", false);
        borderAnimator.SetBool("IsFire", false);
    }
}
