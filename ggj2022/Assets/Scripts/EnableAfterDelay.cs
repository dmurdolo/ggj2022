using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAfterDelay : MonoBehaviour {
    public Behaviour Component;
    public float Delay = 1.0f;

    public float CurrentTime;

    protected void Update() {
        CurrentTime += Time.deltaTime;
        if (CurrentTime > Delay) {
            enabled = false;
            Component.enabled = true;
        }
    }
}
