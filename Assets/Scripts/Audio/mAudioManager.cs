using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class mAudioManager : MonoBehaviour
{

    public static mAudioManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private List<mAudioFile> mAudios;

    public AudioSource mMusic;
    public AudioSource mSFX;

    public float mMusicVolume = 1.0f;
    public float mSFXVolume = 1.0f;

    mAudioFile mBGMusic;

    public void Start()
    {
        mAudios = new List<mAudioFile>();

        mMusicVolume = 1.0f;
        mSFXVolume = 1.0f;

        PlayMusic("bso_0" + Random.Range(0, 2).ToString());

        mBGMusic = mAudios[0];

    }

    public void PlayMusic(string soundName, float volume = 1.0f)
    {

        mAudioFile file = GetFileByName(soundName);

        if (file != null)
        {
            file.volume = volume;
            AudioClip clip = file.clip;
            mMusic.volume = file.volume * mSFXVolume;
            if (clip != null)
            {
                mMusic.clip = clip;
                mMusic.PlayOneShot(mMusic.clip);
            }
        }
    }

    public void PlaySFX(string soundName, float volume = 1.0f)
    {

        mAudioFile file = GetFileByName(soundName);

        if (file != null)
        {
            file.volume = volume;
            AudioClip clip = file.clip;
            mSFX.volume = file.volume * mSFXVolume;
            if (clip != null)
            {
                mSFX.clip = clip;
                mSFX.PlayOneShot(mSFX.clip);
            }
        }
    }

    private mAudioFile GetFileByName(string file)
    {

        foreach (mAudioFile audio in mAudios)
        {
            if (audio.name == file) return audio;
        }

        mAudios.Add(new mAudioFile(file));

        return mAudios[mAudios.Count - 1];

    }

}