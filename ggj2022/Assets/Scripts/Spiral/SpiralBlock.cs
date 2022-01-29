using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralBlock : MonoBehaviour
{
    public Vector3 Destination;
    public float Speed;
    public bool IsAnimating = false;

    private float _startTime;

    void Start()
    {
        this.Reset();
    }

    void Update()
    {
        if (this.IsAnimating)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, Destination, (Time.time - _startTime) * Speed);

            // Stop lerp when it reaches the destination
            if (this.transform.position == Destination)
            {
                this.IsAnimating = false;
            }
        }
    }

    public void Reset()
    {
        _startTime = Time.time;
        this.IsAnimating = true;
    }
}
