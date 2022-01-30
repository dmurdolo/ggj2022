using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMoveOnTrigger : MonoBehaviour, ITriggerable {
    public const float PowerScaling = 2.0f;
    public const float SmoothstepDeadbandScale = 4.0f;

    public bool Triggered = false;
    public Vector3 MoveDistance;
    public float MoveTime = 1.0f;
    public float EaseInOut = 1.0f;

    public float Time;

    public bool DebugReset = false;

    private bool _active;
    private Vector3 _startPosition;

    public void Update() {
        if (!Triggered) {
            enabled = false;
            return;
        }
        if (!_active) {
            _active = true;
            Time = 0.0f;
            _startPosition = transform.position;
        }
        Vector3 targetPosition = _startPosition + MoveDistance;
        Time += UnityEngine.Time.deltaTime;
        float t = MoveTime < 0.01f ? 1.0f : Time / MoveTime;
        if (t >= 1.0f) {
            transform.position = targetPosition;
            enabled = false;
        } else {
            float easeInOut = EaseInOut * PowerScaling;
            float power = easeInOut < 0.0f ? (-easeInOut + 1.0f) : (1.0f / (easeInOut + 1.0f));
            t = Mathf.Pow(t, power);
            float smoothT = t * t * (3 - 2 * t);
            float smoothTAlpha = Mathf.Min(1.0f, Mathf.Abs(smoothT * SmoothstepDeadbandScale));
            t = t * (1.0f - smoothTAlpha) + smoothT * smoothTAlpha;
            transform.position = _startPosition * (1.0f - t) + targetPosition * t;
        }
    }

    public void Trigger() {
        enabled = true;
        Triggered = true;
    }

    public void OnValidate() {
        if (DebugReset) {
            DebugReset = false;
            if (_active) {
                _active = false;
                transform.position = _startPosition;
                if (Triggered) {
                    enabled = true;
                }
            }
        }
    }
}
