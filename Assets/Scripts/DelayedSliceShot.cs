using UnityEngine;
using System.Collections;

public class DelayedSliceShot : MonoBehaviour {

    public float delay;

	void Start () {
        StartCoroutine(Shoot(this.delay));
	}
	
	IEnumerator Shoot(float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 v1 = transform.TransformPoint(-0.5f * Vector3.up);
        Vector3 v2 = transform.TransformPoint(0.5f * Vector3.up);

        SpriteSlicer.SliceAll(v1, v2);

        GetComponent<MeshRenderer>().material.color = Color.red;

        Destroy(gameObject);
    }

}
