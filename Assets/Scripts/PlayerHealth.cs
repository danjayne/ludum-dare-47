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

    [Header("UI")]
    public HealthBar HealthBar;

    [Header("Collisions: Fire")]
    public LayerMask HurtByFireMask;
    public int HurtByFireCollisionDamage = 5;
    private float _TimeSinceLastHurtByFire;
    private float _DelayBeforeNextHurtByFire = 2f;

    [Header("Collision: Enemies")]
    public LayerMask EnemyMask;
    public int HurtByEnemyCollisionDamage = 10;
    private float _TimeSinceLastHurtByEnemy;
    private float _DelayBeforeNextHurtByEnemy = .5f;

    [Header("Health")]
    public int MaxHealth = 100;
    public int CurrentHealth = 0;
    public bool IsDead => CurrentHealth <= 0;

    [Header("Animation")]
    private Animator _Animator;

    private void Start()
    {
        _Animator = GetComponent<Animator>();
        CurrentHealth = MaxHealth;
        HealthBar.SetMaxHealth(MaxHealth);
    }

    private void Update()
    {
        if (IsDead || PauseMenu.GameIsPaused) return;

        _TimeSinceLastHurtByEnemy += Time.deltaTime;
        _TimeSinceLastHurtByFire += Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        if (IsDead || PauseMenu.GameIsPaused) return;

        CheckIfHurtByFire(c.gameObject.layer);
        CheckIfHurtByEnemy(c.gameObject.layer);
    }

    private void CheckIfHurtByEnemy(int layer)
    {
        if (CollidedWithLayer(layer, EnemyMask) && _TimeSinceLastHurtByEnemy >= _DelayBeforeNextHurtByEnemy)
        {
            // We've entered the fire/spike etc
            AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.ArthurHurtByFire, 0.8f);
            TakeDamage(HurtByEnemyCollisionDamage);
            _TimeSinceLastHurtByEnemy = 0f;
        }
    }

    void CheckIfHurtByFire(int hurtByLayer)
    {
        if (CollidedWithLayer(hurtByLayer, HurtByFireMask) && _TimeSinceLastHurtByFire >= _DelayBeforeNextHurtByFire)
        {
            // We've entered the fire/spike etc
            AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.ArthurHurtByFire, 0.8f);
            TakeDamage(HurtByFireCollisionDamage);
            _TimeSinceLastHurtByFire = 0f;
        }
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        if (IsDead) return;

        CheckIfHurtByFire(c.gameObject.layer);
        CheckIfHurtByEnemy(c.gameObject.layer);
    }

    private bool CollidedWithLayer(int layer, LayerMask mask)
    {
        //https://answers.unity.com/questions/454494/how-do-i-use-layermask-to-check-collision.html
        return (mask.value & 1 << layer) == 1 << layer;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        if (IsDead)
        {
            Die(); // or game over
        }
        else
        {
            _Animator.SetTrigger("Hurt");
            HealthBar.SetHealth(CurrentHealth);
        }
    }

    private void Die()
    {
        _Animator.SetTrigger("Death");
        HealthBar.SetHealth(0);

        gameObject.GetComponent<CharacterController2D>().enabled = false;
        gameObject.GetComponent<PlayerInput>().OnLanding();
        gameObject.GetComponent<PlayerInput>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        CameraManager.Instance.PlayDeathCameraScene();
        AudioManager.Instance.PlayMusic(MusicEnum.WitchMusic);
        AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.ArthurHurt, 3f);

        Invoke("Restart", 10f);
    }

    private void Restart()
    {
        CameraManager.Instance.ResetCamera();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
