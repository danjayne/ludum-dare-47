using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using UnityEngineInternal;

public class PlayerInput : MonoBehaviour
{
    public CharacterController2D CController;
    public Animator Animator;
    public float RunSpeed = 40f;
    public int SlashDamage = 20;
    public int StabDamage = 40;
    public Transform AttackPoint;
    public float AttackRange = 0.5f;
    public LayerMask EnemyLayers;

    float horizontalMove = 0f;
    bool jump = false;
    bool dash = false;
    bool crouch = false;

    private void Start()
    {
        Animator.SetBool("IsCrouching", false);
    }

    private void Update()
    {
        if (PlayerHealth.Instance.IsDead)
            this.enabled = false;

        // HACK: Fix this
        float worldBottom = -100f;
        if (transform.position.y < worldBottom)
            PlayerHealth.Instance.TakeDamage(999);

        ReadPlayerInputs();
    }

    void FixedUpdate()
    {
        // Move our character
        CController.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, dash);
        jump = false;
        dash = false;
    }

    public void HelmetShut()
    {
        AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.ArthurHelmetClosing);
    }

    private void ReadPlayerInputs()
    {
        var runSpeed = crouch ? RunSpeed : RunSpeed;

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        Animator.SetFloat("MovementSpeed", Math.Abs(horizontalMove));

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Slash();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Stab();
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }
    }

    public void OnLanding()
    {
        Animator.SetBool("IsJumping", false);
    }

    public void OnDashing()
    {
        Animator.SetTrigger("Dash");
    }

    public void OnCrouching(bool crouching)
    {
        Animator.SetBool("IsCrouching", crouching);
    }

    private void Jump()
    {
        Animator.SetBool("IsJumping", true);
        jump = true;
    }

    private void Dash()
    {
        dash = true;
    }

    private void Stab()
    {
        AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.Stab);
        Animator.SetTrigger("Attack2");
        var hits = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);

        foreach (var enemy in hits)
        {
            enemy.GetComponent<StandardEnemy>().TakeDamage(StabDamage);
        }
    }

    private void Slash()
    {
        AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.Slash1);
        Animator.SetTrigger("Attack1");
        var hits = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);

        foreach (var enemy in hits)
        {
            enemy.GetComponent<StandardEnemy>().TakeDamage(SlashDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null)
            return;

        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}