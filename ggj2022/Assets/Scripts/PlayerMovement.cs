using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;

    public Vector2 moveVal;
    public float moveSpeed;

    public void Move(InputAction.CallbackContext value)
    {
        moveVal = value.ReadValue<Vector2>();
        _player.transform.Translate(new Vector3(Mathf.Ceil(moveVal.x), 0, Mathf.Ceil(moveVal.y)) * moveSpeed);
    }
}
