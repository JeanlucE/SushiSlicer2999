using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
    public EnemyType type;

	// Use this for initialization
	void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
	    switch(type)
        {
            case EnemyType.Fish:
                //movement
                //shooting
                break;
            case EnemyType.Rice:
                {
                    //movement
                    //shooting
                }
                break;
            default:
                {
                    //default movement
                    //shooting
                }
                break;
        }
	}

    public enum EnemyType
    {
        Fish, Rice
    }
}
