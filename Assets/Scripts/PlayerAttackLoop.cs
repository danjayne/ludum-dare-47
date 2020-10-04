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
    static bool _isInitializingActions;

    public Text ElapsedMsText;
    public Text ActionText;
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
    float _elapsedMs;
    List<PlayerAction> _allPlayerActions = new List<PlayerAction>();
    List<PlayerAction> _remainingActionsInCurrentLoop = new List<PlayerAction>();
    float _totalActionTime = 10f;
    bool _actionRecordingComplete;
    bool _actionsActive;

    private void Start()
    {
        Animator.SetBool("IsCrouching", false);

        ElapsedMsText.text = $"Elapsed: {_elapsedMs.ToString()}";
        ActionText.text = $"ACTIONS:{Environment.NewLine}";
    }

    private void Update()
    {
        _elapsedMs += Time.deltaTime;

        // During update, can't Slash, Stab or Dash using Input controls
        // Can only move left and right

        if (Input.GetKeyDown(KeyCode.Backspace) && !_isInitializingActions && !_allPlayerActions.Any())
        {
            _elapsedMs = 0f;
            _isInitializingActions = true;
        }

        if (_isInitializingActions)
        {
            ElapsedMsText.text = $"RECORDING...{Environment.NewLine}Elapsed: {_elapsedMs.ToString()}";

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
        Animator.SetTrigger("Dash");
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

        if (_elapsedMs < currentAction.TimeActionPerformed)
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
        _elapsedMs = 0f;
        ElapsedMsText.text = $"Elapsed: {_elapsedMs.ToString()}";
        ActionText.text = $"ACTIONS: {_allPlayerActions.Count()}{Environment.NewLine}REPLAYING:{Environment.NewLine}";
        _remainingActionsInCurrentLoop = new List<PlayerAction>(_allPlayerActions);

    }

    private void AddPlayerActions()
    {
        if (_elapsedMs >= _totalActionTime)
        {
            ResetPlayerActionLoop();
            _isInitializingActions = false;
            _actionRecordingComplete = true;
            _actionsActive = true;
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            var slash = new PlayerAction { PlayerActionType = PlayerActionTypeEnum.Slash, TimeActionPerformed = _elapsedMs };
            _allPlayerActions.Add(slash);

            ActionText.text += $"{slash.PlayerActionType.ToString()}: {slash.TimeActionPerformed}{Environment.NewLine}";
        }

        if (Input.GetMouseButtonUp(1))
        {
            var stab = new PlayerAction { PlayerActionType = PlayerActionTypeEnum.Stab, TimeActionPerformed = _elapsedMs };
            _allPlayerActions.Add(stab);

            ActionText.text += $"{stab.PlayerActionType.ToString()}: {stab.TimeActionPerformed}{Environment.NewLine}";
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            var dash = new PlayerAction { PlayerActionType = PlayerActionTypeEnum.Dash, TimeActionPerformed = _elapsedMs };
            _allPlayerActions.Add(dash);

            ActionText.text += $"{dash.PlayerActionType.ToString()}: {dash.TimeActionPerformed}{Environment.NewLine}";
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    var jump = new PlayerAction { PlayerActionType = PlayerActionTypeEnum.Jump, TimeActionPerformed = _elapsedMs };
        //    _allPlayerActions.Add(jump);

        //    ActionText.text += $"{jump.PlayerActionType.ToString()}: {jump.TimeActionPerformed}{Environment.NewLine}";
        //}
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