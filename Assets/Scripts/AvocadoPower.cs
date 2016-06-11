using UnityEngine;
using System.Collections;

public class AvocadoPower : MonoBehaviour {

    public GameObject projectile;

    public float respawnMin = 0.9f, respawnMax = 10.0f;

    Coroutine cr;

    void Start()
    {
        cr = StartCoroutine(AvocadoBrain());
	}

    void Destroy()
    {

    }

    IEnumerator AvocadoBrain()
    {
        while (true)
        {
            float waitTime = Random.Range(respawnMin, respawnMax);
            yield return new WaitForSeconds(waitTime);

            Transform player = PlayerController.main.transform;
            Vector3 playerPosition = player.position + 2.0f * (Vector3) Random.insideUnitCircle;

            Quaternion shootDirection = Quaternion.FromToRotation(Vector3.right, playerPosition - transform.position);

            GameObject.Instantiate(projectile, transform.position, shootDirection);
        }
    }

}
