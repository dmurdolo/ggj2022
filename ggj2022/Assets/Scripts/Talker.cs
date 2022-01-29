using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : MonoBehaviour {
    public Conversation Conversation;

    public bool DebugStart = false;
    public bool IsSelectedByPlayer = false;

    public void RequestStartConversation() {
        FindObjectOfType<MenuSystem>().StartConversation(Conversation);
    }

    protected void Update() {
        if (DebugStart) {
            DebugStart = false;
            FindObjectOfType<MenuSystem>().StartConversation(Conversation);
        }
    }

    private void OnTriggerEnter(Collider other) {
        PlayerController player = other.gameObject.GetComponentInParent<PlayerController>();
        if (!player) {
            return;
        }
        player.AddTalker(this);
    }

    private void OnTriggerExit(Collider other) {
        PlayerController player = other.gameObject.GetComponentInParent<PlayerController>();
        if (!player) {
            return;
        }
        player.RemoveTalker(this);
    }

    protected void OnDrawGizmos() {
        if (IsSelectedByPlayer) {
            Vector3 top = transform.position;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(top, top + Vector3.up * 2);
        }
    }
}
