using UnityEngine;
using System.Collections;

public class SpawnObject : MonoBehaviour
{

    public GameObject[] prefabs;
    
	void Start ()
    {
        foreach (GameObject prefab in prefabs)
        {
            GameObject.Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }

}
