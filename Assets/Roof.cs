using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Roof : MonoBehaviour 
{
    public float fadeSpeed = 5.0f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name.Equals("Player"))
        {
            //Debug.Log("herro");
            StartCoroutine(FadeOut());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Equals("Player"))
        {
            //Debug.Log("bai");
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();

        while (rend.color.a < 1.0f)
        {
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, rend.color.a + fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();

        while (rend.color.a > 0.0f)
        {
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, rend.color.a - fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
