using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource[] _audioSources;
    private GameObject _player;

    void Start()
    {
        _player = GameObject.Find("Player");

        // mid, good, bad
        _audioSources = GetComponents<AudioSource>();

        /*
        for (int i = 0; i < _audioSources.Length; i++)
        {
            Debug.Log(_audioSources[i].clip.length);
        }
        */
    }

    void Update()
    {
        // Mid always plays at volume 1
        // z:  60 to  14 = good fade
        // z:  14 to -14 = only midZ
        // z: -14 to -60 = bad fade

        float playerZ = _player.transform.position.z;
        float goodVolume = 0;
        float badVolume = 0;

        if (playerZ >= 14)
        {
            // Good lands
            goodVolume = playerZ > 60.0f ? 1.0f : MathUtils.map(playerZ, 14, 60, 0, 1);
        }
        else if (playerZ <= -14)
        {
            // Bad lands
            badVolume = playerZ < -60.0f ? 10.0f : MathUtils.map(playerZ, -14, -60, 0, 1);
        }

        _audioSources[1].volume = goodVolume;
        _audioSources[2].volume = badVolume;
    }
}
