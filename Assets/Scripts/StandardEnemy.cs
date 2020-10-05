using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnemy : MonoBehaviour
{
    public int MaxHealth = 100;

    int _currentHealth;
    Animator _animator;
    //AIPath _aiPath;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        //_aiPath = GetComponent<AIPath>();
    }

    private void Start()
    {
        _currentHealth = MaxHealth;
    }

    //private void Update()
    //{
    //    if (_aiPath != null)
    //        RotateEnemyBasedOnAIPath();
    //}

    //private void RotateEnemyBasedOnAIPath()
    //{
    //    if (_aiPath.desiredVelocity.x >= 0.01f)
    //    {
    //        transform.localScale = new Vector3(-1f, 1f, 1f);
    //    }
    //    else if (_aiPath.desiredVelocity.x <= -0.01f)
    //    {
    //        transform.localScale = new Vector3(1f, 1f, 1f);
    //    }
    //}

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        _animator.SetTrigger("Hurt");

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        _animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
