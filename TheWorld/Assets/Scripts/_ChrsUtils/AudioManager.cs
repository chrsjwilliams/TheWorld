using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum SFX {   CLICK, FOOTSTEPS, SHORTOFBREATH, BITE, TRAIN, THUNDER, SNAKE,
                    DOORSLAM, EAT, EVILLAUGHTER, MUMBLE, ROAR, SCREAM, SIREN, SLAM}

public enum BGM { SILENCE, GARDEN, OFFICE }

public class AudioManager : SerializedMonoBehaviour
{
    public Dictionary<SFX, AudioClip> audioLibrary;

    public Dictionary<BGM, AudioClip> bgmLibrary;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource bgmAudioSource;
    private AudioClip audioClip;

    private bool fadeAudio = false;

    private TaskManager tm = new TaskManager();
	// Use this for initialization
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
	}

    private void LoadLibrary()
    {
        audioLibrary.Add(SFX.BITE, Resources.Load<AudioClip>("sfx/sfx_bite.mp3"));
        audioLibrary.Add(SFX.EVILLAUGHTER, Resources.Load<AudioClip>("sfx/sfx_evilLaughter.mp3"));
        audioLibrary.Add(SFX.FOOTSTEPS, Resources.Load<AudioClip>("sfx/sfx_footsteps.mp3"));
        audioLibrary.Add(SFX.MUMBLE, Resources.Load<AudioClip>("sfx/sfx_mumble.mp3"));
        audioLibrary.Add(SFX.ROAR, Resources.Load<AudioClip>("sfx/sfx_roar.mp3"));
        audioLibrary.Add(SFX.SHORTOFBREATH, Resources.Load<AudioClip>("sfx/sfx_shortOfBreath.mp3"));
        audioLibrary.Add(SFX.SIREN, Resources.Load<AudioClip>("sfx/sfx_siren.mp3"));
        audioLibrary.Add(SFX.SLAM, Resources.Load<AudioClip>("sfx/sfx_slam.mp3"));


        bgmLibrary.Add(BGM.GARDEN, Resources.Load<AudioClip>("music/bg-garden.mp3"));
        bgmLibrary.Add(BGM.OFFICE, Resources.Load<AudioClip>("music/bg-office.mp3"));


    }

    public void PlayClipVaryPitch(SFX clip)
    {
        float pitch = Random.Range(0.8f, 1.2f);
        PlayClip(clip, 1.0f, pitch);
    }

    public void PlayClip(SFX clip)
    {
        PlayClip(clip, 1.0f, 1.0f);
    }

    public void PlayBGM(BGM bgm)
    {
        if(bgm == BGM.SILENCE)
        {
            bgmAudioSource.Stop();
            return;
        }
        bgmAudioSource.loop = true;
        bgmAudioSource.clip = bgmLibrary[bgm];
        bgmAudioSource.PlayOneShot(bgmLibrary[bgm], 1);
    }

    public void PlayClip(SFX clip, float volume, float pitch)
    {
        audioClip = audioLibrary[clip];
        if(audioClip == null)
        {
            Debug.Log("[WARNING] No audio clip for " + clip.ToString() + " found");
            return;
        }

        audioSource.pitch = pitch;
        audioSource.PlayOneShot(audioClip, volume);
        Debug.Log("Playing Clip " + clip);
    }

    public void StopClip()
    {
        audioSource.Stop();
    }

    public void FadeAudio()
    {
        fadeAudio = true;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    private void Update()
    {
        tm.Update();
        if(fadeAudio)
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0, Time.deltaTime);
            if(audioSource.volume < 0.01f)
            {
                audioSource.Stop();
                audioSource.Stop();
                audioSource.Stop();

                fadeAudio = false;
            }
        }
    }
}
