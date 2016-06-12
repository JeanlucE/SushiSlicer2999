using UnityEngine;
using System.Collections;

public class AlgePower : MonoBehaviour {

    public Vector3 mouthPosition;
    public GameObject tentakelPrefab;

    public MouthController mc;

    private IEnumerator cr;

    void Start()
    {
        StartCoroutine(cr = TentakelBrain());
    }

    void OnDisable()
    {
        StopCoroutine(cr);
    }

    IEnumerator TentakelBrain()
    {
        while (this)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 7.0f));

            mc.SetState(true);

            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));

            SpawnTentakel(Quaternion.AngleAxis(-80f, Vector3.forward));
            SpawnTentakel(Quaternion.AngleAxis(-90f, Vector3.forward));
            SpawnTentakel(Quaternion.AngleAxis(-100f, Vector3.forward));

            yield return new WaitForSeconds(0.5f);

            mc.SetState(false);
        }

        yield return null;
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
