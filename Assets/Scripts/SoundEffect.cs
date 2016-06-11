using UnityEngine;
using System.Collections;

public class SoundEffectManager : MonoBehaviour {
    public static SoundEffectManager Instance;

    public void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

	public void CreateSoundEffect(AudioClip clip)
    {
        if (clip == null)
            return;

        GameObject g = new GameObject();
        g.transform.position = transform.position;
        g.name = clip.name;
        AudioSource audioSource = g.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(g, clip.length + 1);
    }
}
