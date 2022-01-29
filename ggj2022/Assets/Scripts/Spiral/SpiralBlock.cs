using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralBlock : MonoBehaviour
{
    public float AnimationLength;
    public Vector3 DestinationPosition;
    public Vector3 DestinationScale;
    public Vector3 DestinationEulerAngles;
        
    public bool IsAnimating = false;

    private float _startTime;
    public Vector3 StartPosition;
    private Vector3 StartScale;
    private Vector3 StartEulerAngles;

    void Start()
    {
    }

    void Update()
    {
        if (this.IsAnimating)
        {
            float timeElapsed = Time.time - _startTime;
            float t = timeElapsed / AnimationLength;

            this.transform.localPosition = Vector3.Lerp(StartPosition, DestinationPosition, t);
            this.transform.localScale = Vector3.Lerp(StartScale, DestinationScale, t);
            this.transform.eulerAngles = Vector3.Lerp(StartEulerAngles, DestinationEulerAngles, t);

            // Stop lerp when time limit is over
            if (timeElapsed >= AnimationLength)
            {
                this.IsAnimating = false;

                this.transform.localPosition = DestinationPosition;
                this.transform.localScale = DestinationScale;
                this.transform.eulerAngles = DestinationEulerAngles;

                Debug.Log("DONE ANIMATING");
            }
        }
    }

    public void Reset()
    {
        _startTime = Time.time;
        IsAnimating = true;

        StartPosition = this.transform.localPosition;
        StartScale = this.transform.localScale;
        StartEulerAngles = this.transform.eulerAngles;
    }
}