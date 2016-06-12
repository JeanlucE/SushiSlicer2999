using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Sushi/EnemyData")]
public class EnemyData : ScriptableObject {

    public int score;
    public GameObject Icon;

    public GameObject[] variants;

    public GameObject SummonAt(Vector3 at, Transform parent = null)
    {
        GameObject instance = null;

        if (variants.Length > 0)
        {
            GameObject chosen = variants[Random.Range(0, variants.Length)];
            instance = GameObject.Instantiate<GameObject>(chosen);
        }
        else
        {
            Debug.Log("Summoned empty Enemy " + this.name + "!");
            instance = new GameObject(this.name);
        }

        instance.transform.parent = parent;
        instance.transform.localPosition = at;

        return instance;
    }

}
