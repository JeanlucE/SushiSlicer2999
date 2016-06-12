using UnityEngine;
using System.Collections;

public class Confusion : MonoBehaviour {

    public Transform prefab;
    public int amount;
    public float size;

    public float radius;

    public float rotationSpeed;
    public float starSpeed;

    private Transform[] childs;
    private float rotation;
    private float starRotation;

    private float currentSize = 0.0f;
    private float currentSizeSpeed = 2.0f;

	void Start ()
    {
        childs = new Transform[0];
        Rebuild();
	}

    void Rebuild()
    {
        if (childs.Length != amount)
        {
            foreach (Transform t in childs)
            {
                Destroy(t.gameObject);
            }

            Vector3 position = Vector3.up * radius;

            childs = new Transform[amount];

            float wdelta = 360.0f / amount;
            for (int i=0; i<amount; i++)
            {
                Transform t = GameObject.Instantiate<Transform>(prefab);
                t.parent = transform;
                t.localScale = Vector3.zero;
                t.localPosition = Quaternion.AngleAxis(wdelta * i, Vector3.forward) * position;

                childs[i] = t;
            }
        }
    }

    void Update()
    {
        rotation += 360 * rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.AngleAxis(rotation, Vector3.forward);

        Vector3 vscale = Vector3.one * currentSize;
        currentSize += 8.0f * currentSizeSpeed * Time.deltaTime;
        Mathf.SmoothDamp(currentSize, size, ref currentSizeSpeed, 0.2f, 10.0f, 0.1f * Time.deltaTime);

        Quaternion star = Quaternion.AngleAxis(starRotation, Vector3.forward);
        //Debug.Log(childs);
        foreach (Transform t in childs)
        {
            if (t)
            {
                t.localRotation = star;
                t.localScale = vscale;
                starRotation += 360 * starSpeed * Time.deltaTime;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
        Vector3 position = Vector3.up * radius;

        childs = new Transform[amount];

        float wdelta = 360 / amount;
        for (int i = 0; i < amount; i++)
        {
            Vector3 circlePosition = transform.position + transform.rotation * Quaternion.AngleAxis(wdelta * i, Vector3.forward) * position;
            Gizmos.DrawWireSphere(circlePosition, size * 0.3f);
        }
    }
}
