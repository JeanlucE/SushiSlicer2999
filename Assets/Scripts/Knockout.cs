using UnityEngine;
using System.Collections;

public class Knockout : MonoBehaviour {

    [Range(5f, 120f)]
    public float torpor;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController.main.Knock(torpor, 0.5f * GetComponent<Rigidbody2D>().velocity);
            Destroy(gameObject);
        }
    }

}
