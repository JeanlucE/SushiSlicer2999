using UnityEngine;
using System.Collections;

public class AlgePower : MonoBehaviour {

    public Vector3 mouthPosition;
    public GameObject tentakelPrefab;

    private Coroutine cr;

    void Start()
    {
        cr = StartCoroutine(TentakelBrain());
    }

    void OnDestroy()
    {
        StopCoroutine(cr);
    }

    IEnumerator TentakelBrain()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 7.0f));

            SpawnTentakel(Quaternion.AngleAxis(-80f, Vector3.forward));
            SpawnTentakel(Quaternion.AngleAxis(-90f, Vector3.forward));
            SpawnTentakel(Quaternion.AngleAxis(-100f, Vector3.forward));
        }
    }

    void SpawnTentakel(Quaternion direction)
    {
        GameObject tentakel = GameObject.Instantiate<GameObject>(tentakelPrefab);
        tentakel.transform.position = transform.TransformPoint(mouthPosition);
        tentakel.transform.rotation = transform.rotation * direction;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.TransformPoint(mouthPosition), 1);
    }

}
