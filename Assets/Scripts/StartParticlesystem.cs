using UnityEngine;
using System.Collections;

public class StartParticlesystem : MonoBehaviour
{

    void Start () {
	    foreach (ParticleSystem system in GetComponents<ParticleSystem>())
        {
            system.Play(true);
        }
	}

}
