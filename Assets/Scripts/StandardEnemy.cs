﻿using Assets.Scripts;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StandardEnemy : MonoBehaviour
{
    [Header("Health")]
    public int MaxHealth = 100;

    [Header("Death")]
    public Transform OnDeathSpawn;
    public UnityEvent OnDieEvent;

    int _currentHealth;
    Animator _animator;
    const string DEATH_ANIM_NAME = "Spider_Death";

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
            StartCoroutine(nameof(Die));
        else
            PlayHurtSound(false);
    }

    private IEnumerator Die()
    {
        Debug.Log($"{gameObject.name} died.");
        _animator.SetBool("IsDead", true);

        if (OnDieEvent != null)
        {
            OnDieEvent.Invoke();
        }

        transform.parent.GetComponent<Spider>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        var deathAnimationLength = _animator.runtimeAnimatorController.animationClips.FirstOrDefault(a => a.name == DEATH_ANIM_NAME);
        Invoke("SpawnObjectOnDeath", deathAnimationLength?.length ?? 0f);

        this.enabled = false;
        yield return new WaitForSeconds(0f);
    }

    void SpawnObjectOnDeath()
    {
        Instantiate(OnDeathSpawn, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// Currently duplicate of DestroyOnExit.PlayDeathSound
    /// </summary>
    public void PlayHurtSound(bool die)
    {
        switch (gameObject.tag)
        {
            case "Cauldron":
                if (die)
                    AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.SmashCauldron);
                else
                    AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.Cauldron, 1f);
                break;
            case "Frankensteins-monster":
                AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.FrankensteinHurt, .8f);
                break;
            case "Ghost":
                AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.GhostHurt, 1f);
                break;
            case "Spider":
                AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.SpiderHurt, 2f);
                break;
            case "Witch":
                AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.WitchHurt);
                break;
        }
    }
}
