using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour {
    public float VerticalBias = 1.0f;
    public float CameraDistance = 10.0f;

    public Camera Camera;
    public PlayerController Player;

    void Start() {
        Camera = GetComponent<Camera>();
        Player = FindObjectOfType<PlayerController>();
    }

    void LateUpdate() {
        Vector3 cameraDirection = Camera.transform.forward;
        Vector3 cameraTarget = Player.transform.position + Vector3.up * VerticalBias;
        Camera.transform.position = cameraTarget - CameraDistance * cameraDirection;
    }
}
