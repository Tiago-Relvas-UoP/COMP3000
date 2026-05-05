using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// This script is responsible for creating temporary AudioSource components to play different sound effects (E.g. Footsteps), and destroys those components after the sound effect is concluded
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer sfxMix;

    private void Awake()
    {
        instance = this;
    }

    // This method is called externally in other scripts to play out a sound effect. Multiple parameters are in place to edit how the SFX plays out.
    public void PlaySFX(AudioClip audioClip, float volume = 1f, float spatialBlend = 1f, float minDist = 1f, float maxDist = 500f, bool muteAfterTime = false, float timeToMute = 9999f) 
    {
        StartCoroutine(PlaySFXCoroutine(audioClip, volume, spatialBlend, minDist, maxDist, muteAfterTime, timeToMute));
    }

    // Dynamic Enumator for each SFX that plays. Destroys the sound effect after it is done playing.
    IEnumerator PlaySFXCoroutine(AudioClip audioClip, float volume = 1f, float spatialBlend = 1f, float minDist = 1f, float maxDist = 500f, bool muteAfterTime = false, float timeToMute = 9999f)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxMix.FindMatchingGroups("SFX")[0];
        audioSource.dopplerLevel = 0f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.spatialBlend = spatialBlend;
        audioSource.minDistance = minDist;
        audioSource.maxDistance = maxDist;
        audioSource.Play();

        if (muteAfterTime) yield return new WaitForSeconds(timeToMute);
        else yield return new WaitForSeconds(audioSource.clip.length * 2);
        // yield return new WaitForSeconds(audioSource.clip.length * 2);

        Destroy(audioSource);
    }
}
