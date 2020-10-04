using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int MaxHealth = 100;
    public HealthBar HealthBar;
    public LayerMask HurtBy;
    public int HurtByCollisionDamage = 10;

    private float _TimeSinceLastHurt;
    private float _DelayBeforeNextHurt = 5f;
    private int _CurrentHealth = 0;
    private Animator _Animator;

    private void Awake()
    {
        _Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _CurrentHealth = MaxHealth;
        HealthBar.SetMaxHealth(MaxHealth);
    }

    private void Update()
    {
        _TimeSinceLastHurt += Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        if (CollidedWithObjectPlayerIsHurtBy(c) && _TimeSinceLastHurt >= _DelayBeforeNextHurt)
        {
            // We've entered the fire/spike etc
            TakeDamage(HurtByCollisionDamage);
            _TimeSinceLastHurt = 0f;
        }
    }

    private bool CollidedWithObjectPlayerIsHurtBy(Collision2D c)
    {
        //https://answers.unity.com/questions/454494/how-do-i-use-layermask-to-check-collision.html
        return (HurtBy.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer;
    }

    public void TakeDamage(int damage)
    {
        _CurrentHealth -= damage;

        _Animator.SetTrigger("Hurt");

        if (_CurrentHealth <= 0)
        {
            Die(); // or game over
        }
        else
        {
            HealthBar.SetHealth(_CurrentHealth);
        }
    }

    private void Die()
    {
        HealthBar.SetHealth(0);
        _Animator.SetBool("IsDead", true);
    }
}
