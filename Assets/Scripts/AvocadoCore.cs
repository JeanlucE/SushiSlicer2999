using UnityEngine;
using System.Collections;

public class AvocadoCore : MonoBehaviour {

    private float size;
    public float regrowSpeed = 25.0f;

    private IEnumerator cr;
    public Animator animator;

    void Start()
    {
        size = 1;
    }

    void OnDisable()
    {
        if (cr != null)
        {
            StopCoroutine(cr);
        }
    }

    public void Regrow()
    {
        StartCoroutine(cr = RegrowAnimation());
    }

    IEnumerator RegrowAnimation()
    {
        size = 0;
        transform.localScale = Vector3.zero;
        animator.speed = 0;

        float boost = 0, speed = 0;

        while (size < 1)
        {
            yield return new WaitForEndOfFrame();

            if (boost > 0)
            {
                boost -= Time.deltaTime;
                speed += regrowSpeed * Time.deltaTime;

                size += speed * boost * Time.deltaTime;
                transform.localScale = Vector3.one * size;

                if (Random.Range(0, 1) < 0.2)
                {
                    transform.localRotation = Quaternion.AngleAxis(Random.Range(-3, 3), Vector3.forward);
                }
            }
            else
            {
                transform.localRotation = Quaternion.identity;
                yield return new WaitForSeconds(Random.Range(0.2f, 0.9f));

                boost = Random.Range(0.1f, 0.45f);
                speed = 0.1f;
            }
        }

        transform.localScale = Vector3.one;
        animator.speed = 1;

        yield return null;
    }

    public bool FullyGrown()
    {
        return size >= 1;
    }

}
