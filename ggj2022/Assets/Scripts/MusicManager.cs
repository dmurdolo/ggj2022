using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource[] _audioSources;

    // Test
    public GameObject Player;
    public GameObject GoodObject;
    public GameObject BadObject;

    void Start()
    {
        // mid, good, bad
        _audioSources = GetComponents<AudioSource>();
        //Debug.Log(_audioSources.Length);

        for (int i = 0; i < _audioSources.Length; i++)
        {
            //Debug.Log(_audioSources[i].clip.length);
        }
    }

    void Update()
    {
        float goodDist = Vector3.Distance(Player.transform.position, GoodObject.transform.position) - 1.5f;
        float goodVolume = 1 - goodDist;
        _audioSources[1].volume = goodVolume;

        float badDist = Vector3.Distance(Player.transform.position, BadObject.transform.position) - 1.5f;
        float badVolume = 1 - badDist;
        _audioSources[2].volume = badVolume;

        //Debug.Log((_audioSources[1] as AudioSource).clip.name + ": " + goodDist + " " + goodVolume);
        //Debug.Log(goodVolume + " / " + badVolume);
    }
}
