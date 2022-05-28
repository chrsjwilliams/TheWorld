using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum SFX {   CLICK, FOOTSTEPS, SHORTOFBREATH, BITE, TRAIN, THUNDER, SNAKE,
                    DOORSLAM, EAT, EVILLAUGHTER, MUMBLE, ROAR, SCREAM, SIREN, SLAM, WHISTLE, 
                    WHOOSH,FLUTE,FLUTEBROKEN,YAWN,CREEPYWHISPER,  KISS, CRY, EXPLOSION, TROMBONE, MMM, ERROR,
                    WHISTLEFLUTE}


public enum BGM { SILENCE, GARDEN, OFFICE, STATION, SPACE }

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

    [Button]
    private void LoadLibrary()
    {
        audioLibrary.Add(SFX.CLICK, Resources.Load<AudioClip>("Resources/sfx/click") as AudioClip);
        audioLibrary.Add(SFX.ERROR, Resources.Load<AudioClip>("sfx/error.mp3"));
        audioLibrary.Add(SFX.BITE, Resources.Load<AudioClip>("sfx/sfx_bite.mp3"));
        audioLibrary.Add(SFX.CRY, Resources.Load<AudioClip>("sfx/sfx_cry.wav"));
        audioLibrary.Add(SFX.CREEPYWHISPER, Resources.Load<AudioClip>("sfx/sfx_creepyWhisper.wav"));
        audioLibrary.Add(SFX.EVILLAUGHTER, Resources.Load<AudioClip>("sfx/sfx_evilLaughter.wav"));
        audioLibrary.Add(SFX.EXPLOSION, Resources.Load<AudioClip>("sfx/sfx_explosion.mp3"));
        audioLibrary.Add(SFX.FOOTSTEPS, Resources.Load<AudioClip>("sfx/sfx_footsteps.mp3"));
        audioLibrary.Add(SFX.FLUTE, Resources.Load<AudioClip>("sfx/sfx_flute.wav"));
        audioLibrary.Add(SFX.FLUTEBROKEN, Resources.Load<AudioClip>("sfx/sfx_fluteBroken.wav"));
        audioLibrary.Add(SFX.KISS, Resources.Load<AudioClip>("sfx/sfx_kiss.wav"));
        audioLibrary.Add(SFX.MUMBLE, Resources.Load<AudioClip>("sfx/sfx_mumble.wav"));
        audioLibrary.Add(SFX.MMM, Resources.Load<AudioClip>("sfx/sfx_mmm.wav"));
        audioLibrary.Add(SFX.ROAR, Resources.Load<AudioClip>("sfx/sfx_roar.wav"));
        audioLibrary.Add(SFX.SHORTOFBREATH, Resources.Load<AudioClip>("sfx/sfx_shortOfBreath.mp3"));
        audioLibrary.Add(SFX.SCREAM, Resources.Load<AudioClip>("sfx/sfx_scream.mp3"));
        audioLibrary.Add(SFX.SIREN, Resources.Load<AudioClip>("sfx/sfx_siren.mp3"));
        audioLibrary.Add(SFX.SLAM, Resources.Load<AudioClip>("sfx/sfx_slam.wav"));
        audioLibrary.Add(SFX.SNAKE, Resources.Load<AudioClip>("sfx/sfx_snake.mp3"));
        audioLibrary.Add(SFX.WHOOSH, Resources.Load<AudioClip>("sfx/sfx_whoosh.wav"));
        audioLibrary.Add(SFX.YAWN, Resources.Load<AudioClip>("sfx/sfx_yawn.wav"));
        audioLibrary.Add(SFX.WHISTLE, Resources.Load<AudioClip>("sfx/sfx_whistle.wav"));
        audioLibrary.Add(SFX.TROMBONE, Resources.Load<AudioClip>("sfx/sfx_trombone.wav"));


        bgmLibrary.Add(BGM.GARDEN, Resources.Load<AudioClip>("music/bg-garden.mp3"));
        bgmLibrary.Add(BGM.OFFICE, Resources.Load<AudioClip>("music/bg-office.mp3"));


    }

    public void PlayClickSound()
    {
        PlayClip(SFX.CLICK);
    }

    public void PlayErrorSound()
    {
        PlayClip(SFX.ERROR);
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
