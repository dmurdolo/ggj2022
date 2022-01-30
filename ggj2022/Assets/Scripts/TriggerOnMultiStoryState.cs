using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOnMultiStoryState : MonoBehaviour {
    public List<string> ListenForStates = new List<string>();

    public void Start() {
        foreach (string state in ListenForStates) {
            FindObjectOfType<StoryStateManager>().AddListener(this, state, Trigger);
        }
    }

    private void Trigger() {
        var storyStateManager = FindObjectOfType<StoryStateManager>();
        foreach (string state in ListenForStates) {
            if (!storyStateManager.HasState(state)) {
                return;
            }
        }
        foreach (var triggerable in GetComponents<ITriggerable>()) {
            triggerable.Trigger();
        }
    }
}

public interface ITriggerable {
    void Trigger();
}
