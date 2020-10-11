using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Gradient Gradient;
    public Image _Fill;
    
    private Slider _Slider;

    private void Awake()
    {
        _Slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(int maxHealth)
    {
        _Slider.maxValue = maxHealth;
        _Slider.value = maxHealth;

        _Fill.color = Gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        _Slider.value = health;

        _Fill.color = Gradient.Evaluate(_Slider.normalizedValue);
    }
}
