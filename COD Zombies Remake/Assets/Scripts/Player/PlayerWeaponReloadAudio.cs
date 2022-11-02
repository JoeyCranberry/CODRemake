using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerWeaponReloadAudio : MonoBehaviour
{
    private AudioSource audioSource;

    private List<AudioClip> clips;

    private bool playing;

    private int curClip;
    private float timeUntilNextClip;
    private float delayBetweenClips;
    private float curDelayRemaining;

    [SerializeField]
    private PlayState curState;

    public void Setup(AudioSource _audioSource, List<AudioClip> _clips)
    {
        audioSource = _audioSource;
        audioSource.loop = false;
        clips = _clips;
    }

    private void Update()
    {
        if(playing)
        {
            switch(curState)
            {
                case PlayState.PLAYING_CLIP:
                    timeUntilNextClip -= Time.deltaTime;

                    if (timeUntilNextClip <= 0f)
                    {
                        SetNextClip();

                        curDelayRemaining = delayBetweenClips;
                        curState = PlayState.DELAYING;
                    }
                    break;
                case PlayState.DELAYING:
                    curDelayRemaining -= Time.deltaTime;
                    if(curDelayRemaining <= 0f)
                    {
                        curState = PlayState.PLAYING_CLIP;
                    }
                    break;
            }
            
        }
    }

    private void SetNextClip()
    {
        curClip++;
        if(curClip == clips.Count)
        {
            playing = false;
        }
        else
        {
            audioSource.clip = clips[curClip];
            audioSource.Play();
            timeUntilNextClip = clips[curClip].length;
        }
    }

    public void Play(float reloadTime)
    {
        // Find the delay between playing clips so when the final clip finshes, the weapon will be reloaded fully
        delayBetweenClips = (reloadTime - clips.Sum(c => c.length)) / clips.Count;
        
        curClip = -1;
        curState = PlayState.PLAYING_CLIP;

        SetNextClip();
        playing = true;
    }

    private enum PlayState
    {
        PLAYING_CLIP,
        DELAYING
    }
}
