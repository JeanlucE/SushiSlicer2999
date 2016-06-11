using UnityEngine;
using System.Collections;

public class TentakelAnimation : MonoBehaviour {

    public Renderer tentakel;
    public Texture[] textures;

    public float unravelOffset;
    public float swapTime;

    private Material material;
    private Coroutine cr, cr2;

	void Start ()
    {
        material = tentakel.material;
        cr = StartCoroutine(Animate());
        cr2 = StartCoroutine(Unravel());
	}

    void OnDisable()
    {
        StopCoroutine(cr);
        StopCoroutine(cr2);
    }

    IEnumerator Unravel()
    {
        float speed = -9 / 4f;
        float pos = unravelOffset;
        
        while (this && pos > 0)
        {
            yield return new WaitForEndOfFrame();

            pos += speed * Time.deltaTime;
            material.SetFloat("_Clip", pos);
        }
    }

    IEnumerator Animate()
    {
        if (textures.Length > 0)
        {
            int index = Random.Range(0, textures.Length);

            yield return new WaitForSeconds(Random.Range(0, swapTime));
            while (this)
            {
                material.mainTexture = textures[index];
                index = (index + 1) % textures.Length;

                yield return new WaitForSeconds(swapTime);
            }
        }
    }

}
