using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private List<Level> _levels;

    void Start()
    {
        InitLevels();
    }

    private void InitLevels()
    {
        Level level1 = new Level(20, 20);
        _levels.Add(level1);
    }

    void Update()
    {
    }
}
