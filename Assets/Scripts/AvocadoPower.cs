using UnityEngine;
using System.Collections;

public class AvocadoPower : MonoBehaviour {

    public GameObject projectile;
    public AvocadoCore core;

    public float respawnMin = 0.9f, respawnMax = 10.0f;
    public float accuracyRadius = 0.0f;

    private Texture avocadoTemp;
    private IEnumerator cr;

    void Start()
    {
        StartCoroutine(cr = AvocadoBrain());
	}

    void OnDestroy()
    {
        if (cr != null)
        {
            StopCoroutine(cr);
        }
    }

    IEnumerator AvocadoBrain()
    {
        while (this)
        {
            float waitTime = Random.Range(respawnMin, respawnMax);
            yield return new WaitForSeconds(waitTime);

            if (this && !PlayerController.main.IsKnockedOut)
            {
                Transform player = PlayerController.main.transform;
                Vector3 playerPosition = player.position + accuracyRadius * (Vector3)Random.insideUnitCircle;

                Vector3 rel = playerPosition - transform.position;
                Vector3 vel = PlayerController.main.rb.velocity;

                float bs = 45.0f;

                float fa = 1 / ((vel.x * vel.x + vel.y * vel.y) - bs * bs);
                float fb = fa * 2 * (rel.x * vel.x + rel.y * vel.y);
                float fc = fa * (rel.x * rel.x + rel.y * rel.y);

                Vector3 shootVector = rel;

                float det = fb * fb - 4 * fc;
                if (det >= 0)
                {
                    det = Mathf.Sqrt(det);
                    var t1 = (-fb - det) / 2;
                    var t2 = (-fb + det) / 2;
                    if (t1 > t2)
                    {
                        var swap = t1;
                        t1 = t2;
                        t2 = swap;
                    }
                    if (t1 <= 0) t1 = t2;
                    if (t1 > 0)
                    {
                        shootVector = rel + vel * t1;
                    }
                }

                Quaternion shootDirection = Quaternion.FromToRotation(Vector3.up, shootVector);

                GameObject.Instantiate(projectile, transform.position, shootDirection);

                core.Regrow();
                yield return new WaitUntil(core.FullyGrown);
            }
        }

        yield return null;
    }

}
