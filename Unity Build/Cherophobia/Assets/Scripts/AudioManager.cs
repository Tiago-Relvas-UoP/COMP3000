using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public void PlaySFX(AudioClip audioClip, float volume = 1f, float spatialBlend = 1f, float minDist = 1f, float maxDist = 500f, bool muteAfterTime = false, float timeToMute = 9999f) 
    {
        StartCoroutine(PlaySFXCoroutine(audioClip, volume, spatialBlend, minDist, maxDist, muteAfterTime, timeToMute));
    }

    // Update is called once per frame
    IEnumerator PlaySFXCoroutine(AudioClip audioClip, float volume = 1f, float spatialBlend = 1f, float minDist = 1f, float maxDist = 500f, bool muteAfterTime = false, float timeToMute = 9999f)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
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
