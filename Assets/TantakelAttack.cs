using UnityEngine;
using System.Collections;

public class TantakelAttack : MonoBehaviour {

    int wallMask;

    void Start()
    {
        wallMask = LayerMask.NameToLayer("Wall");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController.main.Slowdown(0.5f);
            Destroy(gameObject);
        }
        if (other.gameObject.layer == wallMask)
        {
            Destroy(gameObject);
        }
    }

}
