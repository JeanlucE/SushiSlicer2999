using UnityEngine;
using System.Collections;

public class AvocadoPower : MonoBehaviour {

    public GameObject projectile;
    public AvocadoCore core;

    public float respawnMin = 0.9f, respawnMax = 10.0f;

    private Texture avocadoTemp;
    private Coroutine cr;

    void Start()
    {
        cr = StartCoroutine(AvocadoBrain());
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
        while (true)
        {
            float waitTime = Random.Range(respawnMin, respawnMax);
            yield return new WaitForSeconds(waitTime);

            Transform player = PlayerController.main.transform;
            Vector3 playerPosition = player.position + 2.0f * (Vector3) Random.insideUnitCircle;

            Quaternion shootDirection = Quaternion.FromToRotation(Vector3.up, playerPosition - transform.position);

            GameObject.Instantiate(projectile, transform.position, shootDirection);

            core.Regrow();
            yield return new WaitUntil(core.FullyGrown);
        }
    }

}
