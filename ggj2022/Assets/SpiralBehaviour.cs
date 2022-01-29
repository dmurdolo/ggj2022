using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralBehaviour : MonoBehaviour
{
    public int MaxIterations = 10;
    public int Iteration = 0;
    public float DistanceThreshold = 2.0f;
    public Vector3 Origin;

    private GameObject _player;
    private bool _isInitiated = false;
    private bool _isAnimating = false;


    void Start()
    {
        _player = GameObject.Find("Player");
    }

    void Update()
    {
        // Only the root
        if (this.Iteration == 0)
        {
            Vector2 myPosition = new Vector2(this.transform.position.x, this.transform.position.z);
            Vector2 playerPosition = new Vector2(_player.transform.position.x, _player.transform.position.z);

            if (Vector2.Distance(myPosition, playerPosition) <= DistanceThreshold)
            {
                if (!this._isInitiated)
                {
                    Debug.Log("IN");
                    _isInitiated = true;
                    InitAnimation();
                    AnimateIn();
                }
                else
                {
                    if (!this._isAnimating)
                    {
                        //AnimateIn();
                    }
                }
            }
            else
            {
                Debug.Log("OUT");
                AnimateOut();
            }
        }
    }

    private void InitAnimation()
    {
        List<GameObject> newGameObjects = new List<GameObject>();

        for (int i = 0; i < MaxIterations; i++)
        {
            int currentIteration = i + 1;

            GameObject newGameObject = Instantiate(this.gameObject);
            newGameObject.transform.position = this.transform.position;
            newGameObject.transform.localScale *= (MaxIterations - currentIteration) * 0.1f;
            newGameObject.transform.eulerAngles = this.transform.eulerAngles + new Vector3(-20 * currentIteration, 0, 0);

            newGameObject.GetComponent<Bob>().enabled = false;

            //
            SpiralBlock spiralBlock = newGameObject.AddComponent<SpiralBlock>() as SpiralBlock;
            
            //
            SpiralBehaviour spiralBehaviour = newGameObject.GetComponent<SpiralBehaviour>();
            spiralBehaviour.Iteration = currentIteration;

            newGameObjects.Add(newGameObject);
        }

        foreach(GameObject obj in newGameObjects)
        {
            obj.transform.parent = this.transform;
        }
    }

    private void AnimateIn()
    {
        this._isAnimating = true;

        int currentIteration = 0;

        foreach(Transform child in transform)
        {
            SpiralBlock spiralBlock = child.GetComponent<SpiralBlock>();
            spiralBlock.Reset();

            float radius = (MaxIterations - currentIteration) * 0.25f;   // 10, 9, 8, ...
            float rad = (MaxIterations - currentIteration) * 20 * Mathf.Deg2Rad;
            Vector3 newPosition = new Vector3(0,
                Mathf.Cos(rad) * radius + this.transform.position.y + 2.5f,
                Mathf.Sin(rad) * radius + this.transform.position.z + 2.0f);
            spiralBlock.Destination = newPosition;
            spiralBlock.Speed = (MaxIterations - currentIteration) * 0.25f;

            currentIteration++;
        }
    }

    private void AnimateOut()
    {
        int currentIteration = 0;

        foreach(Transform child in transform)
        {
            SpiralBlock spiralBlock = child.GetComponent<SpiralBlock>();
            spiralBlock.Reset();
            spiralBlock.Destination = this.transform.position;
            spiralBlock.Speed = currentIteration * 0.5f;

            currentIteration++;
        }
    }
}
