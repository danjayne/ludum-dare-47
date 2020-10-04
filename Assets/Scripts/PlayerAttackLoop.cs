using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using UnityEngineInternal;

public class PlayerAttackLoop : MonoBehaviour
{
    public bool IsInitializingActions;
    public float _totalActionTime = 10f;
    public float ElapsedMs;
    public List<PlayerAction> _allPlayerActions = new List<PlayerAction>();
    public List<PlayerAction> _remainingActionsInCurrentLoop = new List<PlayerAction>();
    public bool _actionRecordingComplete;
    public bool _actionsActive;
    public Text ElapsedMsText;
    public Text ActionText;
    public CountdownBar CountdownBar;

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

        ElapsedMsText.text = $"Elapsed: {ElapsedMs.ToString()}";
        ActionText.text = $"ACTIONS:{Environment.NewLine}";
    }

    private void Update()
    {
        ElapsedMs += Time.deltaTime;

        // During update, can't Slash, Stab or Dash using Input controls
        // Can only move left and right

        if (Input.GetKeyDown(KeyCode.Backspace) && !IsInitializingActions && !_allPlayerActions.Any())
        {
            ElapsedMs = 0f;
            IsInitializingActions = true;
        }

        if (IsInitializingActions)
        {
            //ElapsedMsText.text = $"RECORDING...{Environment.NewLine}Elapsed: {ElapsedMs.ToString()}";
            CountdownBar.SetTime(ElapsedMs);
            AddPlayerActions();
        }
        else if (_actionRecordingComplete && _actionsActive)
        {
            ReplayPlayerActions();
        }

        ReadNormalPlayerInput();
    }

    void FixedUpdate()
    {
        // Move our character
        CController.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, dash);
        jump = false;
        dash = false;
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

    private void ReadNormalPlayerInput()
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

        if (!_actionsActive)
        {
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
        Animator.SetTrigger("Attack2");
        var hits = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);

        foreach (var enemy in hits)
        {
            enemy.GetComponent<StandardEnemy>().TakeDamage(StabDamage);
        }
    }

    private void Slash()
    {
        Animator.SetTrigger("Attack1");
        var hits =  Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);
    
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

    void ReplayPlayerActions()
    {
        PlayerAction currentAction = _remainingActionsInCurrentLoop.FirstOrDefault();

        if (currentAction == null)
        {
            ResetPlayerActionLoop();
            ActionText.text += "No more actions, resetting loop";
            return;
        }

        if (ElapsedMs < currentAction.TimeActionPerformed)
            return;

        switch (currentAction.PlayerActionType)
        {
            case PlayerActionTypeEnum.Dash:
                Dash();
                break;
            case PlayerActionTypeEnum.Slash:
                Slash();
                break;
            case PlayerActionTypeEnum.Stab:
                Stab();
                break;
            default:
                Debug.LogError($"Can't handle {currentAction.PlayerActionType}");
                break;
        }

        _remainingActionsInCurrentLoop.Remove(currentAction);
    }

    private void ResetPlayerActionLoop()
    {
        ElapsedMs = 0f;
        ElapsedMsText.text = $"Elapsed: {ElapsedMs.ToString()}";
        ActionText.text = $"ACTIONS: {_allPlayerActions.Count()}{Environment.NewLine}REPLAYING:{Environment.NewLine}";
        _remainingActionsInCurrentLoop = new List<PlayerAction>(_allPlayerActions);

    }

    private void AddPlayerActions()
    {
        if (ElapsedMs >= _totalActionTime)
        {
            ResetPlayerActionLoop();
            IsInitializingActions = false;
            _actionRecordingComplete = true;
            _actionsActive = true;
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            var slash = new PlayerAction { PlayerActionType = PlayerActionTypeEnum.Slash, TimeActionPerformed = ElapsedMs };
            _allPlayerActions.Add(slash);
            CountdownBar.AddActionImage(slash);
            ActionText.text += $"{slash.PlayerActionType.ToString()}: {slash.TimeActionPerformed}{Environment.NewLine}";
        }

        if (Input.GetMouseButtonUp(1))
        {
            var stab = new PlayerAction { PlayerActionType = PlayerActionTypeEnum.Stab, TimeActionPerformed = ElapsedMs };
            _allPlayerActions.Add(stab);
            CountdownBar.AddActionImage(stab);
            ActionText.text += $"{stab.PlayerActionType.ToString()}: {stab.TimeActionPerformed}{Environment.NewLine}";
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            var dash = new PlayerAction { PlayerActionType = PlayerActionTypeEnum.Dash, TimeActionPerformed = ElapsedMs };
            _allPlayerActions.Add(dash);
            CountdownBar.AddActionImage(dash);
            ActionText.text += $"{dash.PlayerActionType.ToString()}: {dash.TimeActionPerformed}{Environment.NewLine}";
        }
    }
}

public class PlayerAction
{
    public int UniqueIdentifier { get; set; }
    public float TimeActionPerformed { get; set; }
    public PlayerActionTypeEnum PlayerActionType { get; set; }
}

public enum PlayerActionTypeEnum
{
    Slash,
    Stab,
    Dash
}