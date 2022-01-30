using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionOnTrigger : MonoBehaviour, ITriggerable {
    public bool DebugTrigger = false;

    public string ToSceneName;

    public void Trigger() {
        Debug.LogError(ToSceneName);
        SceneTransitioner transitioner = new GameObject(nameof(SceneTransitioner)).AddComponent<SceneTransitioner>();
        GameObject.DontDestroyOnLoad(transitioner.gameObject);
        transitioner.ToSceneName = ToSceneName;
    }

    public void OnValidate() {
        if (DebugTrigger) {
            DebugTrigger = false;
            Trigger();
        }
    }
}
