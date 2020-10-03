using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class PlayerAttackLoop : MonoBehaviour
{
    static bool _isInitializingActions;

    public Text ElapsedMsText;
    public Text ActionText;

    float _elapsedMs;
    List<PlayerAction> _playerActions = new List<PlayerAction>();
    float _totalActionTime = 20f;
    //PlayerAction[] _actionQueue;
    bool _actionsComplete;
    bool _actionsActive;

    private void Start()
    {
        ElapsedMsText.text = $"Elapsed: {_elapsedMs.ToString()}";
        ActionText.text = $"ACTIONS:{Environment.NewLine}";
    }

    private void Update()
    {
        _elapsedMs = Time.time;
        ElapsedMsText.text = $"Elapsed: {_elapsedMs.ToString()}";

        // During update, can't Slash, Stab, Dash or Jump using Input controls
        // Can only move left and right

        if (Input.GetKeyDown(KeyCode.Backspace) && !_isInitializingActions && !_playerActions.Any())
        {
            _isInitializingActions = true;
        }

        if (_isInitializingActions)
        {
            AddPlayerActions();
        }
        else if (_actionsComplete && _actionsActive)
        {
            ReplayPlayerActions();
        }
        else
        {
            ReadNormalPlayerInput();
        }
    }

    private void ReplayPlayerActions()
    {
        Debug.Log("Replay player actions");

        // loop through player actions
    }

    private void ReadNormalPlayerInput()
    {
        Debug.Log("Normal player input");
    }

    private void AddPlayerActions()
    {
        if (_elapsedMs >= _totalActionTime)
        {
            _elapsedMs = 0f;
            ElapsedMsText.text = $"Elapsed: {_elapsedMs.ToString()}";
            ActionText.text = $"ACTIONS: {_playerActions.Count()}{Environment.NewLine}";
            _isInitializingActions = false;
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            var slash = new PlayerAction { PlayerActionType = PlayerActionTypeEnum.Slash, TimeActionPerformed = _elapsedMs };
            _playerActions.Add(slash);

            ActionText.text += $"{slash.PlayerActionType.ToString()}: {slash.TimeActionPerformed}{Environment.NewLine}";
        }

        if (Input.GetMouseButtonUp(1))
        {
            var stab = new PlayerAction { PlayerActionType = PlayerActionTypeEnum.Stab, TimeActionPerformed = _elapsedMs };
            _playerActions.Add(stab);

            ActionText.text += $"{stab.PlayerActionType.ToString()}: {stab.TimeActionPerformed}{Environment.NewLine}";
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            var dash = new PlayerAction { PlayerActionType = PlayerActionTypeEnum.Dash, TimeActionPerformed = _elapsedMs };
            _playerActions.Add(dash);

            ActionText.text += $"{dash.PlayerActionType.ToString()}: {dash.TimeActionPerformed}{Environment.NewLine}";
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var jump = new PlayerAction { PlayerActionType = PlayerActionTypeEnum.Jump, TimeActionPerformed = _elapsedMs };
            _playerActions.Add(jump);

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