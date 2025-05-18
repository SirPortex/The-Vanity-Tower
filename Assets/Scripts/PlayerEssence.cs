using UnityEngine;
using UnityEngine.UI;

public class PlayerEssence : MonoBehaviour
{
    [Header("Sliders")]

    public Slider healthSlider;
    public Slider fearSlider;

    [Header("Health")]

    public float health = 1f;

    [Header("Fear")]

    public float fear = 1f;

    [Header("Emotions")]

    public Animator borderAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = healthSlider.value;
        fearSlider.value = fear;
        borderAnimator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
