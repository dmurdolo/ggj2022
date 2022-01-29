using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCharacter : MonoBehaviour {
    public GameObject Target;

    public int TriggerBounceCount = 0;
    public bool Breathe = false;

    private bool _wasBreathe = false;
    private float _breatheAcc = 0.0f;

    protected void Update() {
        if (!Target) {
            return;
        }
        
        if (Breathe && !_wasBreathe) {
            _breatheAcc = 0.0f;
        }
        _wasBreathe = Breathe;

        float yMax = 1.0f;
        float yMin = 0.0f;
        if (Breathe) {
            yMax = 1.0f + Mathf.Atan(Mathf.Sin(_breatheAcc)) * 0.2f;
            _breatheAcc += 0.01f;
        }

        Vector3 offset = new Vector3(0.0f, (yMax + yMin) * 0.5f - 0.5f, 0.0f);
        Vector3 scale = new Vector3(1.0f, Mathf.Max(0.0f, yMax - yMin), 1.0f);
        Target.transform.localPosition = offset;
        Target.transform.localScale = scale;
    }
}
