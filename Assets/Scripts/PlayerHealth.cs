using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    #region "Singleton"

    private static PlayerHealth _instance;

    public static PlayerHealth Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

    }

    #endregion

    public int MaxHealth = 100;
    public HealthBar HealthBar;
    public LayerMask HurtBy;
    public int HurtByCollisionDamage = 10;
    public int CurrentHealth = 0;
    public bool IsDead;

    private float _TimeSinceLastHurt;
    private float _DelayBeforeNextHurt = 5f;
    private Animator _Animator;

    private void Start()
    {
        _Animator = GetComponent<Animator>();
        CurrentHealth = MaxHealth;
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
            AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.ArthurHurtByFire, 0.8f);
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
        CameraManager.Instance.PlayDeathCameraScene();
        AudioManager.Instance.PlayMusic(MusicEnum.WitchMusic);
        AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.ArthurHurt, 3f);
        _Animator.SetBool("IsDead", true);

        IsDead = true;

        Invoke("Restart", 10f);
    }

    private void Restart()
    {
        CameraManager.Instance.ResetCamera();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
