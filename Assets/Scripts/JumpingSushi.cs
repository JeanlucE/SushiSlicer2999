using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animation))]
public class JumpingSushi : MonoBehaviour 
{
    private Animation animation;

	// Use this for initialization
	void Start () 
    {
        animation = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!animation.isPlaying) { Destroy(this.gameObject); }
	}
}
