using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour {

    public GameObject slicePrefab;

    public void Slice(Vector3 from, Vector3 to)
    {
        SpriteSlicer.SliceAll(from, to);

        if (slicePrefab)
        {
            GameObject obj = GameObject.Instantiate<GameObject>(slicePrefab);
            obj.transform.localPosition = from;
            obj.transform.localRotation = Quaternion.FromToRotation(Vector3.right, to - from);
        }
    }

}
