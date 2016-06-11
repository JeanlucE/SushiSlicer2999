using UnityEngine;
using System.Collections;

public class FadeOutParticle : MonoBehaviour {

    [Range(0.01f, 5.0f)]
    public float fadeOutTime = 1.0f;
    public bool step = false;

    private float startTime;
    private MeshRenderer mr;

    void Start()
    {
        this.startTime = Time.time;
        this.mr = GetComponent<MeshRenderer>();
    }

	void Update ()
    {
        float alpha = Mathf.Clamp01(1.0f - (Time.time - startTime) / fadeOutTime);

        if (step)
        {
            alpha = alpha > 0 ? 1 : 0;
        }

        mr.material.SetFloat("_Fade", alpha);
    }

}
