using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    public const float DebounceTime = 0.1f;
    public PlayerController[] Players;

    public Vector2 moveVal;
    public float moveSpeed;

    private bool _debouncing = false;
    private float _debounceRealtime = 0.0f;

    protected void Start() {
        Players = FindObjectsOfType<PlayerController>();
    }

    protected void Update() {
        ProcessDebouncing();
    }

    public void Move(InputAction.CallbackContext value) {
        if (MenuSystem.Instance.IsInMenu) {
            if (value.started) {
                MenuSystem.Instance.ProcessInput(mainAxis: value.ReadValue<Vector2>());
            }
            return;
        }
        moveVal = value.ReadValue<Vector2>();
        // Debug.Log(moveVal);
        foreach (var player in Players) {
            player.RequestedMoveDelta = moveVal;
        }
    }

    public void Submit(InputAction.CallbackContext value) {
        if (_debouncing || !value.started) {
            return;
        }
        bool isDown = value.ReadValue<float>() > 0.5f;
        if (!isDown) {
            return;
        }
        StartDebouncing();
        if (MenuSystem.Instance.WantsInput) {
            MenuSystem.Instance.ProcessInput(accept: true);
            return;
        }
        foreach (var player in Players) {
            player.RequestAccept();
        }
    }

    public void Cancel(InputAction.CallbackContext value) {
        if (_debouncing || !value.started) {
            return;
        }
        bool isDown = value.ReadValueAsButton();
        if (!isDown) {
            return;
        }
        StartDebouncing();
        if (MenuSystem.Instance.WantsInput) {
            MenuSystem.Instance.ProcessInput(cancel: true);
            return;
        }
        foreach (var player in Players) {
            player.RequestCancel();
        }
    }

    private void ProcessDebouncing() {
        if (!_debouncing) {
            return;
        }
        if (Time.realtimeSinceStartup - _debounceRealtime > DebounceTime) {
            _debouncing = false;
        }
    }

    private void StartDebouncing() {
        _debouncing = true;
        _debounceRealtime = Time.realtimeSinceStartup;
    }
}
