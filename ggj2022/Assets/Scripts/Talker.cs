
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : MonoBehaviour {
    [Serializable]
    public class Branch {
        public string State;
        public Conversation Conversation;
    }

    public List<Branch> Branches = new List<Branch>();

    public bool AutoTrigger = false;
    public float AutoTriggerDelay = 5.0f;
    public bool DebugStart = false;
    public bool IsSelectedByPlayer = false;

    private bool _autoTriggered = false;
    private float _autoTriggerTime = 0.0f;

    public void RequestStartConversation() {
        StoryStateManager stateManager = FindObjectOfType<StoryStateManager>();
        Conversation conversation = null;
        foreach (Branch branch in Branches) {
            if (stateManager.HasState(branch.State)) {
                conversation = branch.Conversation;
                break;
            }
        }
        if (!conversation) {
            foreach (Branch branch in Branches) {
                if (string.IsNullOrEmpty(branch.State)) {
                    conversation = branch.Conversation;
                    break;
                }
            }
        }
        if (conversation) {
            FindObjectOfType<MenuSystem>().StartConversation(conversation);
            
            Transform bubble = this.transform.Find("Container/SpeechBubble");
            if (bubble) {
                bubble.GetComponent<AudioSource>().Stop();
            }

            AudioSource audioSource = this.GetComponent<AudioSource>();
            if (audioSource)
            {
                this.GetComponent<AudioSource>().Play();
            }
            else
            {
                Debug.Log("No Audio Source");
            }
        }
    }

    protected void Update() {
        if (DebugStart) {
            DebugStart = false;
            RequestStartConversation();
        }

        if (AutoTrigger && !_autoTriggered) {
            if (!_autoTriggered) {
                _autoTriggerTime += Time.deltaTime;
                if (_autoTriggerTime >= AutoTriggerDelay) {
                    _autoTriggered = true;
                    RequestStartConversation();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        PlayerController player = other.gameObject.GetComponentInParent<PlayerController>();
        if (!player) {
            return;
        }
        
        Transform bubble = this.transform.Find("Container/SpeechBubble");
        if (bubble) {
            bubble.GetComponent<AudioSource>().OrNull()?.Play();
            bubble.localScale = new Vector3(200, 200, 200);
            Bob bob = bubble.GetComponent<Bob>();
            if (bob) {
                bob.Speed = 15.0f;
                bob.Distance = 0.1f;
                bob.Offset = 1;
            }
        }

        player.AddTalker(this);
    }

    private void OnTriggerExit(Collider other) {
        PlayerController player = other.gameObject.GetComponentInParent<PlayerController>();
        if (!player) {
            return;
        }

        Transform bubble = this.transform.Find("Container/SpeechBubble");
        bubble.GetComponent<AudioSource>().Stop();
        bubble.localScale = new Vector3(150, 150, 150);
        Bob bob = bubble.GetComponent<Bob>();
        bob.Speed = 1.0f;
        bob.Distance = 0.1f;
        bob.Offset = 0;

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
