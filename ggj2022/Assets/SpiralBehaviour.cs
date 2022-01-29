using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralBehaviour : MonoBehaviour
{
    public int MaxIterations = 10;
    public int Iteration = 0;

    void Start()
    {
        GameObject gameObject = Instantiate(this.gameObject);
        gameObject.transform.parent = Iteration == 0 ? transform : transform.parent;
        gameObject.transform.localScale *= 0.9f;

        // float rad = deg * Mathf.Deg2Rad;
        float radius = (MaxIterations - Iteration) * 0.25f;   // 10, 9, 8, ...
        float rad = (MaxIterations - Iteration) * 20 * Mathf.Deg2Rad;
        gameObject.transform.localPosition = new Vector3(0,
            Mathf.Cos(rad) * radius + gameObject.transform.parent.position.y + 1.0f,
            Mathf.Sin(rad) * radius + gameObject.transform.parent.position.z / 2.0f);

        gameObject.transform.eulerAngles = gameObject.transform.eulerAngles + new Vector3(-20, 0, 0);

        SpiralBehaviour spiralBehaviour = gameObject.GetComponent<SpiralBehaviour>();
        if (spiralBehaviour.Iteration < MaxIterations)
        {
            spiralBehaviour.Iteration++;
        }
        else
        {
            gameObject.GetComponent<SpiralBehaviour>().enabled = false;
        }
    }

    void Update()
    {
    }
}
