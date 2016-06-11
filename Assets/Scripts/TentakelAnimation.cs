using UnityEngine;
using System.Collections;

public class TentakelAnimation : MonoBehaviour {

    public Renderer[] renderers;
    public float swapTime;

    Coroutine cr;

	void Start ()
    {
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }

        cr = StartCoroutine(Animate());
	}

    void OnDisable()
    {
        StopCoroutine(cr);
    }

    IEnumerator Animate()
    {
        int lastIndex = renderers.Length - 1;
        int index = 0;

        while (this)
        {
            renderers[lastIndex].enabled = false;
            renderers[index].enabled = true;

            lastIndex = index;
            index = (index + 1) % renderers.Length;

            yield return new WaitForSeconds(swapTime);
        }
    }

}
