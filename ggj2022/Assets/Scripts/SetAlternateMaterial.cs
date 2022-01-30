using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAlternateMaterial : MonoBehaviour {
    public bool State = false;

    private Dictionary<Renderer, Material> _originalMaterials = new Dictionary<Renderer, Material>();

    private void SetState(bool state) {
        if (state == State) {
            return;
        }
        State = state;
        if (State) {
            _originalMaterials.Clear();
            foreach (var alternate in GetComponentsInChildren<AlternateMaterialComponent>()) {
                var renderer = alternate.GetComponent<Renderer>();
                _originalMaterials[renderer] = renderer.sharedMaterial;
                renderer.sharedMaterial = alternate.NewMaterial;
            }
        } else {
            foreach (var alternate in GetComponentsInChildren<AlternateMaterialComponent>()) {
                var renderer = alternate.GetComponent<Renderer>();
                if (_originalMaterials.TryGetValue(renderer, out var material)) {
                    renderer.sharedMaterial = material;
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other) {
        PlayerController player = other.gameObject.GetComponentInParent<PlayerController>();
        if (!player) {
            return;
        }
        SetState(true);
    }

    public void OnTriggerExit(Collider other) {
        PlayerController player = other.gameObject.GetComponentInParent<PlayerController>();
        if (!player) {
            return;
        }
        SetState(false);
    }
}
