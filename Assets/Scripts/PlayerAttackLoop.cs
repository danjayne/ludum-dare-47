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

    float horizontalMove = 0f;
    bool jump = false;
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
        ElapsedMsText.text = $"Elapsed: {_elapsedMs.ToString()}";

        // During update, can't Slash, Stab, Dash or Jump using Input controls
        // Can only move left and right

        if (Input.GetKeyDown(KeyCode.Backspace) && !_isInitializingActions && !_allPlayerActions.Any())
        {
            _isInitializingActions = true;
        }

        if (_isInitializingActions)
        {
            AddPlayerActions();
        }
        else if (_actionRecordingComplete && _actionsActive)
        {
            ReplayPlayerActions();
        }

        ReadNormalPlayerInput();
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
        //Debug.Log("Normal player input");

        var runSpeed = crouch ? RunSpeed : RunSpeed;

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        Animator.SetFloat("MovementSpeed", Math.Abs(horizontalMove));

        if (!_actionsActive && Input.GetButtonDown("Jump"))
        {
            Animator.SetBool("IsJumping", true);
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    // Update is called once per frame
    void ReplayPlayerActions()
    {
        //Debug.Log("Replay player actions");

        //ActionText.text += $"Currently {_remainingActionsInCurrentLoop.Count}{nameof(_remainingActionsInCurrentLoop)}";

        // loop through player actions
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
            case PlayerActionTypeEnum.Jump:
                jump = true;
                break;
            default:
                //Debug.LogWarning($"{currentAction.PlayerActionType} replayed at {currentAction.TimeActionPerformed}");
                break;
            //case PlayerActionTypeEnum.Dash:
            //    break;
            //case PlayerActionTypeEnum.Slash:
            //    break;
            //case PlayerActionTypeEnum.Stab:
            //    break;
        }

        _remainingActionsInCurrentLoop.Remove(currentAction);

        //ActionText.text += $"Currently {_remainingActionsInCurrentLoop}{nameof(_remainingActionsInCurrentLoop)}";
    }

    private void ResetPlayerActionLoop()
    {
        _elapsedMs = 0f;
        ElapsedMsText.text = $"Elapsed: {_elapsedMs.ToString()}";
        ActionText.text = $"ACTIONS: {_allPlayerActions.Count()}{Environment.NewLine}REPLAYING:{Environment.NewLine}";
        _remainingActionsInCurrentLoop = new List<PlayerAction>(_allPlayerActions);

    }

    void FixedUpdate()
    {
        // Move our character
        CController.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var jump = new PlayerAction { PlayerActionType = PlayerActionTypeEnum.Jump, TimeActionPerformed = _elapsedMs };
            _allPlayerActions.Add(jump);

            ActionText.text += $"{jump.PlayerActionType.ToString()}: {jump.TimeActionPerformed}{Environment.NewLine}";
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
    Dash,
    Jump
}