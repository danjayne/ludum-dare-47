using Assets.Scripts;
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

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _currentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        _animator.SetTrigger("Hurt");

        if (_currentHealth <= 0)
            Die();
        else
            PlayHurtSound(gameObject);
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        _animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    /// <summary>
    /// Currently duplicate of DestroyOnExit.PlayDeathSound
    /// </summary>
    /// <param name="destroyThis"></param>
    private static void PlayHurtSound(GameObject destroyThis)
    {
        switch (Utils.PrefabName(destroyThis))
        {
            case "Cauldron":
                AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.Cauldron);
                break;
            case "Frankensteins-monster":
                AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.FrankensteinHurt);
                break;
            case "Ghost":
                AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.GhostHurt);
                break;
            case "Spider":
                AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.SpiderHurt);
                break;
            case "Witch":
                AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.WitchHurt);
                break;
        }
    }
}
