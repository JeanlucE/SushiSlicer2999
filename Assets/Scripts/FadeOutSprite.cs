using UnityEngine;
using System.Collections;

public class FadeOutSprite : MonoBehaviour {

    [Range(0.01f, 5.0f)]
    public float fadeOutTime = 1.0f;

    private float startTime;
    private SpriteRenderer sr;

    void Start()
    {
        this.startTime = Time.time;
        this.sr = GetComponent<SpriteRenderer>();
    }

	void Update ()
    {
        Color color = sr.color;
        color.a = Mathf.Clamp01(1.0f - (Time.time - startTime) / fadeOutTime);

        sr.color = color;
	}

}
