using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class mAudioFile
{
    public string name;
    public AudioClip clip;
    [Range (0,1)]
    public float volume;

    public mAudioFile(string fl)
    {
        name = fl; volume = 1.0f;
        clip = Resources.Load<AudioClip>("Sound/" + fl);
    }

}
