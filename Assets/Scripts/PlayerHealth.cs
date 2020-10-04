using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int MaxHealth = 100;
    public HealthBar HealthBar;

    private int CurrentHealth = 0;
    private Animator _Animator;

    private void Awake()
    {
        _Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
        HealthBar.SetMaxHealth(MaxHealth);
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        _Animator.SetTrigger("Hurt");

        if (CurrentHealth <= 0)
        {
            Die(); // or game over
        }
        else
        {
            HealthBar.SetHealth(CurrentHealth);
        }
    }

    private void Die()
    {
        HealthBar.SetHealth(0);
        _Animator.SetBool("IsDead", true);
    }
}
