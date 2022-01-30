using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralBehaviour : MonoBehaviour
{
    public int MaxIterations = 10;
    public int Iteration = 0;
    public float DistanceThreshold = 2.0f;
    public Vector3 OriginalPosition;

    private GameObject _player;
    private bool _isAnimating = false;
    private bool _isAnimatingIn = true;     // State 1 - Spiral closed
    private bool _isAnimatingOut = false;   // State 2 - Spiral opened

    void Start()
    {
        _player = GameObject.Find("Player");
        
        if (this.Iteration == 0)
        {
            OriginalPosition = transform.position;
            InitAnimation();
        }
    }

    void Update()
    {
        if (this.Iteration != 0)
        {
            return;
        }

        // Only the root object

        // Check if finished animating
        bool isFinishedAnimating = true;
        if (_isAnimating)
        {
            foreach(Transform child in transform)
            {
                SpiralBlock spiralBlock = child.GetComponent<SpiralBlock>();
                if (spiralBlock.IsAnimating)
                {
                    isFinishedAnimating = false;
                    break;
                }
            }
        }

        // If not currently animating
        if (isFinishedAnimating)
        {
            _isAnimating = false;

            Vector2 myPosition = new Vector2(this.transform.position.x, this.transform.position.z);
            Vector2 playerPosition = new Vector2(_player.transform.position.x, _player.transform.position.z);

            if (Vector2.Distance(myPosition, playerPosition) <= DistanceThreshold)
            {
                if (_isAnimatingIn)
                {
                    AnimateOut();
                }
            }
            else
            {
                if (_isAnimatingOut)
                {
                    AnimateIn();
                }
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
            
            newGameObject.GetComponent<Bob>().enabled = false;

            newGameObject.transform.position = this.transform.position;

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

    // Spiral in
    private void AnimateIn()
    {
        //Debug.Log("IN");

        if (Iteration == 0)
        {
            AudioClip clip = (AudioClip) Resources.Load("spiralClose");
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();
        }

        _isAnimating = true;
        _isAnimatingIn = true;
        _isAnimatingOut = false;

        int currentIteration = 0;

        foreach(Transform child in transform)
        {
            SpiralBlock spiralBlock = child.GetComponent<SpiralBlock>();
            spiralBlock.Reset();
            spiralBlock.AnimationLength = (MaxIterations - currentIteration) * 0.2f;   // 10 - 1
            spiralBlock.DestinationPosition = Vector3.zero;
            spiralBlock.DestinationScale = Vector3.zero;
            spiralBlock.DestinationEulerAngles = Vector3.zero;

            currentIteration++;
        }
    }

    // Spiral out
    private void AnimateOut()
    {
        //Debug.Log("OUT");

        if (Iteration == 0)
        {
            AudioClip clip = (AudioClip) Resources.Load("spiralOpen");
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();
        }

        _isAnimating = true;
        _isAnimatingIn = false;
        _isAnimatingOut = true;

        int currentIteration = 0;

        foreach(Transform child in transform)
        {
            SpiralBlock spiralBlock = child.GetComponent<SpiralBlock>();
            spiralBlock.Reset();

            float radius = (MaxIterations - currentIteration) * 0.25f;   // 10, 9, 8, ...
            float rad = (MaxIterations - currentIteration) * 20 * Mathf.Deg2Rad;
            Vector3 newPosition = new Vector3(0,
                Mathf.Cos(rad) * radius + OriginalPosition.y + 0.75f,
                Mathf.Sin(rad) * radius + this.transform.position.z - 2.2f);
            
            spiralBlock.AnimationLength = (currentIteration + 1) * 0.4f;   // 1 - 10
            spiralBlock.transform.position = OriginalPosition;   // reset position
            spiralBlock.DestinationPosition = newPosition;
            spiralBlock.DestinationScale = this.transform.localScale * (MaxIterations - currentIteration) * 0.1f;
            spiralBlock.DestinationEulerAngles = this.transform.eulerAngles + new Vector3(-20 * currentIteration, 0, 0);

            currentIteration++;
        }
    }
}
