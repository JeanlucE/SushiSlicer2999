using UnityEngine;
using System.Collections;

public class Knockout : MonoBehaviour {

    [Range(5f, 120f)]
    public float torpor;

	void OnColliderEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController.main.Knock(torpor);
            Destroy(gameObject);
        }
    }

}
