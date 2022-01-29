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
        headLine.widthMultiplier = headLineMultiplier;
        lightLine.widthMultiplier = lightLineMultiplier;
        darkLine.widthMultiplier = darkLineMultiplier;
    }
}
