using UnityEngine;
using System.Collections;

public class SetVelocity : MonoBehaviour {

    public Vector3 velocity;
    public bool localSpace;

	void Start ()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb)
        {
            rb.velocity = localSpace ? transform.TransformVector(velocity) : velocity;
        }
	}

}
