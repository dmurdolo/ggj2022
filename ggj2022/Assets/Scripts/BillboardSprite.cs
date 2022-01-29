using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour {
    private Camera mainCamera;

    protected void Start() {
        mainCamera = FindObjectOfType<Camera>();
    }

    protected void LateUpdate() {
        float rotation = mainCamera.transform.rotation.eulerAngles.y;
        this.transform.rotation = Quaternion.Euler(90, rotation - 180, 0);
    }
}
