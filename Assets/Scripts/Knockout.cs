using UnityEngine;
using System.Collections;

public class Knockout : MonoBehaviour {

    [Range(5f, 120f)]
    public float torpor;

    public GameObject debris;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Vector3 v = GetComponent<Rigidbody2D>().velocity;
            PlayerController.main.Knock(torpor, 0.5f * v);
            if (debris)
            {
                GameObject.Instantiate(debris, transform.position, Quaternion.FromToRotation(Vector3.right, -v));
            }
            Destroy(gameObject);
        }
    }

}
