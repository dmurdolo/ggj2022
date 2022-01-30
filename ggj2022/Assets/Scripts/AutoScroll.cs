using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScroll : MonoBehaviour {
    public float Speed = 1.0f;
    void Update() {
        var transform = GetComponent<RectTransform>();
        var position = transform.position;
        position.y += Speed * Time.deltaTime;
        transform.position = position;
    }
}
