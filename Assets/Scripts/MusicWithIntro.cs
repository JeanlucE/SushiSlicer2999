using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicWithIntro : MonoBehaviour 
{
    private AudioSource audioSource;
    public AudioClip intro;
    public AudioClip loop;

	void Start () 
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = intro;
        audioSource.Play();
	}
	

	void Update () 
    {
        if (audioSource.clip == intro && !audioSource.isPlaying)
        {
            audioSource.clip = loop;
            audioSource.loop = true;
            audioSource.Play();
        }
	}
}
