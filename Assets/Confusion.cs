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

	void Start ()
    {
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
                t.localScale = Vector3.one * size;
                t.localPosition = Quaternion.AngleAxis(wdelta * i, Vector3.forward) * position;

                childs[i] = t;
            }
        }
    }

    void Update()
    {
        rotation += 360 * rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.AngleAxis(rotation, Vector3.forward);

        starRotation += 360 * starSpeed * Time.deltaTime;
        Quaternion star = Quaternion.AngleAxis(rotation, Vector3.forward);
        foreach (Transform t in childs)
        {
            t.localRotation = star;
        }
    }
}
