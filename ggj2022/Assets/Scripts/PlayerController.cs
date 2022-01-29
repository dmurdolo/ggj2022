using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float MovementForce = 1000.0f;
    public float DragWhenMoving = 10.0f;
    public float DragWhenNotMoving = 50.0f;

    public Rigidbody Body;
    public Camera Camera;
    public Vector2 RequestedMoveDelta = Vector2.zero;

    public List<Talker> TalkersInRange = new List<Talker>();
    public Talker SelectedTalker;

    public void Start() {
        Body = GetComponent<Rigidbody>();
        Camera = FindObjectOfType<Camera>();
    }

    public void AddTalker(Talker talker) {
        Debug.Log($"add: {talker}");
        TalkersInRange.Add(talker);
    }

    public void RemoveTalker(Talker talker) {
        Debug.Log($"remove: {talker}");
        TalkersInRange.Remove(talker);
    }

    public void RequestAccept() {
        if (SelectedTalker) {
            SelectedTalker.RequestStartConversation();
        }
    }

    public void RequestCancel() {
    }

    public void Update() {
        Vector3 position = transform.position;
        float minTalkerDistanceSqr = float.MaxValue;
        Talker minTalker = null;
        foreach (Talker talker in TalkersInRange) {
            float distanceSqr = (talker.transform.position - position).sqrMagnitude;
            if (distanceSqr < minTalkerDistanceSqr) {
                minTalkerDistanceSqr = distanceSqr;
                minTalker = talker;
            }
        }
        if (SelectedTalker != minTalker) {
            if (SelectedTalker) {
                SelectedTalker.IsSelectedByPlayer = false;
            }
            if (minTalker) {
                minTalker.IsSelectedByPlayer = true;
            }
        }
        SelectedTalker = minTalker;
    }

    public void FixedUpdate() {
        Vector2 requested = RequestedMoveDelta;
        Vector3 cameraForward = Camera.transform.forward;
        Vector3 cameraRight = Camera.transform.right;
        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;
        cameraForward.Normalize();
        cameraRight.Normalize();
        Body.AddForce(MovementForce * (cameraForward * requested.y + cameraRight * requested.x), ForceMode.Force);

        bool isMoving = requested.sqrMagnitude > 0.01f;
        Body.drag = isMoving ? DragWhenMoving : DragWhenNotMoving;
    }

}
