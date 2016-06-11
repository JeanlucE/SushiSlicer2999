using UnityEngine;
using System.Collections;

public class FadeOutMesh : MonoBehaviour {

    [Range(0.01f, 5.0f)]
    public float fadeOutTime = 1.0f;

    public float fadeOutOffset = 0.0f;

    private float startTime;
    private MeshRenderer mr;

    void Start()
    {
        this.startTime = Time.time + fadeOutOffset;
        this.mr = GetComponent<MeshRenderer>();
    }

	void Update ()
    {
        float alpha = Mathf.Clamp01(1.0f - (Time.time - startTime) / fadeOutTime);
        mr.material.SetFloat("_Fade", alpha);
	}

}
