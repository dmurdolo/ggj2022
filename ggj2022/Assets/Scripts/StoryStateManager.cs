using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryStateManager : MonoBehaviour {
    private class ListenerData {
        public Component Component;
        public Action Callback;
    }

    public List<string> States = new List<string>();
    private HashSet<string> _statesSet = new HashSet<string>();

    private Dictionary<string, List<ListenerData>> _listeners = new Dictionary<string, List<ListenerData>>();

    public void AddListener(Component component, string state, Action callback) {
        if (!_listeners.TryGetValue(state, out List<ListenerData> listeners)) {
            listeners = new List<ListenerData>();
            _listeners[state] = listeners;
        }
        listeners.Add(new ListenerData { Component = component, Callback = callback });
    }

    public void RemoveListener(Component component, string state) {
        if (_listeners.TryGetValue(state, out List<ListenerData> listeners)) {
            listeners.RemoveAll(listener => listener.Component == component);
        }
    }

    protected void OnValidate() {
        IndexStates();
    }

    protected void OnEnabled() {
        IndexStates();
    }

    private void IndexStates() {
        _statesSet.Clear();
        foreach (string state in States) {
            _statesSet.Add(state);
        }
    }

    public bool HasState(string state) {
        return _statesSet.Contains(state);
    }

    public void AddState(string state) {
        if (_statesSet.Contains(state)) {
            return;
        }
        States.Add(state);
        _statesSet.Add(state);
        if (_listeners.TryGetValue(state, out List<ListenerData> listeners)) {
            foreach (var listener in listeners) {
                if (listener.Component) {
                    listener.Callback();
                }
            }
        }
    }

    public void RemoveState(string state) {
        if (!_statesSet.Contains(state)) {
            return;
        }
        States.Remove(state);
        _statesSet.Remove(state);
    }
}
