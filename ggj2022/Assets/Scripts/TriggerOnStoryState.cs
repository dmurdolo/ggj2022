using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOnStoryState : MonoBehaviour {
    public string ListenForState = "";

    private string _registeredState = "";

    public void Start() {
        _registeredState = ListenForState;
        FindObjectOfType<StoryStateManager>().AddListener(this, ListenForState, Trigger);
    }

    public void OnDisabled() {
        FindObjectOfType<StoryStateManager>().RemoveListener(this, _registeredState);
    }

    protected void OnValidate() {
        OnDisabled();
    }
    
    private void Trigger() {
        foreach (var triggerable in GetComponents<ITriggerable>()) {
            triggerable.Trigger();
        }
    }
}
