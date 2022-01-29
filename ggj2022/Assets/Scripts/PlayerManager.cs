using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    public float headLineMultiplier;
    public float darkLineMultiplier;
    public float lightLineMultiplier;

    TrailRenderer darkLine;
    TrailRenderer lightLine;
    TrailRenderer headLine;

    private float _lightLevel {
        get { return m_lightLevel; }
        set { m_lightLevel  = value; }
    }
    public float m_lightLevel = 1f;

    private float _darkLevel {
        get { return m_darkLevel; }
        set { m_darkLevel  = value; }
    }
    public float m_darkLevel = 1f;

    // Start is called before the first frame update
    void Start()
    {   
        if (_player) {
            darkLine = _player.transform.Find("DarkTrail").GetComponent<TrailRenderer>();
            lightLine = _player.transform.Find("LightTrail").GetComponent<TrailRenderer>();
            headLine =  _player.transform.Find("HeadTrail").GetComponent<TrailRenderer>();
        }     
    }

    // Update is called once per frame
    void Update()
    {        
        headLine.widthMultiplier = headLineMultiplier * (m_darkLevel + m_lightLevel) / 2;
        lightLine.widthMultiplier = lightLineMultiplier * m_lightLevel > 2 ? 2 : m_lightLevel;
        darkLine.widthMultiplier = darkLineMultiplier * m_darkLevel > 2 ? 2 : m_darkLevel;
    }
}
